using System;
using System.Threading;
using KJFramework.ServiceModel.Bussiness.Default.Proxy;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Objects;
using KJFramework.ServiceModel.Proxy;
using KJFramework.Timer;
using ServerTest.Contract;

namespace ClientTest
{
    class Program
    {
        private static int callbackcount;
        static void Main(string[] args)
        {
            Program program = new Program();
            Console.WriteLine("Finished.");
            Console.ReadLine();
        }
    }
}
