using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Net.Transaction;

namespace KJFramework.Architecture.UnitTest.KAE.Applications.Processors
{
    [KAEProcessorProperties(ProtocolId = 1, ServiceId = 0, DetailsId = 2)]
    public class TestJsonKAEProcessor : JsonKAEProcessor
    {
        public TestJsonKAEProcessor(IApplication application) : base(application)
        {
        }

        protected override void InnerProcess(IMessageTransaction<string> package)
        {
        }
    }
}