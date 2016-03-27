using System;
using System.Net;
using KJFramework.EventArgs;
using KJFramework.IO.Helper;
using KJFramework.Net.Channels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Platform.Client.ProtocolStack;

namespace KJFramework.Platform.Client.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialie client protocol stack test program......");
            Console.WriteLine("Initialie network node......");
            NetworkNode<ClientMessage> networkNode = new NetworkNode<ClientMessage>(new ClientProtocolStack());
            Console.WriteLine("Initialie protocol stack......");
            networkNode.ProtocolStack.Initialize();
            networkNode.NewMessageReceived += NewMessageReceived;
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            ITransportChannel transportChannel = new TcpTransportChannel(iep);
            Console.WriteLine("Begin connect to dynamic service center......");
            if (!networkNode.Connect((IRawTransportChannel) transportChannel))
            {
                ConsoleHelper.PrintLine("Can not connect to target remote address. #address: " + iep.Address + "!",
                                        ConsoleColor.DarkRed);
                ConsoleHelper.PrintLine("PRESS <ANY> KEY TO EXIT.", ConsoleColor.DarkRed);
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Begin send set title message......");
            ClientSetTagRequestMessage setTitleRequestMessage = new ClientSetTagRequestMessage();
            setTitleRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
            setTitleRequestMessage.Bind();
            networkNode.Send(transportChannel.Key, setTitleRequestMessage.Body);
            Console.WriteLine("Client test program started!\r\nWaiting command......");
            string command;
            while ((command = Console.ReadLine()) != "exit")
            {
                string[] orders = command.Split(' ');
                switch (orders[0])
                {
                    case "GetServices":
                        ClientGetServicesRequestMessage getServicesRequestMessage = new ClientGetServicesRequestMessage();
                        getServicesRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        getServicesRequestMessage.MachineName = "*ALL*";
                        getServicesRequestMessage.ServiceName = "*ALL*";
                        getServicesRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, getServicesRequestMessage.Body);
                        break;
                    case "GetHealth":
                        ClientGetComponentHealthRequestMessage getComponentHealthRequestMessage = new ClientGetComponentHealthRequestMessage();
                        getComponentHealthRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        getComponentHealthRequestMessage.Components = new[] { "*ALL*" };
                        getComponentHealthRequestMessage.ServiceName = orders[1];
                        getComponentHealthRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, getComponentHealthRequestMessage.Body);
                        break;
                    case "GetFiles":
                        ClientGetFileInfomationRequestMessage getFileInfomationRequestMessage = new ClientGetFileInfomationRequestMessage();
                        getFileInfomationRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        getFileInfomationRequestMessage.Files = "*ALL*";
                        getFileInfomationRequestMessage.ServiceName = orders[1];
                        getFileInfomationRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, getFileInfomationRequestMessage.Body);
                        break;
                    case "Reset":
                        ClientResetHeartBeatTimeRequestMessage resetHeartBeatTimeRequestMessage = new ClientResetHeartBeatTimeRequestMessage();
                        resetHeartBeatTimeRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        resetHeartBeatTimeRequestMessage.Target = orders[1];
                        resetHeartBeatTimeRequestMessage.Interval = int.Parse(orders[2]);
                        resetHeartBeatTimeRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, resetHeartBeatTimeRequestMessage.Body);
                        break;
                    case "Update":
                        ClientUpdateComponentRequestMessage updateComponentRequestMessage = new ClientUpdateComponentRequestMessage();
                        updateComponentRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        updateComponentRequestMessage.ComponentName = "*ALL*";
                        updateComponentRequestMessage.ServiceName = orders[1];
                        updateComponentRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, updateComponentRequestMessage.Body);
                        break;
                    case "GetNodes":
                        ClientGetDeployNodesRequestMessage getDeployNodesRequestMessage = new ClientGetDeployNodesRequestMessage();
                        getDeployNodesRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        getDeployNodesRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, getDeployNodesRequestMessage.Body);
                        break;
                    case "GetCore":
                        ClientGetCoreServiceRequestMessage getCoreServiceRequestMessage = new ClientGetCoreServiceRequestMessage();
                        getCoreServiceRequestMessage.Header.ClientTag = "#TEST-CLIENT#";
                        getCoreServiceRequestMessage.Category = orders[1];
                        getCoreServiceRequestMessage.Bind();
                        networkNode.Send(transportChannel.Key, getCoreServiceRequestMessage.Body);
                        break;
                }
            }
        }

        static void NewMessageReceived(object sender, LightSingleArgEventArgs<ReceivedMessageObject<ClientMessage>> e)
        {
            ClientMessage message = e.Target.Message;
            if (message is ClientSetTagResponseMessage)
            {
                ClientSetTagResponseMessage setTagResponseMessage = (ClientSetTagResponseMessage)message;
                ConsoleHelper.PrintLine("Received a set title response message. #result: " + setTagResponseMessage.Result, ConsoleColor.DarkGray);
                return;
            }
            ConsoleHelper.PrintLine("Received a new response message. #type: " + message, ConsoleColor.DarkGray);
        }
    }
}
