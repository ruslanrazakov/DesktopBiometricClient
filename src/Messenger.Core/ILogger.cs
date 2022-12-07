using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Core
{
    /// <summary>
    /// Интерфейс для сервисов перехватывания и логгирования служебный сообщений
    /// </summary>
    public interface ILogHandler
    {
        void Log(string message);
    }

    /// <summary>
    /// Реализация ILogHandler для логгирования в консоль
    /// </summary>
    public class LogHandlerConsole : ILogHandler
    {
        public void Log(string message)
        {
            Console.WriteLine(DateTime.Now + " >>>");
            if(message.Count() > 100)
                message = message[..100] + "....";
            Console.WriteLine(message + Environment.NewLine);
        }
    }
}
