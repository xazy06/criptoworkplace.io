using System;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).Wait();
            Console.ReadKey();
        }

        private static Task MainAsync(string[] args)
        {
            return new App().Run();
        }
    }
}
