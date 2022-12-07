using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Client.MVVM
{
    public static class TaskUtilities
    {
        /// <summary>
        /// Fire and forget расширения для AsyncCommand
        /// </summary>
        /// <param name="task">Объект задачи</param>
        /// <param name="handler"></param>
        public static async void FireAndForgetSafeAsync(this Task task, ILogHandler handler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler = new ErrorHandlerNotepad();
                handler?.Log(ex.ToString());
            }
        }
    }
}
