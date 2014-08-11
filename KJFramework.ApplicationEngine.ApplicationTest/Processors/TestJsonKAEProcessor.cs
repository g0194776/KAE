using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.ValueStored;

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
            MetadataContainer reqMsg = package.Request;
            MetadataContainer rspMsg = new MetadataContainer();
            MessageIdentity identity = reqMsg.GetAttributeAsType<MessageIdentity>(0x00);
            identity.DetailsId += 1;
            rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(identity));
            package.SendResponse(rspMsg);
        }
    }
}