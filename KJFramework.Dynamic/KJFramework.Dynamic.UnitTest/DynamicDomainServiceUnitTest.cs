using System.Diagnostics;
using System.Threading;
using KJFramework.Dynamic.Components;
using KJFramework.Dynamic.Structs;
using KJFramework.EventArgs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Dynamic.UnitTest
{
    [TestClass]
    public class DynamicDomainServiceUnitTest
    {
        private static string _workRoot = @"H:\Work Base\KJFramework\KJFramework\KJFramework.Dynamic\HostService\bin\Debug\Components\";
        [TestMethod]
        public void ConstructorTest()
        {
            DynamicDomainService service = new DynamicDomainService(_workRoot, new ServiceDescriptionInfo
                                                                                   {
                                                                                       Name = "Dynamic Unit Test",
                                                                                       ServiceName = "KJFramework.Dynamic.UnitTest"
                                                                                   });
        }

        [TestMethod]
        public void ConstructorWithoutArgTest()
        {
            DynamicDomainService service = new DynamicDomainService(_workRoot, new ServiceDescriptionInfo
            {
                Name = "Dynamic Unit Test",
                ServiceName = "KJFramework.Dynamic.UnitTest"
            });
            Assert.IsNotNull(service.WorkRoot);
        }

        [TestMethod]
        public void InitializeTest()
        {
            DynamicDomainService service = new DynamicDomainService(_workRoot, new ServiceDescriptionInfo
            {
                Name = "Dynamic Unit Test",
                ServiceName = "KJFramework.Dynamic.UnitTest"
            });
            Assert.IsTrue(service.ComponentCount > 0);
        }

        [TestMethod]
        public void StartTest()
        {
            DynamicDomainService service = new DynamicDomainService(_workRoot, new ServiceDescriptionInfo
            {
                Name = "Dynamic Unit Test",
                ServiceName = "KJFramework.Dynamic.UnitTest"
            });
            service.WorkingProcess += WorkingProcess;
            Assert.IsTrue(service.ComponentCount > 0);
            service.Start();
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            autoResetEvent.WaitOne(5000);
        }

        void WorkingProcess(object sender, LightSingleArgEventArgs<string> e)
        {
            Debug.WriteLine(e.Target);
        }
    }
}