using System.Collections.Generic;
using System.IO;
using System.Linq;
using KJFramework.ApplicationEngine.Helpers;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class FileCompressionTest
    {
        #region Methods.

        [Test]
        public void CompressTest()
        {
            List<FileInfo> files = Directory.GetFiles("res-files").Select(f => new FileInfo(f)).ToList();
            byte[] data = FileCompression.CompressFile(files, 9, false);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length > 0);
        }

        #endregion
    }
}