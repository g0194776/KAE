using System;
using System.Text;
using System.Threading;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class TcpTransportChannelTest
    {
        #region Members

        private byte[] _recvData;

        #endregion

        [TestMethod]
        public void ConnectTest()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(9990);
            Assert.IsTrue(hostChannel.Regist());
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 9990);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void ConnectFailTest()
        {
            ITransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 10101);
            transportChannel.Connect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }

        [TestMethod]
        public void DisconnectTest()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(9990);
            Assert.IsTrue(hostChannel.Regist());
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 9990);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            transportChannel.Disconnect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void DisconnectTest1()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(9990);
            hostChannel.ChannelCreated+= ChannelCreatedForDisconnect;
            Assert.IsTrue(hostChannel.Regist());
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 9990);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.UnRegist());
            Thread.Sleep(3000);
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            ITransportChannel transportChannel = new TcpTransportChannel("61.135.169.125", 10101);
            Assert.IsTrue(transportChannel.Key != Guid.Empty);
        }

        [TestMethod]
        public void SendTest()
        {
            TcpTransportChannel transportChannel = new TcpTransportChannel("61.135.169.125", 80);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) > 0);
        }

        [TestMethod]
        public void SendFailTest()
        {
            TcpTransportChannel transportChannel = new TcpTransportChannel("61.135.169.125", 80);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) > 0);
            transportChannel.Disconnect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) == -1);
        }

        [TestMethod]
        public void ReconnectTest()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(9990);
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 9990);
            transportChannel.Connect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(transportChannel.Reconnect());
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void ReadTest()
        {
            TcpHostTransportChannel hostChannel = new TcpHostTransportChannel(9990);
            hostChannel.ChannelCreated += hostChannel_ChannelCreated;
            Assert.IsTrue(hostChannel.Regist());
            TcpTransportChannel transportChannel = new TcpTransportChannel("127.0.0.1", 9990);
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelKey != 0);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            transportChannel.ReceivedData += TransportChannelReceivedData;
            transportChannel.Open();
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) > 0);
            Thread.Sleep(1000);
            Assert.IsNotNull(_recvData);
            Assert.IsTrue(Encoding.Default.GetString(_recvData) == "0123456789");
            Assert.IsTrue(hostChannel.UnRegist());
        }

        void ChannelCreatedForDisconnect(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<ITransportChannel> e)
        {
            Assert.IsTrue(e.Target.Send(Encoding.Default.GetBytes("0123456789")) > 0);
            e.Target.Disconnect();
        }

        void hostChannel_ChannelCreated(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<ITransportChannel> e)
        {
            e.Target.Open();
            Assert.IsTrue(e.Target.Send(Encoding.Default.GetBytes("0123456789")) > 0);
        }

        void TransportChannelReceivedData(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
            _recvData = e.Target;
        }
    }
}