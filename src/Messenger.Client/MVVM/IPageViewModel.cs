using Messenger.Client.Utils;
using Messenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Client.MVVM
{
    public interface IPageViewModel
    {
        string Name { get; }
        bool IsActive { get; set; }
        void ViewModelClosed();
        void ViewModelOpened();
        IPageViewModel SetChildPage(ApplicationPages page);

        /// <summary>
        /// Убрать в отдельный интерфейс, нарушение interface segregation principe!
        /// </summary>
        ApiResponse ApiResponse { get; set; }
    }
}