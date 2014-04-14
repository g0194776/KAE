using System;
using System.IO;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.ApplicationEngine.Resources.Packs;
using KJFramework.ApplicationEngine.Resources.Packs.Sections;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class KPPResourceTest
    {
        #region Methods.

        [Test]
        public void PackTest()
        {
            DateTime now = DateTime.Now;
            string fileName = DateTime.Now.Ticks + ".kpp";
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", "test.package");
            section.SetField("PackDescription", "test package description.");
            section.SetField("EnvironmentFlag", (byte)0x01);
            section.SetField("Version", "1.0.0");
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", "1.txt");
            section.SetField("GlobalUniqueIdentity", guid);
            KPPResource.Pack("res-files", fileName, head, section);
            Assert.IsTrue(File.Exists(fileName));
            File.Delete(fileName);
        }

        [Test]
        public void PackTest2()
        {
            DateTime now = DateTime.Now;
            string fileName = DateTime.Now.Ticks + ".kpp";
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", "test.package");
            section.SetField("PackDescription", "test package description.");
            section.SetField("EnvironmentFlag", (byte)0x01);
            section.SetField("Version", "1.0.0");
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", "KJFramework.ApplicationEngine.ApplicationTest.dll");
            section.SetField("GlobalUniqueIdentity", guid);
            KPPResource.Pack(@"..\..\..\KJFramework.ApplicationEngine.ApplicationTest\bin\Debug\", fileName, head, section);
            Assert.IsTrue(File.Exists(fileName));
            File.Delete(fileName);
        }

        [Test]
        public string PackTestWithoutDelete()
        {
            DateTime now = DateTime.Now;
            string fileName = DateTime.Now.Ticks + ".kpp";
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", "test.package");
            section.SetField("PackDescription", "test package description.");
            section.SetField("EnvironmentFlag", (byte)0x01);
            section.SetField("Version", "1.0.0");
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", "KJFramework.ApplicationEngine.ApplicationTest.dll");
            section.SetField("GlobalUniqueIdentity", guid);
            KPPResource.Pack(@"..\..\..\KJFramework.ApplicationEngine.ApplicationTest\bin\Debug\", fileName, head, section);
            Assert.IsTrue(File.Exists(fileName));
            return Path.GetFullPath(fileName);
        }

        [Test]
        public void UnPackTest()
        {
            DateTime now = DateTime.Now;
            string fileName = DateTime.Now.Ticks + ".kpp";
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", "test.package");
            section.SetField("PackDescription", "test package description.");
            section.SetField("EnvironmentFlag", (byte)0x01);
            section.SetField("Version", "1.0.0");
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", "1.txt");
            section.SetField("GlobalUniqueIdentity", guid);
            KPPResource.Pack("res-files", fileName, head, section);
            Assert.IsTrue(File.Exists(fileName));

            KPPDataStructure structure = KPPResource.UnPack(fileName);
            Assert.IsNotNull(structure);


            File.Delete(fileName);
        }

        #endregion
    }
}