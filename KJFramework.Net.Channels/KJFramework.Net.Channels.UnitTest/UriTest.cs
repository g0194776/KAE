using KJFramework.Net.Channels.Uri;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Channels.UnitTest
{
    [TestClass]
    public class UriTest
    {
        [TestMethod]
        public void TcpUriTest()
        {
            TcpUri tcpUri = new TcpUri("tcp://localhost:8889/Test");
            Assert.IsTrue(tcpUri.GetServiceUri() == ":8889/Test");
        }

        [TestMethod]
        public void PipeUriTest()
        {
            PipeUri pipeUri = new PipeUri("pipe://./Test");
            Assert.IsTrue(pipeUri.GetServiceUri() == "/Test");
        }
    }
}