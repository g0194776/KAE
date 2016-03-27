using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Uri;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class PipeHostTransportChannelTest
    {
        [TestMethod]
        public void RegistTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("Test");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.InstanceCount == 10);
            hostChannel.UnRegist();
        }

        [TestMethod]
        public void RegistTest1()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("Test1", 100);
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.InstanceCount == 100);
            hostChannel.UnRegist();
        }

        [TestMethod]
        public void RegistTest2()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel(new PipeUri("pipe://./Test2"), 100);
            Assert.IsTrue(hostChannel.Name == "Test2");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.InstanceCount == 100);
            hostChannel.UnRegist();
        }

        [TestMethod]
        public void UnRegistTest()
        {
            PipeHostTransportChannel hostChannel = new PipeHostTransportChannel("Test");
            Assert.IsTrue(hostChannel.Regist());
            Assert.IsTrue(hostChannel.InstanceCount == 10);
            Assert.IsTrue(hostChannel.UnRegist());
        }
    }
}