using System;
using System.IO;
using KJFramework.ApplicationEngine.Resources.Packs;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    internal class KPPDataHeadTest
    {
        #region Methods.

        [Test]
        public void PackTest()
        {
            KPPDataHead head = new KPPDataHead();
            MemoryStream stream = new MemoryStream();
            head.Pack(stream);
            byte[] buffer = stream.GetBuffer();
            byte[] data = new byte[stream.Length];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 21);
            Assert.IsTrue(data[0] == 'K');
            Assert.IsTrue(data[1] == 'P');
            Assert.IsTrue(data[2] == 'P');
        }

        [Test]
        public void UnPackTest()
        {
            KPPDataHead head = new KPPDataHead();
            MemoryStream stream = new MemoryStream();
            head.Pack(stream);
            byte[] buffer = stream.GetBuffer();
            byte[] data = new byte[stream.Length];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 21);
            Assert.IsTrue(data[0] == 'K');
            Assert.IsTrue(data[1] == 'P');
            Assert.IsTrue(data[2] == 'P');
            //reset postition.
            stream.Position = 0;
            KPPDataHead newHead = new KPPDataHead();
            newHead.UnPack(stream);
            Assert.IsTrue(newHead.GetField<ulong>("TotalSize") == 0UL);
            Assert.IsTrue(newHead.GetField<long>("CRC") == 0L);
            Assert.IsTrue(newHead.GetField<ushort>("SectionCount") == 0);
        }

        [Test]
        public void UnPackTest2()
        {
            KPPDataHead head = new KPPDataHead();
            MemoryStream stream = new MemoryStream();
            head.SetField("CRC", 100L);
            head.SetField("SectionCount", (ushort)1);
            head.Pack(stream);
            byte[] buffer = stream.GetBuffer();
            byte[] data = new byte[stream.Length];
            Buffer.BlockCopy(buffer, 0, data, 0, data.Length);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 21);
            Assert.IsTrue(data[0] == 'K');
            Assert.IsTrue(data[1] == 'P');
            Assert.IsTrue(data[2] == 'P');
            //reset postition.
            stream.Position = 0;
            KPPDataHead newHead = new KPPDataHead();
            newHead.UnPack(stream);
            Assert.IsTrue(newHead.GetField<ulong>("TotalSize") == 0UL);
            Assert.IsTrue(newHead.GetField<long>("CRC") == 100L);
            Assert.IsTrue(newHead.GetField<ushort>("SectionCount") == 1);
        }

        #endregion
    }
}