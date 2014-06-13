using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.ApplicationTest.Processors
{
    [KAEProcessorProperties(ProtocolId = 1, ServiceId = 0, DetailsId = 4)]
    public class TestJsonKAEProcessor3 : IntellegenceKAEProcessor
    {
        public TestJsonKAEProcessor3(IApplication application)
            : base(application)
        {
        }

        protected override void InnerProcess(IMessageTransaction<IntellectObject> package)
        {
            throw new System.NotImplementedException();
        }
    }
}