using Messenger.Client.Models;
using Messenger.Client.MVVM;
using Messenger.Client.Utils;
using Messenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Client.ViewModels
{
    class DetectedPersonInfoViewModel : ViewModelBase, IPageViewModel
    {
        #region IPageViewModel members
        public string Name => throw new NotImplementedException();

        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                SetProperty(ref _isActive, value);
                if(!_isActive)
                {
                    Person = new Person();
                }
            }
        }

        public void ViewModelClosed() { }

        public void ViewModelOpened() 
        {
            Settings = SettingsHelper.Get(optionsPath);
        }

        public IPageViewModel SetChildPage(ApplicationPages page) => this;

        ApiResponse _apiResponse;
        public ApiResponse ApiResponse 
        {
            get => _apiResponse;
            set
            {
                SetProperty(ref _apiResponse, value);
                Person = new Person()
                {
                    IsPersonAccepted = _apiResponse.MatchScore > 0.8 ? true : false,
                };
            }
        }
        #endregion

        Person _person;
        public Person Person
        {
            get => _person;
            set
            {
                SetProperty(ref _person, value);
            }
        }

        string _personAccepted;
        public string PersonAccepted
        {
            get => _personAccepted;
            set
            {
                _personAccepted = _apiResponse.MatchScore > 0.8 ? "Зарегистрированная персона" : "Незарегистрированный пользователь!";
                SetProperty(ref _personAccepted, value);
            }
        }

        public Utils.SettingsEntity _settings;
        public Utils.SettingsEntity Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        string optionsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Options.json");

        public DetectedPersonInfoViewModel()
        {
           ApiResponse = new ApiResponse();
           Settings = SettingsHelper.Get(optionsPath);
        }
    }
}
