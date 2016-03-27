using System;
using KJFramework.Dynamic.Components;

namespace KJFramework.Platform.Deploy.SMC
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicDomainService dynamicService = new DynamicDomainService();
            dynamicService.Start();
            Console.ReadLine();
        }
    }
}
