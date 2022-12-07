using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Messenger.Core
{
    public class VideoStream
    {
        public WriteableBitmap _bitmap;
        public event EventHandler FrameUpdated;
        public event EventHandler FaceCaptured;
        public event EventHandler FaceLost;

        private bool _faceIsInFrame;
        private CancellationTokenSource _cancelToken = new();
        private VideoCapture _capture;
        private CascadeClassifier _cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
        private Size _videoSize = new OpenCvSharp.Size(250, 200);
        private Size _faceDetectionSize = new OpenCvSharp.Size(30, 30);
        private Rect[] _rects;

        private string _videoTempPath = AppDomain.CurrentDomain.BaseDirectory + "/temp.mp4";

        public async Task Start()
        {
            _capture = new VideoCapture(0, VideoCaptureAPIs.ANY);
            _cancelToken = new();
            await StartAnimation();
        }

        private async Task StartAnimation()
        {
            while (!_cancelToken.IsCancellationRequested)
            {
                using (var frameMat = _capture.RetrieveMat())
                {
                    _capture.Read(frameMat);
                    _rects = _cascadeClassifier.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionTypes.ScaleImage, _faceDetectionSize);
                    
                    //рисует квадрат вокруг лица. удобно, но лица с квадратами движки считают за фейк. 
                    /*foreach (var rect in _rects)
                    {
                        Cv2.Rectangle(frameMat, rect, Scalar.Red);
                    }*/

                    //при обнаружении или потере лица в кадре один раз отсылаем соответствующее событие
                    if (_rects.Length > 0 && !_faceIsInFrame)
                    {
                        _faceIsInFrame = true;
                        FaceCaptured?.Invoke(null, EventArgs.Empty);
                    }
                    else if (_rects.Length == 0 && _faceIsInFrame)
                    {
                        _faceIsInFrame = false;
                        FaceLost?.Invoke(null, EventArgs.Empty);
                    }

                    _bitmap = frameMat.ToWriteableBitmap();
                    _bitmap.Freeze();

                    FrameUpdated?.Invoke(null, EventArgs.Empty);
                    await Task.Delay(30);
                }
            }
            _capture.Release();
            _faceIsInFrame = false;
        }

        public async Task<byte[]> GetVideoPartFromStream(int seconds)
        {
            var timeTo = DateTime.Now.Add(new TimeSpan(hours: 0, minutes: 0, seconds: seconds));

            using (var writer = new VideoWriter(_videoTempPath, -1, 10, _videoSize))
            using (var frameMat = _capture.RetrieveMat())
            {
                while (DateTime.Now < timeTo)
                {
                    _capture.Read(frameMat);

                    using var canny = new Mat();
                    Cv2.Resize(frameMat, canny, _videoSize, 0, 0, InterpolationFlags.Linear);
                    writer.Write(canny);

                    await Task.Delay(100);
                }
            }

            return File.ReadAllBytes(_videoTempPath);
        }

        public void Stop()
        {
            _cancelToken?.Cancel();
        }
    }
}
