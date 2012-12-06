using System;
using System.Net;
using KJFramework.Net.Channels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.Client.Test
{
    class Program
    {
        private static IRequestScheduler<CSNMessage> _scheduler;
        static void Main(string[] args)
        {
            _scheduler = new RequestScheduler<CSNMessage>(100);
            ITransportChannel channel = new TcpTransportChannel(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11010));
            NetworkNode<CSNMessage> networkNode = new NetworkNode<CSNMessage>(new CSNProtocolStack());
            networkNode.ProtocolStack.Initialize();
            _scheduler.Regist(networkNode);
            _scheduler.Regist(new DemoFunctionNode());
            _scheduler.Start();
            Console.WriteLine(!networkNode.Connect((IRawTransportChannel) channel)
                                  ? "Can' connect to configuration station node !"
                                  : "Connect to configuration station node successfully !");
            string commod;
            while ((commod = Console.ReadLine()) != "exit")
            {
                switch (commod)
                {
                    case "regist":
                        CSNRegistRequestMessage csnRegistRequestMessage = new CSNRegistRequestMessage();
                        csnRegistRequestMessage.Header.ServiceKey = Environment.MachineName + ":Service.Test:1.0.0.0";
                        csnRegistRequestMessage.Bind();
                        networkNode.Send(channel.Key, csnRegistRequestMessage.Body);
                        Console.WriteLine("#Already send regist request message to CSN......");
                        break;
                    case "gettable":
                        CSNGetDataTableRequestMessage csnGetKeyDataRequestMessage = new CSNGetDataTableRequestMessage();
                        csnGetKeyDataRequestMessage.Header.ServiceKey = Environment.MachineName + ":Service.Test:1.0.0.0";
                        csnGetKeyDataRequestMessage.DatabaseName = "MNAVDB";
                        csnGetKeyDataRequestMessage.TableName = "MNAV_AmsConfig";
                        csnGetKeyDataRequestMessage.Bind();
                        networkNode.Send(channel.Key, csnGetKeyDataRequestMessage.Body);
                        Console.WriteLine("#Already send get table data request message to CSN......");
                        break;
                    case "getservices":
                        break;
                }
            }
        }
    }
}
