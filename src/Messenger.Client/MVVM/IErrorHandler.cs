using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Client.MVVM
{
    /// <summary>
    /// Интерфейс для сервисов перехватывания и логгирования служебныx сообщений
    /// </summary>
    public interface ILogHandler
    {
        void Log(string message);
    }

    /// <summary>
    /// Реализация ErrorHandler для логгирования в txt файл
    /// </summary>
    public class ErrorHandlerNotepad : ILogHandler
    {
        public void Log(string e)
        {
            System.Diagnostics.Debug.WriteLine(e);

            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            string content = $"{DateTime.Now} >> {e} \n\n";
            File.AppendAllText(logFilePath, content);
        }
    }

    /// <summary>
    /// Реализация ErrorHandler для логгирования в консоль
    /// </summary>
    public class LogHandlerConsole : ILogHandler
    {

        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public LogHandlerConsole()
        {
            AllocConsole();
            Console.WriteLine("test");
        }

        public void Log(string message)
        {
            Console.WriteLine(DateTime.Now + " >>>");
            Console.WriteLine(message);
        }
    }
}
