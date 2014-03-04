using System;
using System.Threading;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using NUnit.Framework;

namespace KJFramework.Net.Transaction.UnitTest
{
    public class MetadataConnectionAgentTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
        }

        [Test]
        public void ReceiveTransactionTest()
        {
            MetadataTransactionManager manager1 = new MetadataTransactionManager(new TransactionIdentityComparer());
            MetadataTransactionManager manager2 = new MetadataTransactionManager(new TransactionIdentityComparer());
            ITransportChannel connectedChannel = null;
            AutoResetEvent msgEvent = new AutoResetEvent(false);
            AutoResetEvent channelEvent = new AutoResetEvent(false);
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20011);
            Assert.IsTrue(hostChannel.Regist());
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                connectedChannel = args.Target;
                channelEvent.Set();
            };
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 20011);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            if (!channelEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed channel be connect test.");
            Assert.IsNotNull(connectedChannel);
            Assert.IsTrue(connectedChannel.IsConnected);
            IServerConnectionAgent<MetadataContainer> conencteeAgent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)connectedChannel,
                                                             new MetadataProtocolStack()), manager1);
            conencteeAgent.NewTransaction += delegate(object sender, LightSingleArgEventArgs<IMessageTransaction<MetadataContainer>> args)
            {
                IMessageTransaction<MetadataContainer> newTransaction = args.Target;
                Assert.IsNotNull(newTransaction);
                Assert.IsNotNull(newTransaction.Request);
                //MessageIdentity
                Assert.IsNotNull(newTransaction.Request.IsAttibuteExsits(0x00));
                //TransactionIdentity
                Assert.IsNotNull(newTransaction.Request.IsAttibuteExsits(0x01));
                Assert.IsNotNull(newTransaction.Request.IsAttibuteExsits(0x0A));
                MetadataContainer rsp = new MetadataContainer();
                rsp.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 10, ServiceId = 0, DetailsId = 1 }));
                rsp.SetAttribute(0x0A, new ByteValueStored(0x00));
                newTransaction.SendResponse(rsp);
            };
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            IServerConnectionAgent<MetadataContainer> conenctorAgent = new MetadataConnectionAgent(new MessageTransportChannel<MetadataContainer>(transportChannel, new MetadataProtocolStack()), manager2);
            bool hasException = false;
            MessageTransaction<MetadataContainer> transaction = conenctorAgent.CreateTransaction();
            transaction.Failed += delegate
            {
                hasException = true;
                msgEvent.Set();
            };
            transaction.Timeout += delegate
            {
                hasException = true;
                msgEvent.Set();
            };
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<MetadataContainer> args)
            {
                MetadataContainer rspMsg = args.Target;
                Assert.IsTrue(rspMsg.IsAttibuteExsits(0x00));
                Assert.IsTrue(rspMsg.IsAttibuteExsits(0x01));
                Assert.IsTrue(rspMsg.IsAttibuteExsits(0x0A));
                hasException = rspMsg.GetAttributeAsType<byte>(0x0A) != 0;
                Console.WriteLine(rspMsg);
                msgEvent.Set();
            };
            ResourceBlock req = new MetadataContainer()
            .SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 10, ServiceId = 0, DetailsId = 0 }))
            .SetAttribute(0x0A, new StringValueStored("EEEEEEEEEEE..."));
            transaction.SendRequest((MetadataContainer) req);
            if (!msgEvent.WaitOne(1000))
                throw new System.Exception("#Cannot Completed currently transaction REQ & RSP Flow.");
            conencteeAgent.Close();
            conenctorAgent.Close();
            Assert.IsTrue(hostChannel.UnRegist());
        }

        #endregion
    }
}