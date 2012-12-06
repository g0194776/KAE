using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using KJFramework.Net.Channels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.Test.Client
{
    class Program
    {
        private static IRequestScheduler<DSNMessage> _scheduler;
        static void Main(string[] args)
        {
            _scheduler = new RequestScheduler<DSNMessage>(100);
            ITransportChannel channel = new TcpTransportChannel(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11007));
            NetworkNode<DSNMessage> networkNode = new NetworkNode<DSNMessage>(new DSNProtocolStack());
            networkNode.ProtocolStack.Initialize();
            _scheduler.Regist(networkNode);
            _scheduler.Regist(new DemoFunctionNode());
            _scheduler.Start();
            Console.WriteLine(!networkNode.Connect((IRawTransportChannel) channel)
                                  ? "Can' connect to deploy service node !"
                                  : "Connect to deploy service node successfully !");
            string commod;
            while ((commod = Console.ReadLine()) != "exit")
            {
                switch (commod)
                {
                    case "deploy":
                        {
                            const int packageSizae = 4086;
                            //begin transfer file
                            DSNBeginTransferFileRequestMessage requestMessage = new DSNBeginTransferFileRequestMessage();
                            requestMessage.ServiceName = "KJFramework.Platform.Deploy.DSC";
                            requestMessage.Name = "服务中心";
                            requestMessage.Version = "0.0.0.1";
                            requestMessage.Description = "服务中心, 用于管理所有被控主机";
                            string requestToken = md5_hash("D:\\DSC.rar");
                            requestMessage.RequestToken = requestToken;
                            FileStream stream = new FileStream("D:\\DSC.rar", FileMode.Open);
                            int total = (int)(stream.Length / packageSizae) + (stream.Length > packageSizae ? 1 : 0);
                            requestMessage.TotalPacketCount = total;
                            requestMessage.Bind();
                            networkNode.Send(channel.Key, requestMessage.Body);
                            stream.Position = 0;
                            //Send datas.
                            int offset = 0;
                            int last = 0;
                            for (int i = 0; i < total; i++)
                            {
                                byte[] data;
                                last = (int)(stream.Length - offset);
                                if (last >= packageSizae)
                                {
                                    data = new byte[packageSizae];
                                    stream.Read(data, 0, packageSizae);
                                    offset += packageSizae;
                                }
                                else
                                {
                                    data = new byte[last];
                                    stream.Read(data, 0, last);
                                    offset += last;
                                }
                                //Send it.
                                DSNTransferDataMessage transferDataMessage = new DSNTransferDataMessage();
                                transferDataMessage.RequestToken = requestToken;
                                transferDataMessage.CurrentPackageId = i;
                                transferDataMessage.Data = data;
                                transferDataMessage.Bind();
                                networkNode.Send(channel.Key, transferDataMessage.Body);
                            }
                            //End of it.
                            DSNEndTransferFileRequestMessage endTransferFileRequestMessage = new DSNEndTransferFileRequestMessage();
                            endTransferFileRequestMessage.RequestToken = requestToken;
                            endTransferFileRequestMessage.Bind();
                            networkNode.Send(channel.Key, endTransferFileRequestMessage.Body);
                            Console.WriteLine("Send file data finished.");
                        }
                        break;
                    case "undeploy":
                        {
                            DSNUnDeployServiceRequestMessage dsnUnDeployServiceRequestMessage = new DSNUnDeployServiceRequestMessage();
                            dsnUnDeployServiceRequestMessage.IsDetailReport = true;
                            dsnUnDeployServiceRequestMessage.ServiceName = "KJFramework.Platform.Deploy.DSC";
                            dsnUnDeployServiceRequestMessage.Reason = "没事儿闲的";
                            dsnUnDeployServiceRequestMessage.Bind();
                            networkNode.Send(channel.Key, dsnUnDeployServiceRequestMessage.Body);
                        }
                        break;
                    case "getservices":
                        {
                            DSNGetLocalServiceInfomationRequestMessage getLocalServiceInfomationRequestMessage = new DSNGetLocalServiceInfomationRequestMessage();
                            getLocalServiceInfomationRequestMessage.Bind();
                            networkNode.Send(channel.Key, getLocalServiceInfomationRequestMessage.Body);
                        }
                        break;
                }
            }
        }


        public static string md5_hash(string path)
        {
            FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = get_md5.ComputeHash(get_file);
            string resule = System.BitConverter.ToString(hash_byte);
            resule = resule.Replace("-", "");
            get_file.Close();
            return resule;
        } 

    }
}
