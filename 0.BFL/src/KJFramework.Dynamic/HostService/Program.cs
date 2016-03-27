using System;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;

namespace HostService
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicDomainService service = new DynamicDomainService(new ServiceDescriptionInfo
                                             {
                                                 Name = "Host Service", 
                                                 ServiceName = "Host Service Name"
                                             });
            service.Start();
            string content;
            while ((content = Console.ReadLine()) != "exit")
            {
                if (content.Contains("update"))
                {
                    string[] orders = content.Split(' ');
                    service.Update(orders[1]);
                }
                else if (content == "restart") service.Update();
            }
        }
    }
}
