using Messenger.Client.MVVM;
using Messenger.Client.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Messenger.Client
{
    class MainVewModel : ViewModelBase
    {
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

        public ICommand OpenRegisterViewCommand { get; }
        public ICommand OpenDetectionViewCommand { get; }
        public ICommand OpenSettingsViewCommand { get; }

        public MainVewModel()
        {
            ChildViewModels.AddRange(new List<IPageViewModel>()
            {
                new MainContentViewModel(),
                new SettingsViewModel()
            });

            CurrentChildViewModel = ChildViewModels.First();
            CurrentChildViewModel.ViewModelOpened();

            bool canOpenView = true;
            OpenRegisterViewCommand = new RelayCommand(_ => GetRegisterView(), _ => { return canOpenView; });
            OpenDetectionViewCommand = new RelayCommand(_ => GetDetectionView(), _ => { return canOpenView; });
            OpenSettingsViewCommand = new RelayCommand(_ => GetSettingsView(), _ => { return canOpenView; });

        }

        private void GetDetectionView()
        {
            CurrentChildViewModel.ViewModelClosed();
            CurrentChildViewModel = ChildViewModels[(int)ApplicationPages.MainContent];
            CurrentChildViewModel.SetChildPage(ApplicationPages.Detection).ViewModelOpened();
            CurrentChildViewModel.ViewModelOpened();
        }

        private void GetRegisterView()
        {
            CurrentChildViewModel.ViewModelClosed();
            CurrentChildViewModel = ChildViewModels[(int)ApplicationPages.MainContent];
            CurrentChildViewModel.SetChildPage(ApplicationPages.Register).ViewModelOpened();
            //CurrentChildViewModel.ViewModelOpened();
        }

        private void GetSettingsView()
        {
            CurrentChildViewModel.ViewModelClosed();
            CurrentChildViewModel = ChildViewModels[(int)ApplicationPages.Settings];
            CurrentChildViewModel.ViewModelOpened();
        }
    }

    public enum ApplicationPages
    {
        /// <summary>
        /// Child pages of MainWindow
        /// </summary>
        MainContent = 0,
        Settings = 1,

        /// <summary>
        /// Child pages of MainContent
        /// </summary>
        Detection = 0,
        Register = 1,
    }
}
