using System;
using System.Threading;
using KJFramework.EventArgs;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.UnitTest.ProtocolStack;
using NUnit.Framework;

namespace KJFramework.Net.Transaction.UnitTest
{
    public class IntellectConnectionAgentTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
        }

        [Test]
        public void ReceiveTransactionTest()
        {
            MessageTransactionManager manager1 = new MessageTransactionManager(new TransactionIdentityComparer());
            MessageTransactionManager manager2 = new MessageTransactionManager(new TransactionIdentityComparer());
            ITransportChannel connectedChannel = null;
            AutoResetEvent msgEvent = new AutoResetEvent(false);
            AutoResetEvent channelEvent = new AutoResetEvent(false);
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20010);
            Assert.IsTrue(hostChannel.Regist());
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                connectedChannel = args.Target;
                channelEvent.Set();
            };
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 20010);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            if (!channelEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed channel be connect test.");
            Assert.IsNotNull(connectedChannel);
            Assert.IsTrue(connectedChannel.IsConnected);
            IServerConnectionAgent<BaseMessage> conencteeAgent =
                new IntellectObjectConnectionAgent(new MessageTransportChannel<BaseMessage>((IRawTransportChannel) connectedChannel,
                                                             new TestProtocolStack()), manager1);
            conencteeAgent.NewTransaction += delegate(object sender, LightSingleArgEventArgs<IMessageTransaction<BaseMessage>> args)
                {
                    IMessageTransaction<BaseMessage> newTransaction = args.Target;
                    Assert.IsNotNull(newTransaction);
                    Assert.IsNotNull(newTransaction.Request);
                    Assert.IsNotNull(newTransaction.Request.TransactionIdentity);
                    Assert.IsNotNull(newTransaction.Request.MessageIdentity);
                    newTransaction.SendResponse(new TestResponseMessage());
                };
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            IServerConnectionAgent<BaseMessage> conenctorAgent =
                new IntellectObjectConnectionAgent(new MessageTransportChannel<BaseMessage>(transportChannel,
                                                             new TestProtocolStack()), manager2);
            bool hasException = false;
            MessageTransaction<BaseMessage> transaction = conenctorAgent.CreateTransaction();
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
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<BaseMessage> args)
            {
                TestResponseMessage rspMsg = (TestResponseMessage) args.Target;
                hasException = rspMsg.ErrorId != 0;
                Console.WriteLine(rspMsg);
                msgEvent.Set();
            };
            transaction.SendRequest(new TestRequestMessage {Value1 = "EEEEEEEEE..."});
            if (!msgEvent.WaitOne(1000))throw new System.Exception("#Cannot Completed currently transaction REQ & RSP Flow.");
            conencteeAgent.Close();
            conenctorAgent.Close();
            Assert.IsTrue(hostChannel.UnRegist());
        }

        #endregion
    }
}