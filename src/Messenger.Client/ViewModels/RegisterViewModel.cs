using Messenger.Client.Models;
using Messenger.Client.MVVM;
using Messenger.Core.Entities;
using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.Client.ViewModels
{
    class RegisterPersonViewModel : ViewModelBase, IPageViewModel
    {
        #region IPageViewModel members
        public string Name => throw new NotImplementedException();

        bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
        public void ViewModelClosed() { }

        public void ViewModelOpened() { }

        public IPageViewModel SetChildPage(ApplicationPages page) => this;
        public ApiResponse ApiResponse { get; set; }
        #endregion

        

        Person _person;
        public Person Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }

        public ICommand RegisterCommand;

        public RegisterPersonViewModel(WriteableBitmap bitmap)
        {
            bool canRegister = true;
            RegisterCommand = new RelayCommand(_ => RegisterPerson(), _ => canRegister);
        }

        private void RegisterPerson()
        {
            
        }
    }
}
