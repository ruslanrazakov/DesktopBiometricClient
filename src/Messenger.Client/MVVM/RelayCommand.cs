using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.Client.MVVM
{
    /// <summary>
    /// Синхронная имплементация паттерна Command
    /// </summary>
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            try
            {
                this.execute(parameter);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                var handler = new LogHandlerConsole();
                handler?.Log(ex.ToString());
            }
        }
    }
}
