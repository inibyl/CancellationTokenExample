using System;
using System.Threading;
using System.Threading.Tasks;

namespace TokenTest
{
    class Program
    {
        private static CancellationToken cancellationToken;
        private static CancellationTokenSource source;

        static void Main(string[] args)
        {
            source = new CancellationTokenSource();
            cancellationToken = source.Token;
            // метод "OnCancelled" будет вызываться при отмене задачи
            source.Token.Register(OnCancelled);

            Task.Run(() =>
            {

                using (source.Token.Register(Thread.CurrentThread.Abort))
                {
                    DoSomething();
                }

            }, cancellationToken);

            // отменяем через 5 минут
            Thread.Sleep(5000);
            source.Cancel();
        }

        static void DoSomething()
        {
            for (int i = 0;
                i < 10;
                i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("+");
            }
        }

        static void OnCancelled()
        {
            Console.WriteLine("ЗАДАЧА ОТМЕНЕНА!");
        }
    }
}
