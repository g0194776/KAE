using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
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
        }
    }
}