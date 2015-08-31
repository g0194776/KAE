using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Factories;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Architecture.UnitTest.KAE.Applications;
using KJFramework.Net.Identities;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class ApplicationTest
    {
        [SetUp]
        public void Initialize()
        {
            SystemWorker.Initialize("KAEWorker", RemoteConfigurationSetting.Default, new SolitaryRemoteConfigurationProxy());
            KAESystemInternalResource.Factory = new DefaultInternalResourceFactory();
            KAESystemInternalResource.Factory.Initialize();
        }

        [Test]
        public TestApplication InitializeTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            try
            {
                Assert.IsTrue(Directory.Exists(path));
                Console.WriteLine("Done");
                Console.WriteLine("#Target kpp path: " + file);
                IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> apps = ((IApplicationFinder)KAESystemInternalResource.Factory.GetResource(KAESystemInternalResource.APPFinder)).Search(path);
                Assert.IsNotNull(apps);
                Assert.IsTrue(apps.Count > 0);
                IList<Tuple<ApplicationEntryInfo, KPPDataStructure>> tuples;
                Assert.IsTrue(apps.TryGetValue("test.package", out tuples));
                Assert.IsNotNull(tuples);
                Tuple<ApplicationEntryInfo, KPPDataStructure> tuple = tuples[0];
                Assert.IsNotNull(tuple.Item1);
                Assert.IsNotNull(tuple.Item2);
                TestApplication application = new TestApplication();
                Assert.IsTrue(application.Status == ApplicationStatus.Unknown);
                application.Initialize(tuple.Item2, null, null);
                Assert.IsTrue(application.PackageName == "test.package");
                Assert.IsTrue(application.Version == "2.0.0");
                Assert.IsTrue(application.Description == "test package description.");
                Assert.IsTrue(application.GlobalUniqueId == tuple.Item2.GetSectionField<Guid>(0x00, "GlobalUniqueIdentity"));
                Assert.IsTrue(application.Status == ApplicationStatus.Initialized);
                FieldInfo fieldInfo = typeof(Application).GetField("_processors", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(fieldInfo);
                IDictionary<ProtocolTypes, Dictionary<MessageIdentity, object>> processors = (IDictionary<ProtocolTypes, Dictionary<MessageIdentity, object>>) fieldInfo.GetValue(application);
                Assert.IsNotNull(processors);
                return application;
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        [Test]
        public void StartTest()
        {
            TestApplication app = InitializeTest();
            Assert.IsNotNull(app);
            app.OnLoading();
            app.Start();
            Assert.IsTrue(app.Status == ApplicationStatus.Running);
            Assert.IsTrue(!string.IsNullOrEmpty(app.TunnelAddress));
            Console.WriteLine("Tunnel Address: " + app.TunnelAddress);
        }

        [Test]
        public void StopTest()
        {
            TestApplication app = InitializeTest();
            Assert.IsNotNull(app);
            app.OnLoading();
            app.Start();
            Assert.IsTrue(app.Status == ApplicationStatus.Running);
            Assert.IsTrue(!string.IsNullOrEmpty(app.TunnelAddress));
            Console.WriteLine("Tunnel Address: " + app.TunnelAddress);
            app.Stop();
            Assert.IsTrue(app.Status == ApplicationStatus.Stopped);
        }

        [Test]
        public void GetAbilityTest()
        {
            TestApplication app = InitializeTest();
            Assert.IsNotNull(app);
            app.Start();
            Assert.IsTrue(app.Status == ApplicationStatus.Running);
            IDictionary<ProtocolTypes, IList<MessageIdentity>> protocols = app.AcquireSupportedProtocols();
            Assert.IsNotNull(protocols);
            Assert.IsTrue(protocols.Count > 0);
        }
    }
}