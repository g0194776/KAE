using System;

namespace KJFramework.ApplicationEngine.ApplicationTest2
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
