using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Dynamic.Structs;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class ApplicationFinderTest
    {
        #region Methods.

        [Test]
        [Description("针对于pck1/test.package.kpp的查找测试")]
        public void SearchTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            Console.WriteLine("#Target kpp path: " + file);
            try
            {
                IDictionary<string, IList<Tuple<DomainComponentEntryInfo, KPPDataStructure>>> apps = ApplicationFinder.Search(path);
                Assert.IsNotNull(apps);
                Assert.IsTrue(apps.Count == 1);
                IList<Tuple<DomainComponentEntryInfo, KPPDataStructure>> tuples;
                Assert.IsTrue(apps.TryGetValue("test.package", out tuples));
                Assert.IsNotNull(tuples);
                Assert.IsTrue(tuples.Count == 1);
                Tuple<DomainComponentEntryInfo, KPPDataStructure> tuple = tuples[0];
                Assert.IsNotNull(tuple.Item1);
                Assert.IsNotNull(tuple.Item2);
            }
            finally
            {
                if (File.Exists(file)) File.Decrypt(file);
            }
        }

        #endregion
    }
}