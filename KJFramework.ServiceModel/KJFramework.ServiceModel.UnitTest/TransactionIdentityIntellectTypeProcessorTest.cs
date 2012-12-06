using System.Net;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.ServiceModel.Identity;
using KJFramework.ServiceModel.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.ServiceModel.UnitTest
{
    [TestClass]
    public class TransactionIdentityIntellectTypeProcessorTest
    {
        [TestMethod]
        public void BindTest()
        {
            TransactionIdentity identity = new TransactionIdentity
                                               {
                                                   Iep = new IPEndPoint(IPAddress.Parse("192.168.110.111"), 55555),
                                                   MessageId = 98765,
                                                   IsOneway = true,
                                                   IsRequest = true
                                               };
            TransactionIdentityIntellectTypeProcessor processor = new TransactionIdentityIntellectTypeProcessor();
            byte[] data = processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, identity);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 18);
        }

        [TestMethod]
        public void PickupTest()
        {
            TransactionIdentity identity = new TransactionIdentity
                                               {
                                                   Iep = new IPEndPoint(IPAddress.Parse("192.168.110.111"), 55555),
                                                   MessageId = 98765,
                                                   IsOneway = true,
                                                   IsRequest = true
                                               };
            TransactionIdentityIntellectTypeProcessor processor = new TransactionIdentityIntellectTypeProcessor();
            byte[] data = processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, identity);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 18);

            TransactionIdentity newObj =
                (TransactionIdentity) processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, data, 0, 18);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Iep);
            Assert.IsTrue(newObj.Iep.Equals(identity.Iep));
            Assert.IsTrue(newObj.MessageId == identity.MessageId);
            Assert.IsTrue(newObj.IsOneway == identity.IsOneway);
            Assert.IsTrue(newObj.IsRequest == identity.IsRequest);
        }
    }
}