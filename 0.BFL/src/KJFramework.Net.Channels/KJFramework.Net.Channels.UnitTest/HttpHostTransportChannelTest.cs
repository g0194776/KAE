using System.Net;
using System.Text;
using System.Threading;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class HttpHostTransportChannelTest
    {
        private IHttpTransportChannel _transportChannel;
        [TestMethod]
        public void RegistTest()
        {
            HttpHostTransportChannel hostChannel =new HttpHostTransportChannel();
            hostChannel.Prefixes.Add("http://+:8080/");
            Assert.IsTrue(hostChannel.Regist());
        }

        [TestMethod]
        public void UnRegistTest()
        {
            HttpHostTransportChannel hostChannel = new HttpHostTransportChannel();
            hostChannel.Prefixes.Add("http://+:8081/");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.UnRegist());
        }

        [TestMethod]
        public void CreateChannelTest()
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
            _transportChannel = (IHttpTransportChannel) e.Target;
            string html = @"<html><h1>That's a test html.</h1></html>";
            //disposable channel.
            _transportChannel.Send(Encoding.Default.GetBytes(html));
        }
    }
}