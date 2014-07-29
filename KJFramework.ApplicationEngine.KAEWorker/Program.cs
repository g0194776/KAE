using System;
using System.IO;
using System.Net;

namespace KJFramework.ApplicationEngine.KAEWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemWorker.Instance.Initialize("KAEWorker", true);
            KAEHost host = new KAEHost(Path.GetFullPath("."), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6611));
            host.Start();
            Console.ReadLine();
        }
    }
}
