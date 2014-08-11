using System.Threading;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Factories;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class LocalCSNTest
    {
        #region Methods.

        [Test]
        public void UpdatingCallbackTest()
        {
            IDataBroadcaster<string, string> broadcaster = DataBroadcasterFactory.Instance.Create<string, string>("*", new NetworkResource("127.0.0.1:11111"), true);
            Assert.IsTrue(broadcaster.Broadcast("test1", "test2"));
            Thread.Sleep(5000);
        }

        #endregion
    }
}