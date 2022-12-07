using Messenger.Client.MVVM;
using Messenger.Client.Utils;
using Messenger.Core;
using Messenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ClientSettings = Messenger.Client.Utils.SettingsHelper;

namespace Messenger.Client.ViewModels
{
    class MainContentViewModel : ViewModelBase, IPageViewModel
    {
        #region IPageViewModel members
        public string Name => throw new NotImplementedException();

        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public void ViewModelClosed()
        {
            _isActive = false;
        }

        public void ViewModelOpened() 
        {
            Engine = ClientSettings.GetEngine(optionsPath).ToString();
            IsActive = true;
        }

        public IPageViewModel SetChildPage(ApplicationPages page)
            => CurrentChildViewModel = ChildViewModels[(int)page];

        public ApiResponse ApiResponse { get; set; }
        #endregion

        List<IPageViewModel> _childViewModels;
        public List<IPageViewModel> ChildViewModels
        {
            get
            {
                if (_childViewModels == null)
                    _childViewModels = new List<IPageViewModel>();

                return _childViewModels;
            }
        }

        IPageViewModel _currentChildViewModel;
        public IPageViewModel CurrentChildViewModel
        {
            get => _currentChildViewModel;
            set
            {
                if (_currentChildViewModel != value)
                    SetProperty(ref _currentChildViewModel, value);
            }
        }

        WriteableBitmap _bitmap;
        public WriteableBitmap Bitmap
        {
            get => _bitmap;
            set => SetProperty(ref _bitmap, value);
        }

        bool _cameraOn;
        public bool CameraOn
        {
            get => _cameraOn;
            set
            {
                SetProperty(ref _cameraOn, value);

                if (_cameraOn) StartStreaming();
                else StopStreaming();
            }
        }

        bool _faceDetected;
        public bool FaceDetected
        {
            get => _faceDetected;
            set
            {
                SetProperty(ref _faceDetected, value);
            }
        }

        double _liveness;
        public double Liveness
        {
            get => _liveness;
            set => SetProperty(ref _liveness, value);

        }

        string _engine;
        public string Engine
        {
            get => _engine;
            set => SetProperty(ref _engine, value);

        }

        StatusBarBehavior _statusBarBehavior;
        public StatusBarBehavior StatusBarBehavior
        {
            get => _statusBarBehavior;
            set => SetProperty(ref _statusBarBehavior, value);
        }

        string optionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.json");

        Messenger.Core.VideoStream _videoStream;
        Messenger.Core.FramesHutch _framesHutch;
        Messenger.Core.ApiGate _apiGate;
        Messenger.Core.TimerAsync _timerAsync;
        Messenger.Client.MVVM.ILogHandler _logHandler;
        CancellationTokenSource _timerCancellationTokenSource;

        public MainContentViewModel()
        {
            _videoStream = new();
            _framesHutch = new();
            _apiGate = new();
            _logHandler = new Messenger.Client.MVVM.LogHandlerConsole();
            _timerAsync = new TimerAsync(delay: 5000);

            StatusBarBehavior = new StatusBarBehavior()
            {
                Message = "Камера выключена",
                IsBad = false
            };

            ChildViewModels.AddRange(new List<IPageViewModel>()
            {
                new DetectedPersonInfoViewModel(),
                new RegisterPersonViewModel(Bitmap)
            });
            CurrentChildViewModel = ChildViewModels.First();
        }

        private void StartStreaming()
        {
            StatusBarBehavior.Message = "Камера включена";
            StatusBarBehavior.IsBad = false;

            try
            {
                Liveness = 0;

                _videoStream.FrameUpdated += VideoStream_FrameUpdated;
                _videoStream.FaceCaptured += VideoStream_FaceCaptured;
                _videoStream.FaceLost += VideoStream_FaceLost;

                Task.Run(() => _videoStream.Start());
            }
            catch (Exception ex)
            {
                _logHandler.Log(ex.ToString());
            }
        }

