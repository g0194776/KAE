using System;
using System.IO;

namespace KJFramework.ApplicationEngine.KAEWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            KAEHost host = new KAEHost(Path.GetFullPath("."));
            host.Start();
            Console.ReadLine();
        }
    }
}
