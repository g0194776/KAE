﻿using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using KJFramework.Cache.Containers;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels.Caches;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction.ProtocolStack;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace KJFramework.Net.Channels.UnitTest
{
    public class PipeChannelTest
    {
        #region Methods.

        [Test]
        [Description("正常注册一个命名管道测试")]
        public void RegistTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("pipe://./test1");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.TotalCount == 254);
        }

        [Test]
        [Description("正常注销一个命名管道测试")]
        public void UnRegistTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("pipe://./test2");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.TotalCount == 254);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [Test]
        [Description("正常连接一个命名管道测试")]
        public void ConnectTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("pipe://./test5");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.TotalCount == 254);
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./test5"));
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.AvailableCount == 253);
            Assert.IsTrue(hostChannel.UsedCount == 1);
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            transportChannel.Disconnected += delegate { resetEvent.Set(); };
            Assert.IsTrue(hostChannel.UnRegist());
            if (!resetEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed transport channel disconnect test.");
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }


        [Test]
        [Description("正常连接一个命名管道，并断开测试")]
        public void DisconnectTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("pipe://./test3");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.TotalCount == 254);
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./test3"));
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.AvailableCount == 253);
            Assert.IsTrue(hostChannel.UsedCount == 1);
            transportChannel.Disconnect();
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Thread.Sleep(2000);
            Assert.IsTrue(hostChannel.AvailableCount == 254);
            Assert.IsTrue(hostChannel.UsedCount == 0);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [Test]
        [Description("连接一个未知的命名管道测试")]
        public void ConnectFailTest()
        {
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./test4"));
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
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("pipe://./test6");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.TotalCount == 254);
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                connectedChannel = args.Target;
                channelEvent.Set();
            };
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./test6"));
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
            transportChannel.Connect();
            if (!channelEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed channel be connect test.");
            Assert.IsNotNull(connectedChannel);
            Assert.IsTrue(connectedChannel.IsConnected);


            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.AvailableCount == 253);
            Assert.IsTrue(hostChannel.UsedCount == 1);
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
            Assert.IsTrue(sendCount == data.Length);
            if (!msgEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed message channel receive test.");
            Assert.IsNotNull(msg);
            Console.WriteLine(msg);
            transportChannel.Disconnect();

            msgChannel.Disconnected += delegate { resetEvent.Set(); };
            if (!resetEvent.WaitOne(2000)) throw new System.Exception("#Cannot passed transport channel disconnect test.");
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Thread.Sleep(2000);
            Assert.IsTrue(hostChannel.AvailableCount == 254);
            Assert.IsTrue(hostChannel.UsedCount == 0);
            Assert.IsTrue(!connectedChannel.IsConnected);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        private void InnerHightSpeedSendTest(int destinationInstanceCount, int destinationMsgCount,string instanceName)
        {
            int msgRecvCount = 0, sendMsgCount = 0;
            int previousMsgRecvCount = 0, previousSendMsgCount = 0;
            AutoResetEvent msgEvent = new AutoResetEvent(false);
            Console.Write("#Begining prepare Host channel resource...");
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("pipe://./" + instanceName);
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.TotalCount == 254);
            hostChannel.ChannelCreated += delegate(object sender, LightSingleArgEventArgs<ITransportChannel> args)
            {
                MessageTransportChannel<MetadataContainer> msgChannel = new MessageTransportChannel<MetadataContainer>((IRawTransportChannel)args.Target, new MetadataProtocolStack());
                msgChannel.ReceivedMessage += delegate(object s, LightSingleArgEventArgs<List<MetadataContainer>> a)
                {
                    Interlocked.Add(ref msgRecvCount, a.Target.Count);
                };
            };
            Console.WriteLine("Done");
            Console.Write("#Begining prepare {0} instances of Named Pipe channel...", destinationInstanceCount);
            IList<PipeTransportChannel> clients = new List<PipeTransportChannel>();
            for (int i = 0; i < destinationInstanceCount; i++)
            {
                PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./" + instanceName));
                Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Unknown);
                transportChannel.Connect();
                Assert.IsTrue(transportChannel.IsConnected);
                Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
                clients.Add(transportChannel);
            }
            Thread.Sleep(10000);
            Assert.IsTrue(hostChannel.AvailableCount == 254 - destinationInstanceCount);
            Assert.IsTrue(hostChannel.UsedCount == destinationInstanceCount);
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
            foreach (PipeTransportChannel clientChannel in clients) clientChannel.Disconnect();
            Thread.Sleep(2000);
            Assert.IsTrue(hostChannel.AvailableCount == 254);
            Assert.IsTrue(hostChannel.UsedCount == 0);
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_5_Instance()
        {
            int capacity = ChannelConst.NamedPipeBuffPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(5, 1000000, "HightSpeedInstance5");
            FixedCacheContainer<BuffStub>  container = (FixedCacheContainer<BuffStub>) ChannelConst.NamedPipeBuffPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_10_Instance()
        {
            int capacity = ChannelConst.NamedPipeBuffPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(10, 1000000, "HightSpeedInstance10");
            FixedCacheContainer<BuffStub> container = (FixedCacheContainer<BuffStub>)ChannelConst.NamedPipeBuffPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        [Test]
        [Description("快速发送消息并且校验BUFF缓存个数是否跟初始化的一致测试")]
        public void HighSpeedSendAndBuffCountTest_254_Instance()
        {
            int capacity = ChannelConst.NamedPipeBuffPool.Capacity;
            Console.WriteLine("Initialized capacity: " + capacity);
            InnerHightSpeedSendTest(254, 1000000, "HightSpeedInstance254");
            FixedCacheContainer<BuffStub> container = (FixedCacheContainer<BuffStub>)ChannelConst.NamedPipeBuffPool;
            Assert.IsNotNull(container);
            Thread.Sleep(2000);
            Console.WriteLine("Exactlly internal cache count: " + container.GetCount());
            Assert.IsTrue(capacity == container.GetCount());
        }

        #endregion
    }
}