        private void StopStreaming()
        {
            StatusBarBehavior.Message = "Камера выключена";
            StatusBarBehavior.IsBad = false;

            Liveness = 0;
            
            _timerCancellationTokenSource?.Cancel();
            _videoStream.Stop();
            _videoStream.FrameUpdated -= VideoStream_FrameUpdated;
            _videoStream.FaceCaptured -= VideoStream_FaceCaptured;
            _videoStream.FaceLost -= VideoStream_FaceLost;
        }

        private void VideoStream_FrameUpdated(object sender, EventArgs e)
        {
            Bitmap = _videoStream._bitmap;
        }

        private void VideoStream_FaceCaptured(object sender, EventArgs e)
        {
            _logHandler.Log("Face detected");
            StatusBarBehavior.Message = "Обнаружено лицо, обмен данными через 5 секунд...";
            StatusBarBehavior.IsBad = false;

            FaceDetected = true;
            ChildViewModels.ForEach(childVM => childVM.IsActive = true);
            _timerCancellationTokenSource = new();

            Task.Run(async () =>
            {
                await _timerAsync.Start(async () =>
                {
                    if (IsActive)
                        await CatchAndSendFrames();

                }, _timerCancellationTokenSource.Token);
            }, _timerCancellationTokenSource.Token);
        }

        private void VideoStream_FaceLost(object sender, EventArgs e)
        {
            _timerCancellationTokenSource?.Cancel();
            FaceDetected = false;
            StatusBarBehavior.Message = "Лиц в кадре нет";
            StatusBarBehavior.IsBad = false;
            _logHandler.Log("Face lost");
            ChildViewModels.ForEach(childVM => childVM.IsActive = false);
        }

        private async Task CatchAndSendFrames()
        {
            try
            {
                ApiResponse response = new();
                foreach (var frames in Enumerable.Range(0, 1))
                {
                    var frameBase64 = _framesHutch.GetBase64Frame(Bitmap);
                    bool frameAdded = _framesHutch.Add(frameBase64);
                    if (_framesHutch.IsFull())
                    {
                        byte[] videoBytes = ClientSettings.GetEngine(optionsPath) == Core.Entities.Engine.NTech ?
                                                           await _videoStream.GetVideoPartFromStream(seconds: 2) :
                                                           new byte[0];
                        _logHandler.Log("Hutch full, starting api gate...");
                        response = await _apiGate.PostDataGetLiveness(_framesHutch.Frames, ClientSettings.GetEngine(optionsPath), videoBytes);
                        StatusBarBehavior = UpdateStatusBar(response.ResponseStatus);
                        _logHandler.Log(response.ResponseStatus.ToString());
                        _framesHutch.Clear();
                        break;
                    }

                    _logHandler.Log("Frame added");
                    await Task.Delay(100);
                }
                Liveness = response.Liveness;
                ChildViewModels.ForEach(childVM => childVM.ApiResponse = response);
            }
            catch (Exception ex)
            {
                _logHandler.Log(ex.ToString());
                MVVM.ILogHandler notepadeLogger = new ErrorHandlerNotepad();
                notepadeLogger.Log(ex.ToString());

                StatusBarBehavior.Message = "Что-то пошло не так. Подробности в log.txt";
                StatusBarBehavior.IsBad = true;
            }
        }

        private StatusBarBehavior UpdateStatusBar(ResponseStatus status) => status switch
        {
            ResponseStatus.Ok => new StatusBarBehavior() 
            {
                Message = $"Обмен данными прошел успешно в {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}",
                IsBad = true
            },
            ResponseStatus.EmptyApiGateString => new StatusBarBehavior()
            {
                Message = "Пустая строка с url до external API в Options.json",
                IsBad = true
            },
            ResponseStatus.WrongApiGateString => new StatusBarBehavior() 
            {
                Message = "Невалидный url до external API в Options.json",
                IsBad = true
            },
            _ =>  new StatusBarBehavior() 
            {
                Message = "Что то пошло не так. Подробности в лог файле.", 
                IsBad = true
            }
        };
    }
}
