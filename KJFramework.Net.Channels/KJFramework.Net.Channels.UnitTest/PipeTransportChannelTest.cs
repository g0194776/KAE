using System;
using System.Text;
using System.Threading;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Uri;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class PipeTransportChannelTest
    {
        #region Members

        private byte[] _recvData;

        #endregion

        [TestMethod]
        public void ConnectTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest");
            Assert.IsTrue(hostChannel.Regist());
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest"));
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void ConnectFailTest()
        {
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTestFail"));
            transportChannel.Connect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }

        [TestMethod]
        public void DisconnectTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest");
            Assert.IsTrue(hostChannel.Regist());
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest"));
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            transportChannel.Disconnect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsNull(transportChannel.Stream);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void ConstructorTest()
        {
            ITransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest"));
            Assert.IsTrue(transportChannel.Key != Guid.Empty);
        }

        [TestMethod]
        public void SendTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest5");
            Assert.IsTrue(hostChannel.Regist());
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest5"));
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) > 0);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void SendFailTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest4");
            Assert.IsTrue(hostChannel.Regist());
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest4"));
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) > 0);
            transportChannel.Disconnect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) == -1);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void ReconnectTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest3");
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest3"));
            transportChannel.Connect();
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(transportChannel.Reconnect());
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void ReadTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest2");
            hostChannel.ChannelCreated += hostChannel_ChannelCreated;
            Assert.IsTrue(hostChannel.Regist());
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest2"));
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            transportChannel.ReceivedData += TransportChannelReceivedData;
            Assert.IsTrue(transportChannel.Send(new byte[] { 0x01 }) > 0);
            Thread.Sleep(500);
            Assert.IsNotNull(_recvData);
            Assert.IsTrue(Encoding.Default.GetString(_recvData) == "0123456789");
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void DisconnectTest1()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("PipeTest1");
            Assert.IsTrue(hostChannel.Regist());
            PipeTransportChannel transportChannel = new PipeTransportChannel(new PipeUri("pipe://./PipeTest1"));
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            Assert.IsTrue(hostChannel.UnRegist());
            Thread.Sleep(500);
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsNull(transportChannel.Stream);
        }

        void hostChannel_ChannelCreated(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<ITransportChannel> e)
        {
            Assert.IsTrue(e.Target.Send(Encoding.Default.GetBytes("0123456789")) > 0);
        }

        void TransportChannelReceivedData(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
            _recvData = e.Target;
        }
    }
}