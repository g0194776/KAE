using System;
using KJFramework.Dynamic.Components;

namespace KJFramework.Platform.Deploy.DSN
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicDomainService dynamicServiceCenter = new DynamicDomainService();
            dynamicServiceCenter.Start();
            Console.ReadLine();
        }
    }
}
