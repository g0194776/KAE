using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;

namespace KJFramework.ApplicationEngine.ApplicationTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            MemoryAllotter.Instance.Initialize();
            KAEHost host = new KAEHost(Path.GetFullPath(".")); 
            host.Start();
            FieldInfo field = host.GetType().GetField("_hostChannels", BindingFlags.NonPublic | BindingFlags.Instance);
            List<IHostTransportChannel> channels = (List<IHostTransportChannel>)field.GetValue(host);


            MetadataTransactionManager transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
            ITransportChannel channel = new TcpTransportChannel(new IPEndPoint(IPAddress.Parse("127.0.0.1"), ((TcpHostTransportChannel)channels[1]).Port));
            channel.Connect();
            MetadataConnectionAgent agent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)channel, new MetadataProtocolStack()), transactionManager);
            MessageTransaction<MetadataContainer> transaction = agent.CreateTransaction();
            transaction.Failed += delegate(object sender, System.EventArgs eventArgs)
            {
                
            };
            transaction.Timeout += delegate(object sender, System.EventArgs eventArgs)
            {
                
            };
            transaction.ResponseArrived+=
                delegate(object sender, LightSingleArgEventArgs<MetadataContainer> eventArgs)
                {
                    
                };
            MetadataContainer reqMsg = new MetadataContainer();
            reqMsg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 1, ServiceId = 0, DetailsId = 2 }));
            reqMsg.SetAttribute(0x0A, new StringValueStored("Hello, KAE"));
            transaction.SendRequest(reqMsg);
            Console.WriteLine("Wrote OK...");
            Console.ReadLine();
            host.Stop();
        }
    }
}
