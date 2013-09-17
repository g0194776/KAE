using System;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;

namespace KJFramework.Platform.Deploy.CSN
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicDomainService service = new DynamicDomainService(new ServiceDescriptionInfo
            {
                Description = "CSN服务",
                Name = "CSNe",
                ServiceName = "CSN",
                Version = "0.0.0.1"
            });
            service.Start();
            Console.ReadLine();
        }
    }
}
