using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.ApplicationTest.Processors
{
    [KAEProcessorProperties(ProtocolId = 1, ServiceId = 0, DetailsId = 2)]
    public class TestMetadataKAEProcessor : MetadataKAEProcessor
    {
        public TestMetadataKAEProcessor(IApplication application)
            : base(application)
        {
        }

        protected override void InnerProcess(IMessageTransaction<MetadataContainer> package)
        {
            MetadataContainer rspMsg = new MetadataContainer();
            package.SendResponse(rspMsg);
        }
    }
}