using System.Net;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Messages.UnitTest
{
    [TestClass]
    public class IPEndPointIntellectTypeProcessorTest
    {
        #region Methods

        [TestMethod]
        [Description("IPEndPoint 一般性绑定测试")]
        public void BindTest()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("110.110.110.110"), 37111);
            IPEndPointIntellectTypeProcessor processor = new IPEndPointIntellectTypeProcessor();
            byte[] data = new byte[12];
            processor.Process(data, 0, IntellectTypeProcessorMapping.DefaultAttribute, iep);
            IntellectObjectEngineUnitTest.PrintBytes(data);
        }

        [TestMethod]
        [Description("IPEndPoint 一般性解析测试")]
        public void PickupTest()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("110.110.110.110"), 37111);
            IPEndPointIntellectTypeProcessor processor = new IPEndPointIntellectTypeProcessor();
            byte[] data = new byte[12];
            processor.Process(data, 0, IntellectTypeProcessorMapping.DefaultAttribute, iep);
            IntellectObjectEngineUnitTest.PrintBytes(data);
            //parse.
            IPEndPoint newObj = (IPEndPoint) processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, data);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Address.Equals(iep.Address));
            Assert.IsTrue(newObj.Port == iep.Port);
        }

        [TestMethod]
        [Description("IPEndPoint 一般性绑定测试")]
        public void BindTest1()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("110.110.110.110"), 37111);
            IPEndPointIntellectTypeProcessor processor = new IPEndPointIntellectTypeProcessor();
            byte[] data = processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, iep);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            IntellectObjectEngineUnitTest.PrintBytes(data);
        }

        [TestMethod]
        [Description("IPEndPoint 一般性解析测试")]
        public void PickupTest1()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("110.110.110.110"), 37111);
            IPEndPointIntellectTypeProcessor processor = new IPEndPointIntellectTypeProcessor();
            byte[] data = processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, iep);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            IntellectObjectEngineUnitTest.PrintBytes(data);
            //parse.
            IPEndPoint newObj = (IPEndPoint)processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, data);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Address.Equals(iep.Address));
            Assert.IsTrue(newObj.Port == iep.Port);
        }

        #endregion
    }
}