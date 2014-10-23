using System;

namespace KJFramework.ApplicationEngine.KAEWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            KAEHost host = new KAEHost();
            host.Start();
            Console.ReadLine();
        }
    }
}
