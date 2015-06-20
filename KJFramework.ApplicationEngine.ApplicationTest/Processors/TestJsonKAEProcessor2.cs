using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.ApplicationTest.Processors
{
    [KAEProcessorProperties(ProtocolId = 1, ServiceId = 0, DetailsId = 3)]
    public class TestMetadataKAEProcessor2 : MetadataKAEProcessor
    {
        public TestMetadataKAEProcessor2(IApplication application)
            : base(application)
        {
        }

        protected override void InnerProcess(IMessageTransaction<MetadataContainer> package)
        {
            MetadataContainer rspMsg = new MetadataContainer();
            rspMsg.SetAttribute(0x0C, new StringValueStored("Hello, Client!"));
            package.SendResponse(rspMsg);
        }
    }
}