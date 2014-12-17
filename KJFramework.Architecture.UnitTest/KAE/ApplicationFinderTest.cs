using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Factories;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Resources;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class ApplicationFinderTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            SystemWorker.Initialize("KAEWroker", RemoteConfigurationSetting.Default, KAEHostTest.BuildConfigurationProxy());
            KAESystemInternalResource.Factory = new DefaultInternalResourceFactory();
            KAESystemInternalResource.Factory.Initialize();
        }

        [Test]
        [Description("针对于pck1/test.package.kpp的查找测试")]
        public void SearchTest()
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
                Assert.IsTrue(apps.Count == 1);
                IList<Tuple<ApplicationEntryInfo, KPPDataStructure>> tuples;
                Assert.IsTrue(apps.TryGetValue("test.package", out tuples));
                Assert.IsNotNull(tuples);
                Assert.IsTrue(tuples.Count == 1);
                Tuple<ApplicationEntryInfo, KPPDataStructure> tuple = tuples[0];
                Assert.IsNotNull(tuple.Item1);
                Assert.IsNotNull(tuple.Item2);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        #endregion
    }
}