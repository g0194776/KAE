using System;
using System.IO;
using KJFramework.Dynamic.Components;

namespace KJFramework.Platform.Deploy.DSC
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicDomainService dynamicServiceCenter = new DynamicDomainService();
            dynamicServiceCenter.Start();
            while (true)
            {
                string commod = Console.ReadLine();
                string[] cc = commod.Split(' ');
                switch (cc[0])
                {
                    case "GetServices":
                        Console.WriteLine();
                        //dynamicServiceCenter.GetServiceInfomation();
                        break;
                    case "Update":
                        Console.WriteLine();
                        //dynamicServiceCenter.UpdateComponent();
                        break;
                    case "GetHealth":
                        Console.WriteLine();
                        //dynamicServiceCenter.GetHealth();
                        break;
                    case "Reset":
                        Console.WriteLine();
                        //dynamicServiceCenter.Reset(cc[1]);
                        break;
                    case "ResetS":
                        Console.WriteLine();
                        //dynamicServiceCenter.ResetS(cc[1]);
                        break;
                    case "ResetD":
                        Console.WriteLine();
                        //dynamicServiceCenter.ResetD(cc[1]);
                        break;
                    case "GetFiles":
                        Console.WriteLine();
                        //dynamicServiceCenter.GetFiles(cc[1]);
                        break;
                }
            }
        }
    }
}
