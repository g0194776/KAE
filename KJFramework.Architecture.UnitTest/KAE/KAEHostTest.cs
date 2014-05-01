using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class KAEHostTest
    {
        #region Methods.

        [Test]
        public void StartTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path);
            try
            {
                host.Start();
                FieldInfo field = host.GetType().GetField("_hostChannels", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(field);
                List<IHostTransportChannel> channels = (List<IHostTransportChannel>) field.GetValue(host);
                Assert.IsNotNull(channels);
                Assert.IsTrue(channels.Count > 0);
                Assert.IsTrue(host.Status == KAEHostStatus.Started);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                host.Stop();
                Assert.IsTrue(host.Status == KAEHostStatus.Stopped);
            }
        }

        [Test]
        public void StartWithCustomizeNetworkTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path);
            try
            {
                host.Start();
                FieldInfo field = host.GetType().GetField("_hostChannels", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(field);
                List<IHostTransportChannel> channels = (List<IHostTransportChannel>)field.GetValue(host);
                Assert.IsNotNull(channels);
                Assert.IsTrue(channels.Count > 1);
                Assert.IsTrue(host.Status == KAEHostStatus.Started);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                host.Stop();
                Assert.IsTrue(host.Status == KAEHostStatus.Stopped);
            }
        }

        [Test]
        public void ElementaryCommunicationTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path);
            try
            {
                host.Start();
                FieldInfo field = host.GetType().GetField("_hostChannels", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(field);
                List<IHostTransportChannel> channels = (List<IHostTransportChannel>)field.GetValue(host);
                Assert.IsNotNull(channels);
                Assert.IsTrue(channels.Count > 1);
                Assert.IsTrue(host.Status == KAEHostStatus.Started);

                MetadataTransactionManager transactionManager = new MetadataTransactionManager(new TransactionIdentityComparer());
                ITransportChannel channel = new TcpTransportChannel(new IPEndPoint(IPAddress.Parse("127.0.0.1"), ((TcpHostTransportChannel)channels[0]).Port));
                channel.Connect();
                MetadataConnectionAgent agent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)channel, new MetadataProtocolStack()), transactionManager);
                MessageTransaction<MetadataContainer> transaction = agent.CreateTransaction();
                MetadataContainer reqMsg = new MetadataContainer();
                reqMsg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity{ProtocolId = 1,ServiceId = 2,DetailsId = 0}));
                reqMsg.SetAttribute(0x0A, new StringValueStored("Hello, KAE"));
                transaction.SendRequest(reqMsg);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                host.Stop();
                Assert.IsTrue(host.Status == KAEHostStatus.Stopped);
            }
        }

        
        #endregion

    }
}