using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.ApplicationEngine.Finders;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Resources;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class ApplicationDynamicObjectTest
    {
        #region Methods.

        [Test]
        public void ConstructTest()
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
                IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> apps = ApplicationFinder.Search(path);
                Assert.IsNotNull(apps);
                Assert.IsTrue(apps.Count == 1);
                IList<Tuple<ApplicationEntryInfo, KPPDataStructure>> tuples;
                Assert.IsTrue(apps.TryGetValue("test.package", out tuples));
                Assert.IsNotNull(tuples);
                Assert.IsTrue(tuples.Count == 1);
                Tuple<ApplicationEntryInfo, KPPDataStructure> tuple = tuples[0];
                Assert.IsNotNull(tuple.Item1);
                Assert.IsNotNull(tuple.Item2);
                ApplicationDynamicObject dynamicObject = new ApplicationDynamicObject(tuple.Item1, tuple.Item2, null, null);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }


        [Test]
        public void StartTest()
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
                IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> apps = ApplicationFinder.Search(path);
                Assert.IsNotNull(apps);
                Assert.IsTrue(apps.Count > 0);
                IList<Tuple<ApplicationEntryInfo, KPPDataStructure>> tuples;
                Assert.IsTrue(apps.TryGetValue("test.package", out tuples));
                Assert.IsNotNull(tuples);
                Assert.IsTrue(tuples.Count == 1);
                Tuple<ApplicationEntryInfo, KPPDataStructure> tuple = tuples[0];
                Assert.IsNotNull(tuple.Item1);
                Assert.IsNotNull(tuple.Item2);
                ApplicationDynamicObject dynamicObject = new ApplicationDynamicObject(tuple.Item1, tuple.Item2, null, null);
                dynamicObject.Start();
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        #endregion
    }
}