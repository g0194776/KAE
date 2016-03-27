using System;
using System.IO;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.Maintenance
{
    class Program
    {
        static void Main(string[] args)
        {
            MaintenanceDynamicDomainService service = new MaintenanceDynamicDomainService();
            service.Start();
            while (true)
            {
                string commod = Console.ReadLine();
                string[] cc = commod.Split(' ');
                switch (cc[0])
                {
                    case "reset":
                        Console.WriteLine();
                        //service.Schedule(new DynamicServiceResetHeartBeatTimeRequestMessage { Interval = int.Parse(cc[1]) });
                        break;
                    case "update":
                        Console.WriteLine();
                        //service.Schedule(new DynamicServiceUpdateComponentRequestMessage { ComponentName = cc.Length == 1 ? "*ALL*" : cc[1] });
                        break;
                    case "gethealth":
                        Console.WriteLine();
                        //service.Schedule(new DynamicServiceGetComponentHealthRequestMessage { Components = new[] { "*ALL*" } });
                        break;
                    case "getfiles":
                        Console.WriteLine();
                        //service.Schedule(cc.Length == 1
                        //                     ? new DynamicServiceGetFileInfomationRequestMessage() {Files = "*ALL*"}
                        //                     : new DynamicServiceGetFileInfomationRequestMessage() {Files = cc[1]});
                        break;
                }
            }
        }
    }
}
