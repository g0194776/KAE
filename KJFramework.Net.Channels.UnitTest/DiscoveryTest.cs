using System;
using System.Threading;
using KJFramework.Net.Channels.Disconvery;
using KJFramework.Net.Channels.Disconvery.Protocols;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace KJFramework.Net.Channels.UnitTest
{
    public class DiscoveryTest
    {
        #region Methods

        [Test]
        public void InputPinInitializeTest()
        {
            DiscoveryInputPin inputPin = new DiscoveryInputPin(55505);
            inputPin.Start();
            Assert.IsTrue(inputPin.Enable);
        }

        [Test]
        public void BoradcastTest()
        {
            CommonBoradcastProtocol recvObj = null;
            AutoResetEvent autoReset = new AutoResetEvent(false);
            DiscoveryInputPin inputPin = new DiscoveryInputPin(55505);
            inputPin.AddNotificationEvent("TEST", delegate(CommonBoradcastProtocol obj)
            {
                recvObj = obj;
                autoReset.Set();
            });
            inputPin.Start();

            DiscoveryOnputPin onputPin = new DiscoveryOnputPin(55505);
            onputPin.Send(new CommonBoradcastProtocol {Key = "TEST", Environment = "PROC", Value = "~~"});
            if (!autoReset.WaitOne(5000)) throw new System.Exception("timeout");
            Assert.IsNotNull(recvObj);
            Console.WriteLine(JsonConvert.SerializeObject(recvObj));
        }

        [Test]
        public void ReuseAddressTest()
        {
            InputPinInitializeTest();
            //twice.
            InputPinInitializeTest();
        }

        #endregion

    }
}
