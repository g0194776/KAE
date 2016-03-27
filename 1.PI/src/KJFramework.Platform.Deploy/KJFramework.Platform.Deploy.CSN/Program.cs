using System;
using KJFramework.Dynamic.Components;

namespace KJFramework.Platform.Deploy.CSN
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
