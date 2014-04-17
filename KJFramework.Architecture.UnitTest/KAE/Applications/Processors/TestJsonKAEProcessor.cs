using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Packages;
using KJFramework.ApplicationEngine.Processors;

namespace KJFramework.Architecture.UnitTest.KAE.Applications.Processors
{
    [KAEProcessorProperties(ProtocolId = 1, ServiceId = 0, DetailsId = 2)]
    public class TestJsonKAEProcessor : JsonKAEProcessor
    {
        public TestJsonKAEProcessor(IApplication application) : base(application)
        {
        }

        public override void InnerProcess(IBusinessPackage<string> package)
        {
        }
    }
}