using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.ServiceModel.Identity;
using KJFramework.ServiceModel.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.ServiceModel.UnitTest
{
    [TestClass]
    public class MessageIdentityIntellectTypeProcessorTest
    {
        [TestMethod]
        public void BindTest()
        {
            MessageIdentity identity = new MessageIdentity {ProtocolId = 0, ServiceId = 1, DetailsId = 2};
            MessageIdentityIntellectTypeProcessor processor = new MessageIdentityIntellectTypeProcessor();
            byte[] data = processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, identity);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 3);
        }

        [TestMethod]
        public void PickupTest()
        {
            MessageIdentity identity = new MessageIdentity {ProtocolId = 0, ServiceId = 1, DetailsId = 2};
            MessageIdentityIntellectTypeProcessor processor = new MessageIdentityIntellectTypeProcessor();
            byte[] data = processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, identity);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 3);

            MessageIdentity newObj = (MessageIdentity) processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, data, 0, 3);
            Assert.IsTrue(newObj.ProtocolId == 0);
            Assert.IsTrue(newObj.ServiceId == 1);
            Assert.IsTrue(newObj.DetailsId == 2);
        }
    }
}