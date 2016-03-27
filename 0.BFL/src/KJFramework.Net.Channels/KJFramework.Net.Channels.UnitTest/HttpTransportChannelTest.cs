using System;
using System.Net;
using System.Text;
using System.Threading;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class HttpTransportChannelTest
    {
        private IHttpTransportChannel _transportChannel;
        [TestMethod]
        public void ServerSideConstructorTest()
        {
            HttpHostTransportChannel hostChannel = new HttpHostTransportChannel();
            hostChannel.Prefixes.Add("http://+:8081/");
            hostChannel.ChannelCreated += ChannelCreated;
            Assert.IsTrue(hostChannel.Regist());
            WebRequest webRequest = WebRequest.Create("http://127.0.0.1:8081/");
            WebResponse webResponse = webRequest.GetResponse();
            Thread.Sleep(1000);
            Assert.IsNotNull(_transportChannel);
            Assert.IsTrue(_transportChannel.ChannelType == HttpChannelTypes.Accepted);
            Assert.IsNotNull(_transportChannel.GetRequest());
            Assert.IsNotNull(_transportChannel.GetResponse());
            Assert.IsTrue(_transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsFalse(_transportChannel.IsConnected);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        void ChannelCreated(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<ITransportChannel> e)
        {
            Assert.IsTrue(e.Target.IsConnected);
            Assert.IsTrue(e.Target.CommunicationState == CommunicationStates.Opened);
            _transportChannel = (IHttpTransportChannel)e.Target;
            string html = @"<html><h1>That's a test html.</h1></html>";
            //disposable channel.
            _transportChannel.Send(Encoding.Default.GetBytes(html));
        }

        [TestMethod]
        public void ClientSideConstructorTest()
        {
            HttpHostTransportChannel hostChannel = new HttpHostTransportChannel();
            hostChannel.Prefixes.Add("http://+:8081/");
            hostChannel.ChannelCreated += ChannelCreated;
            Assert.IsTrue(hostChannel.Regist());
            IHttpTransportChannel clientChannel = new HttpTransportChannel("http://127.0.0.1:8081/", 5000);
            Assert.IsTrue(clientChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(clientChannel.ChannelType == HttpChannelTypes.Connected);
            Assert.IsFalse(clientChannel.IsConnected);
            clientChannel.Connect();
            clientChannel.ReceivedData += ReceivedData;
            Assert.IsTrue(clientChannel.IsConnected);
            Assert.IsTrue(clientChannel.CommunicationState == CommunicationStates.Opened);
            clientChannel.Send();
            Thread.Sleep(1000);
            Assert.IsTrue(clientChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsFalse(clientChannel.IsConnected);
            Assert.IsNotNull(_transportChannel);
            Assert.IsTrue(_transportChannel.ChannelType == HttpChannelTypes.Accepted);
            Assert.IsNotNull(_transportChannel.GetRequest());
            Assert.IsNotNull(_transportChannel.GetResponse());
            Assert.IsTrue(_transportChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsFalse(_transportChannel.IsConnected);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void TimeoutTest()
        {
            HttpHostTransportChannel hostChannel = new HttpHostTransportChannel();
            hostChannel.Prefixes.Add("http://+:8081/");
            hostChannel.ChannelCreated += ChannelCreated;
            Assert.IsTrue(hostChannel.Regist());
            IHttpTransportChannel clientChannel = new HttpTransportChannel("http://127.0.0.1:8081/", 1);
            Assert.IsTrue(clientChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsTrue(clientChannel.ChannelType == HttpChannelTypes.Connected);
            Assert.IsFalse(clientChannel.IsConnected);
            clientChannel.Connect();
            clientChannel.ReceivedData += ReceivedData;
            Assert.IsTrue(clientChannel.IsConnected);
            Assert.IsTrue(clientChannel.CommunicationState == CommunicationStates.Opened);
            clientChannel.Send();
            Thread.Sleep(500);
            Assert.IsTrue(clientChannel.CommunicationState == CommunicationStates.Closed);
            Assert.IsFalse(clientChannel.IsConnected);
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void FinishTest()
        {
            IHttpTransportChannel transportChannel = new HttpTransportChannel("http://www.baidu.com/", 5000);
            transportChannel.ReceivedData += ReceivedData;
            transportChannel.Connect();
            Assert.IsTrue(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelType == HttpChannelTypes.Connected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Opened);
            transportChannel.Send();
            Thread.Sleep(5000);
            Assert.IsFalse(transportChannel.IsConnected);
            Assert.IsTrue(transportChannel.ChannelType == HttpChannelTypes.Connected);
            Assert.IsTrue(transportChannel.CommunicationState == CommunicationStates.Closed);
        }

        void ReceivedData(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
            byte[] data = e.Target;
            Console.WriteLine("Recv data is: " + Encoding.Default.GetString(data));
        }
    }
}