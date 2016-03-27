using System.Diagnostics;
using KJFramework.Net.Channels.HostChannels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class TcpHostTransportChannelTest
    {
        [TestMethod]
        public void RegistTest()
        {
            TcpHostTransportChannel hostTransportChannel = new TcpHostTransportChannel(9999);
            Assert.IsTrue(hostTransportChannel.Regist());
        }

        [TestMethod]
        public void RegistForIllegalPortTest1()
        {
            System.Exception exception = null;
            try
            {
                TcpHostTransportChannel hostTransportChannel = new TcpHostTransportChannel(-1);
                Assert.IsTrue(hostTransportChannel.Regist());
            }
            catch (System.Exception ex)
            {
                exception = ex;
                Debug.Print(ex.Message);
            }
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void RegistForIllegalPortTest2()
        {
            System.Exception exception = null;
            try
            {
                TcpHostTransportChannel hostTransportChannel = new TcpHostTransportChannel(21321313);
                Assert.IsTrue(hostTransportChannel.Regist());
            }
            catch (System.Exception ex)
            {
                exception = ex;
                Debug.Print(ex.Message);
            }
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void UnRegistTest()
        {
            TcpHostTransportChannel hostTransportChannel = new TcpHostTransportChannel(9999);
            Assert.IsTrue(hostTransportChannel.Regist());
            Assert.IsTrue(hostTransportChannel.UnRegist());
        }
    }
}