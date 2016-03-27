using System;
using System.Collections.Generic;
using System.Threading;

using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Architecture.UnitTest.KAE;
using KJFramework.Containers;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Caches;
using KJFramework.Net.Enums;
using KJFramework.Net.HostChannels;
using KJFramework.Net.Transaction.ProtocolStack;
using NUnit.Framework;

namespace KJFramework.Net.Channels.UnitTest
{
    public class TcpChannelTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            SystemWorker.Initialize("KAEWorker", RemoteConfigurationSetting.Default, new EtcdRemoteConfigurationProxy(new System.Uri("")));
        }

        [Test]
        [Description("正常注册一个TCP管道测试")]
        public void RegistTest()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20001);
            Assert.IsTrue(hostChannel.Regist());
        }

        [Test]
        [Description("正常注销一个TCP管道测试")]
        public void UnRegistTest()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20002);
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [Test]
        [Description("正常连接一个TCP管道测试")]
        public void ConnectTest()
        {
            TcpTransportChannel connectedChannel = null;
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20003);
            Assert.IsTrue(hostChannel.Regist());
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                connectedChannel = (TcpTransportChannel) args.Target;
            };
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 20003);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            transportChannel.Disconnected += delegate { resetEvent.Set(); };
            Assert.IsTrue(hostChannel.UnRegist());
            transportChannel.Disconnect();
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }


        [Test]
        [Description("正常连接一个TCP管道，并断开测试")]
        public void DisconnectTest()
        {
            TcpTransportChannel connectedChannel = null;
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20004);
            Assert.IsTrue(hostChannel.Regist());
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                connectedChannel = (TcpTransportChannel)args.Target;
            };
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 20004);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            transportChannel.Disconnect();
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [Test]
        [Description("连接一个未知的TCP管道测试")]
        public void ConnectFailTest()
        {
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 55555);
            transportChannel.Connect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }

        [Test]
        [Description("发送消息测试")]
        public void SendMessageTest()
        {
            ITransportChannel connectedChannel = null;
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            AutoResetEvent msgEvent = new AutoResetEvent(false);
            AutoResetEvent channelEvent = new AutoResetEvent(false);
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(20005);
            Assert.IsTrue(hostChannel.Regist());
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                connectedChannel = args.Target;
                channelEvent.Set();
            };
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 20005);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            if (!channelEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed channel be connect test.");
            Assert.IsNotNull(connectedChannel);
            Assert.IsTrue(connectedChannel.IsConnected);
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            MetadataContainer msg = null;
            IMessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel) connectedChannel, new MetadataProtocolStack());
            msgChannel.ReceivedMessage += delegate(object sender, LightSingleArgEventArgs<List<MetadataContainer>> args)
            {
                msg = args.Target[0];
                msgEvent.Set();
            };
            MetadataContainer container = new MetadataContainer();
            container.SetAttribute(0x00, new StringValueStored("Test-Name"));
            container.SetAttribute(0x01, new StringValueStored("Test-Value"));
            byte[] data = MetadataObjectEngine.ToBytes(container);
            Assert.IsNotNull(data);
            int sendCount = transportChannel.Send(data);
            Assert.IsTrue(sendCount == data.Length || sendCount == 1);
            if (!msgEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed message channel receive test.");
            Assert.IsNotNull(msg);
            Console.WriteLine(msg);
            transportChannel.Disconnect();

            msgChannel.Disconnected += delegate { resetEvent.Set(); };
            if (!resetEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed transport channel disconnect test.");
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Thread.Sleep(2000);
            Assert.IsTrue(!connectedChannel.IsConnected);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        private void InnerHightSpeedSendTest(int destinationInstanceCount, int destinationMsgCount, int port)
        {
            int msgRecvCount = 0, sendMsgCount = 0;
            int previousMsgRecvCount = 0, previousSendMsgCount = 0;
            AutoResetEvent msgEvent = new AutoResetEvent(false);
            Console.Write("#Begining prepare Host channel resource...");
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(port);
            Assert.IsTrue(hostChannel.Regist());
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                MessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)args.Target, new MetadataProtocolStack());
                msgChannel.ReceivedMessage += delegate(object s, LightSingleArgEventArgs<List<MetadataContainer>> a)
                {
                    Interlocked.Add(ref msgRecvCount, a.Target.Count);
                };
            };
            Console.WriteLine("Done");
            Console.Write("#Begining prepare {0} numbers of TCP channel...", destinationInstanceCount);
            IList<TcpTransportChannel> clients = new List<TcpTransportChannel>();
            for (int i = 0; i < destinationInstanceCount; i++)
            {
                TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", port);
                Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
                transportChannel.Connect();
                Assert.IsTrue(transportChannel.IsConnected);
                Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
                clients.Add(transportChannel);
            }
            Console.WriteLine("Done");
            MetadataContainer container = new MetadataContainer();
            container.SetAttribute(0x00, new StringValueStored("Test-Name"));
            container.SetAttribute(0x01, new StringValueStored("Test-Value"));
            byte[] data = MetadataObjectEngine.ToBytes(container);
            Assert.IsNotNull(data);
            Console.WriteLine("#Sending message on those of transport channels...");
            ThreadPool.QueueUserWorkItem(delegate
            {
                while (true)
                {
                    previousMsgRecvCount = msgRecvCount;
                    previousSendMsgCount = sendMsgCount;
                    Thread.Sleep(1000);
                    if (destinationMsgCount != previousSendMsgCount || destinationMsgCount != previousMsgRecvCount)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Send Count: {0}    <---{1} messages/per-second.", sendMsgCount, sendMsgCount - previousSendMsgCount);
                        Console.WriteLine("Recv Count: {0}    <---{1} messages/per-second.", msgRecvCount, msgRecvCount - previousMsgRecvCount);
                        Console.WriteLine();
                    }
                }
            });
            ThreadPool.QueueUserWorkItem(delegate
            {
                while (sendMsgCount != destinationMsgCount)
                {
                    for (int i = 0; i < clients.Count; i++)
                    {
                        clients[i].Send(data);
                        Interlocked.Increment(ref sendMsgCount);
                        if (sendMsgCount == destinationMsgCount) break;
                    }
                    //Thread.Sleep(100);
                }
                msgEvent.Set();
            });
            msgEvent.WaitOne();
            Thread.Sleep(2000);
            Assert.IsTrue(sendMsgCount == destinationMsgCount);
            Assert.IsTrue(sendMsgCount == msgRecvCount);
            foreach (TcpTransportChannel clientChannel in clients) clientChannel.Disconnect();
            Thread.Sleep(2000);
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_5_Instance()
        {
            int capacity = ChannelConst.BuffAsyncStubPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(5, 1000000, 20006);
            FixedCacheContainer<SocketBuffStub> container = (FixedCacheContainer<SocketBuffStub>)ChannelConst.BuffAsyncStubPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_10_Instance()
        {
            int capacity = ChannelConst.BuffAsyncStubPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(10, 1000000, 20007);
            FixedCacheContainer<SocketBuffStub> container = (FixedCacheContainer<SocketBuffStub>)ChannelConst.BuffAsyncStubPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_254_Instance()
        {
            int capacity = ChannelConst.BuffAsyncStubPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(254, 1000000, 20008);
            FixedCacheContainer<SocketBuffStub> container = (FixedCacheContainer<SocketBuffStub>)ChannelConst.BuffAsyncStubPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_20000_Instance()
        {
            int capacity = ChannelConst.BuffAsyncStubPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(20000, 1000000, 20009);
            FixedCacheContainer<SocketBuffStub> container = (FixedCacheContainer<SocketBuffStub>)ChannelConst.BuffAsyncStubPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        #endregion
    }
}