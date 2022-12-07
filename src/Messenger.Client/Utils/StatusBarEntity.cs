using Messenger.Client.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Client.Utils
{
    public class StatusBarBehavior : ViewModelBase
    {
        string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);

        }

        bool _isBad;
        public bool IsBad
        {
            get => _isBad;
            set => SetProperty(ref _isBad, value);
        }
    }
}
