using System;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Core
{
    public class TimerAsync
    {
        private CancellationTokenSource cts;
        public int Delay { get; private set; }

        public TimerAsync(int delay)
        {
            Delay = delay;
        }

        public async Task Start(Func<Task> task, CancellationToken token)
        {
            cts = new CancellationTokenSource();

            while (!token.IsCancellationRequested)
            {
                await Task.Delay(Delay);

                //еще раз проверяем токен отмены перед вызовом метода
                if(!token.IsCancellationRequested)
                    await task();
            }
        }

        public async Task Stop(Func<Task> task)
        {
            await task();
            cts?.Cancel();
        }

        public void Stop()
        {
            cts?.Cancel();
        }
    }
}
