using System;
using System.IO;
using KJFramework.ApplicationEngine.Resources.Packs.Sections;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class PackageAttributeDataSectionTest
    {
        #region Methods.

        [Test]
        public void PackTest()
        {
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            MemoryStream stream = new MemoryStream();
            section.Pack(stream);
            byte[] buffer = stream.GetBuffer();
            byte[] data = new byte[stream.Length];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
        }

        [Test]
        public void UnPackTest()
        {
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            MemoryStream stream = new MemoryStream();
            section.Pack(stream);
            byte[] buffer = stream.GetBuffer();
            byte[] data = new byte[stream.Length];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
            //reset postition.
            stream.Position = 0;
            PackageAttributeDataSection newSection = new PackageAttributeDataSection();
            newSection.UnPack(stream);
            Assert.IsTrue(string.IsNullOrEmpty(newSection.GetField<string>("PackName")));
            Assert.IsTrue(string.IsNullOrEmpty(newSection.GetField<string>("PackDescription")));
            Assert.IsTrue(newSection.GetField<byte>("EnvironmentFlag") == 0x00);
            Assert.IsTrue(string.IsNullOrEmpty(newSection.GetField<string>("Version")));
            Assert.IsTrue(newSection.GetField<DateTime>("PackTime") == DateTime.MinValue);
            Assert.IsTrue(string.IsNullOrEmpty(newSection.GetField<string>("ApplicationMainFileName")));
            Assert.IsTrue(newSection.GetField<Guid>("GlobalUniqueIdentity") == Guid.Empty);
        }

        [Test]
        public void UnPackTest2()
        {
            DateTime now = DateTime.Now;
            Guid guid = Guid.NewGuid();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", "kpp.test1");
            section.SetField("PackDescription", "That is a description of current package.");
            section.SetField("EnvironmentFlag", (byte)1);
            section.SetField("Version", "1.1.0");
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", "1.dll");
            section.SetField("GlobalUniqueIdentity", guid);
            MemoryStream stream = new MemoryStream();
            section.Pack(stream);
            byte[] buffer = stream.GetBuffer();
            byte[] data = new byte[stream.Length];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
            //reset postition.
            stream.Position = 0;
            PackageAttributeDataSection newSection = new PackageAttributeDataSection();
            newSection.UnPack(stream);
            Assert.IsTrue(newSection.GetField<string>("PackName") == "kpp.test1");
            Assert.IsTrue(newSection.GetField<string>("PackDescription") == "That is a description of current package.");
            Assert.IsTrue(newSection.GetField<byte>("EnvironmentFlag") == 0x01);
            Assert.IsTrue(newSection.GetField<string>("Version") == "1.1.0");
            Assert.IsTrue(newSection.GetField<DateTime>("PackTime") == now);
            Assert.IsTrue(newSection.GetField<string>("ApplicationMainFileName") == "1.dll");
            Assert.IsTrue(newSection.GetField<Guid>("GlobalUniqueIdentity") == guid);
        }

        #endregion
    }
}