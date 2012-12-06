using System;
using KJFramework.ServiceModel.Bussiness.Default.Services;
using KJFramework.ServiceModel.Elements;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(HellowWorld), new TcpBinding("tcp://localhost:9999/Test"));
            serviceHost.IsSupportExchange = true;
            serviceHost.Opened += (sender, e) =>
            {
                Console.WriteLine("Service has been opened.");
            };
            serviceHost.Open();
            Console.ReadLine();
        }
    }
}
