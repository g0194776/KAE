using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Net.Cloud.Objects;
using KJFramework.Net.ProtocolStacks;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Cloud.UnitTest
{
    #region Test protocol stack.

    public class TestProtocolStack : ProtocolStack<string>
    {
        #region Overrides of ProtocolStack<string>

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>
        /// 返回初始化的结果
        /// </returns>
        /// <exception cref="T:KJFramework.Net.Exception.InitializeFailedException">初始化失败</exception>
        public override bool Initialize()
        {
            return true;
        }

        /// <summary>
        /// 解析元数据
        /// </summary>
        /// <param name="data">元数据</param>
        /// <returns>
        /// 返回能否解析的一个标示
        /// </returns>
        public override List<string> Parse(byte[] data)
        {
            return new List<string> { Encoding.Default.GetString(data) };
        }

        /// <summary>
        /// 将一个消息转换为2进制形式
        /// </summary>
        /// <param name="message">需要转换的消息</param>
        /// <returns>
        /// 返回转换后的2进制
        /// </returns>
        public override byte[] ConvertToBytes(string message)
        {
            return Encoding.Default.GetBytes(message);
        }

        #endregion
    }

    #endregion

    #region Intellect object test protocol stack.

    public class IntellectObjectTestProtocolStack : ProtocolStack<TestObj>
    {
        #region Overrides of ProtocolStack<TestObj>

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>
        /// 返回初始化的结果
        /// </returns>
        /// <exception cref="T:KJFramework.Net.Exception.InitializeFailedException">初始化失败</exception>
        public override bool Initialize()
        {
            return true;
        }

        /// <summary>
        /// 解析元数据
        /// </summary>
        /// <param name="data">元数据</param>
        /// <returns>
        /// 返回能否解析的一个标示
        /// </returns>
        public override List<TestObj> Parse(byte[] data)
        {
            return new List<TestObj>{(TestObj) IntellectObjectEngine.GetObject<object>(typeof(TestObj), data)};
        }

        /// <summary>
        /// 将一个消息转换为2进制形式
        /// </summary>
        /// <param name="message">需要转换的消息</param>
        /// <returns>
        /// 返回转换后的2进制
        /// </returns>
        public override byte[] ConvertToBytes(TestObj message)
        {
            message.Bind();
            return message.Body;
        }

        #endregion
    }


    #endregion

    #region Test contract

    [ServiceContract(ServiceConcurrentType = ServiceConcurrentTypes.Concurrent)]
    public interface IServiceContract
    {
        [Operation]
        string TestCall(string content);
    }

    public class ServiceContract : IServiceContract
    {
        #region Implementation of IServiceContract

        public string TestCall(string content)
        {
            Console.WriteLine("S-S recv: " + content);
            return "Yes, i do.";
        }

        #endregion
    }

    #endregion

    /// <summary>
    ///This is a test class for NetworkNodeTest and is intended
    ///to contain all NetworkNodeTest Unit Tests
    ///</summary>
    [TestClass]
    public class NetworkNodeTest
    {
        private IRawTransportChannel _rawTransportChannel;
        private Guid _guid = Guid.Empty;
        private string _content;
        private TestObj _testObj;
        [TestMethod]
        public void ConnectTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            Assert.IsTrue(networkNode.Connect(new TcpTransportChannel("61.135.169.125", 80)));
        }

        [TestMethod]
        public void GetTransportChannelTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            var tcpTransportChannel = new TcpTransportChannel("61.135.169.125", 80);
            Assert.IsTrue(networkNode.Connect(tcpTransportChannel));
            ITransportChannel transportChannel = networkNode.GetTransportChannel(tcpTransportChannel.Key);
            Assert.IsNotNull(transportChannel);
            Assert.IsTrue(transportChannel.IsConnected);
        }

        [TestMethod]
        public void GetTransportChannelFailTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            var tcpTransportChannel = new TcpTransportChannel("61.135.169.125", 80);
            Assert.IsTrue(networkNode.Connect(tcpTransportChannel));
            ITransportChannel transportChannel = networkNode.GetTransportChannel(Guid.Empty);
            Assert.IsNull(transportChannel);
        }

        [TestMethod]
        public void DisconnectTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            var tcpTransportChannel = new TcpTransportChannel("61.135.169.125", 80);
            Assert.IsTrue(networkNode.Connect(tcpTransportChannel));
            ITransportChannel transportChannel = networkNode.GetTransportChannel(tcpTransportChannel.Key);
            Assert.IsNotNull(transportChannel);
            Assert.IsTrue(transportChannel.IsConnected);
            transportChannel.Disconnect();
            Assert.IsFalse(transportChannel.IsConnected);
        }

        [TestMethod]
        public void BroadcastTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            var tcpTransportChannel = new TcpTransportChannel("61.135.169.125", 80);
            Assert.IsTrue(networkNode.Connect(tcpTransportChannel));
            ITransportChannel transportChannel = networkNode.GetTransportChannel(tcpTransportChannel.Key);
            Assert.IsNotNull(transportChannel);
            Assert.IsTrue(transportChannel.IsConnected);
            networkNode.Broadcast("Hello~");
        }

        [TestMethod]
        public void CloseTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            var tcpTransportChannel = new TcpTransportChannel("61.135.169.125", 80);
            Assert.IsTrue(networkNode.Connect(tcpTransportChannel));
            ITransportChannel transportChannel = networkNode.GetTransportChannel(tcpTransportChannel.Key);
            Assert.IsNotNull(transportChannel);
            Assert.IsTrue(transportChannel.IsConnected);
            networkNode.Close();
            Assert.IsNull(networkNode.GetTransportChannel(tcpTransportChannel.Key));
        }

        [TestMethod]
        public void HostTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            networkNode.Regist(new TcpHostTransportChannel(11099));
            networkNode.Close();
        }

        [TestMethod]
        public void HostAndConnectTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            networkNode.Regist(new TcpHostTransportChannel(11099));
            networkNode.NewTransportChannelCreated += NewTransportChannelCreated1;
            Assert.IsTrue(networkNode.Connect(new TcpTransportChannel("127.0.0.1", 11099)));
            Assert.IsNotNull(_rawTransportChannel);
            networkNode.Close();
        }

        [TestMethod]
        public void HostAndDisconnectTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            networkNode.Regist(new TcpHostTransportChannel(11099));
            networkNode.NewTransportChannelCreated += NewTransportChannelCreated;
            networkNode.TransportChannelRemoved += TransportChannelRemoved;
            Assert.IsTrue(networkNode.Connect(new TcpTransportChannel("127.0.0.1", 11099)));
            Assert.IsNotNull(_rawTransportChannel);
            Thread.Sleep(5000);
            Assert.IsTrue(_guid != Guid.Empty);
            networkNode.Close();
        }

        [TestMethod]
        public void HostAndReadTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            networkNode.Regist(new TcpHostTransportChannel(11099));
            networkNode.NewTransportChannelCreated += NewTransportChannelCreated1;
            networkNode.TransportChannelRemoved += TransportChannelRemoved;
            var channel = new TcpTransportChannel("127.0.0.1", 11099);
            channel.ReceivedData += ReceivedData;
            Assert.IsTrue(networkNode.Connect(channel));
            Assert.IsNotNull(_rawTransportChannel);
            Thread.Sleep(2000);
            Assert.IsNotNull(_content);
            Assert.IsTrue(_content == "0123456789");
            networkNode.Close();
        }

        [TestMethod]
        public void SendTest()
        {
            NetworkNode<string> networkNode = new NetworkNode<string>(new TestProtocolStack());
            networkNode.Regist(new TcpHostTransportChannel(11099));
            networkNode.NewTransportChannelCreated += NewTransportChannelCreated2;
            networkNode.TransportChannelRemoved += TransportChannelRemoved;
            var channel = new TcpTransportChannel("127.0.0.1", 11099);
            channel.ReceivedData += ReceivedData;
            Assert.IsTrue(networkNode.Connect(channel));
            Assert.IsNotNull(_rawTransportChannel);
            networkNode.Send(channel.Key, "asfdasdfdsfdsf~");
            networkNode.Close();
        }

        [TestMethod]
        public void SendIntellectObjectTest()
        {
            NetworkNode<TestObj> networkNode = new NetworkNode<TestObj>(new IntellectObjectTestProtocolStack());
            networkNode.NewMessageReceived += NewMessageReceived;
            networkNode.Regist(new TcpHostTransportChannel(11099));
            networkNode.TransportChannelRemoved += TransportChannelRemoved;
            var channel = new TcpTransportChannel("127.0.0.1", 11099);
            Assert.IsTrue(networkNode.Connect(channel));
            networkNode.Send(channel.Key, new TestObj{Name = "Kevin Kline."});
            Thread.Sleep(500);
            Assert.IsNotNull(_testObj);
            networkNode.Close();
        }

        [TestMethod]
        public void HostConnectServiceTest()
        {
            NetworkNode<TestObj> networkNode = new NetworkNode<TestObj>(new IntellectObjectTestProtocolStack());
            networkNode.Regist(new TcpBinding("tcp://127.0.0.1:11007/TestHost"), typeof(ServiceContract));
            networkNode.Close();
        }


        [TestMethod]
        public void ConnectToServiceTest()
        {
            NetworkNode<TestObj> networkNode = new NetworkNode<TestObj>(new IntellectObjectTestProtocolStack());
            networkNode.Regist(new TcpBinding("tcp://127.0.0.1:11007/TestHost"), typeof(ServiceContract));
            IServiceContract contract = networkNode.Connect<IServiceContract>(new TcpBinding("tcp://127.0.0.1:11007/TestHost"));
            Assert.IsNotNull(contract);
            string content = contract.TestCall("What's your name?");
            Assert.IsNotNull(content);
            Console.WriteLine("Client side recv: " + content);
            networkNode.Close();
        }

        void NewMessageReceived(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<ReceivedMessageObject<TestObj>> e)
        {
            _testObj = e.Target.Message;
            ReceivedMessageObject<TestObj> receivedMessageObject = e.Target;
            Console.WriteLine("S-S Recv intellect obj, details below......");
            Console.WriteLine("Recv node id: " + receivedMessageObject.NodeId);
            Console.WriteLine("Recv channel key: " + receivedMessageObject.Channel.Key);
            Console.WriteLine("Recv message --> \"Name:\" " + receivedMessageObject.Message.Name);
        }

        void ReceivedData(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
            _content = Encoding.Default.GetString(e.Target);
        }

        void TransportChannelRemoved(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<Guid> e)
        {
            _guid = e.Target;
        }

        void NewTransportChannelCreated(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            _rawTransportChannel = e.Target;
            _rawTransportChannel.Send(Encoding.Default.GetBytes("0123456789"));
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 Thread.Sleep(500);
                                                 _rawTransportChannel.Disconnect();
                                             });
        }

        void NewTransportChannelCreated1(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            _rawTransportChannel = e.Target;
            _rawTransportChannel.ReceivedData += _rawTransportChannel_ReceivedData;
            _rawTransportChannel.Send(Encoding.Default.GetBytes("0123456789"));
        }
        void NewTransportChannelCreated2(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<IRawTransportChannel> e)
        {
            _rawTransportChannel = e.Target;
            _rawTransportChannel.ReceivedData += _rawTransportChannel_ReceivedData;
        }

        void _rawTransportChannel_ReceivedData(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
            Console.WriteLine("S-S Receive a message: " + Encoding.Default.GetString(e.Target));
        }
    }

    public class TestObj : IntellectObject
    {
        [IntellectProperty(0)]
        public string Name { get; set; }
    }
}
