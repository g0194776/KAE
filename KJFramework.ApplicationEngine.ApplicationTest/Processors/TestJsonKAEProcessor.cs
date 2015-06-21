using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Objects;

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
            IMessageTransactionProxy<MetadataContainer> transactionProxy = SystemWorker.GetTransactionProxy<MetadataContainer>(ProtocolTypes.Metadata);
            IMessageTransaction<MetadataContainer> transaction = transactionProxy.CreateTransaction(new Protocols {ProtocolId = 1, ServiceId = 0, DetailsId = 3}, null);
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<MetadataContainer> e)
            {
                package.SendResponse(e.Target);
            };
            MetadataContainer reqMsg = new MetadataContainer();
            reqMsg.SetAttribute(0x05, new ByteValueStored((byte)ApplicationLevel.Stable));
            reqMsg.SetAttribute(0x0A, new StringValueStored("Hello, KAE APP"));
            transaction.SendRequest(reqMsg);
        }
    }
}