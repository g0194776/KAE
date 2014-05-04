using System;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;

namespace KJFramework.ApplicationEngine.RRCS
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicDomainService service = new DynamicDomainService(new ServiceDescriptionInfo
            {
                Name = "KAE Rules Routine Controller Service",
                ServiceName = "KJFramework.ApplicationEngine.RRCS",
                Version = "1.0.0",
                Description = "A service that which it'll manages whole KAE business remoting adddresses."
            });
            service.Start();
            Console.ReadLine();
        }
    }
}
