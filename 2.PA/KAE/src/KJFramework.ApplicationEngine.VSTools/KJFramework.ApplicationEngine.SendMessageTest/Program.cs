﻿using System;
using System.Net;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;

namespace KJFramework.ApplicationEngine.SendMessageTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Loop Count: ");
            int count = int.Parse(Console.ReadLine());
            Console.Write("Remoting KAEHost Metadata Port: ");
            int port = int.Parse(Console.ReadLine());
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            MemoryAllotter.Instance.Initialize();
            ChannelConst.Initialize();

            MetadataTransactionManager transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
            ITransportChannel channel = new TcpTransportChannel(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            channel.Connect();
            MetadataConnectionAgent agent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)channel, new MetadataProtocolStack()), transactionManager);
            for (int i = 0; i < count; i++)
            {
                MessageTransaction<MetadataContainer> transaction = agent.CreateTransaction();
                transaction.Failed += delegate(object sender, System.EventArgs eventArgs)
                {
                    Console.WriteLine("FAIL");
                };
                transaction.Timeout += delegate(object sender, System.EventArgs eventArgs)
                {
                    Console.WriteLine("Timeout");
                };
                transaction.ResponseArrived +=
                    delegate(object sender, LightSingleArgEventArgs<MetadataContainer> eventArgs)
                    {
                        Console.WriteLine("Received RSP:");
                        Console.WriteLine(eventArgs.Target);
                    };
                transaction.KPPUniqueId = Guid.Parse("6567846e-71a8-40ae-9b2b-37882bcc6ba1");
                MetadataContainer reqMsg = new MetadataContainer();
                reqMsg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 1, ServiceId = 0, DetailsId = 2 }));
                reqMsg.SetAttribute(0x05, new ByteValueStored((byte)ApplicationLevel.Stable));
                reqMsg.SetAttribute(0x0A, new StringValueStored("Hello, KAE"));
                transaction.SendRequest(reqMsg);
            }
            Console.WriteLine("Wrote OK...");
            Console.ReadLine();
        }
    }
}
