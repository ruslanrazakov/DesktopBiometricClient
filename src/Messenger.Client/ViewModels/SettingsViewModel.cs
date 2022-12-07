using Messenger.Client.Models;
using Messenger.Client.MVVM;
using Messenger.Client.Utils;
using Messenger.Core.Entities;
using System;
using System.IO;
using System.Windows.Input;

namespace Messenger.Client.ViewModels
{
    class SettingsViewModel : ViewModelBase, IPageViewModel
    {
        #region IPageViewModel members
        public string Name => throw new NotImplementedException();

        public bool IsActive { get; set; }

        public IPageViewModel SetChildPage(ApplicationPages page) => this;

        public void ViewModelClosed() { }

        public void ViewModelOpened() 
        {
            CanEditSettings = true;
        }

        public ApiResponse ApiResponse { get; set; }
        #endregion

        bool _canEditSettings;
        public bool CanEditSettings
        {
            get => _canEditSettings;
            set => SetProperty(ref _canEditSettings, value);
        }

        public Utils.SettingsEntity _settings;
        public Utils.SettingsEntity Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        public ICommand SaveCommand { get; }
        string optionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.json");

        public SettingsViewModel()
        {
            Settings = SettingsHelper.Get(optionsPath);

            SaveCommand = new RelayCommand(_ => Save(), _ => true);
        }

        private void Save()
        {
            SettingsHelper.Save(Settings, optionsPath);
            CanEditSettings = false;
        }
    }
}
