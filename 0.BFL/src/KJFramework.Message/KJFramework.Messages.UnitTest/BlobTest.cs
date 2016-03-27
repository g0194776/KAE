using System;
using System.IO;
using System.Net;
using System.Text;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Types;
using NUnit.Framework;

namespace KJFramework.Messages.UnitTest
{
    [TestFixture]
    public class BlobTest
    {
        #region Methods

        [Test]
        [Description("GZIP压缩测试")]
        public void GZipCompressTest()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();
          
            #endregion

            Blob blob = new Blob(CompressionTypes.GZip, data);
            byte[] compressedData = blob.Compress();
            Assert.IsTrue(compressedData != null);
            Assert.IsTrue(compressedData.Length > 0);
            Assert.IsTrue((CompressionTypes)compressedData[0] == CompressionTypes.GZip);
            Console.WriteLine(blob);
        }


        [Test]
        [Description("BZip2压缩测试")]
        public void BZip2CompressTest2()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();

            #endregion

            Blob blob = new Blob(CompressionTypes.BZip2, data);
            byte[] compressedData = blob.Compress();
            Assert.IsTrue(compressedData != null);
            Assert.IsTrue(compressedData.Length > 0);
            Assert.IsTrue((CompressionTypes)compressedData[0] == CompressionTypes.BZip2);
            Console.WriteLine(blob);
        }

        [Test]
        [Description("GZip解压缩测试")]
        public void GZipDecompressTest()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();

            #endregion

            Blob blob = new Blob(CompressionTypes.GZip, data);
            byte[] compressedData = blob.Compress();
            Assert.IsTrue(compressedData != null);
            Assert.IsTrue(compressedData.Length > 0);
            Console.WriteLine(blob);

            Blob newObj = new Blob(compressedData);
            Assert.IsTrue(newObj.CompressionType == CompressionTypes.GZip);
            byte[] decompressedData = newObj.Decompress();
            Assert.IsTrue(decompressedData != null);
            Assert.IsTrue(decompressedData.Length > 0);
            Assert.IsTrue(data.Length == decompressedData.Length);
            for (int i = 0; i < data.Length; i++)
                Assert.IsTrue(data[i] == decompressedData[i]);
        }

        [Test]
        [Description("BZip2解压缩测试")]
        public void BZip2DecompressTest()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();

            #endregion

            Blob blob = new Blob(CompressionTypes.BZip2, data);
            byte[] compressedData = blob.Compress();
            Assert.IsTrue(compressedData != null);
            Assert.IsTrue(compressedData.Length > 0);
            Assert.IsTrue((CompressionTypes)compressedData[0] == CompressionTypes.BZip2);
            Console.WriteLine(blob);

            Blob newObj = new Blob(compressedData);
            Assert.IsTrue(newObj.CompressionType == CompressionTypes.BZip2);
            byte[] decompressedData = newObj.Decompress();
            Assert.IsTrue(decompressedData != null);
            Assert.IsTrue(decompressedData.Length > 0);
            Assert.IsTrue(data.Length == decompressedData.Length);
            for (int i = 0; i < data.Length; i++)
                Assert.IsTrue(data[i] == decompressedData[i]);
        }

        [Test]
        [Description("解压缩测试")]
        public void CompressionFulLTest()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();

            #endregion

            for (byte i = 0; i < 2; i++)
            {
                Blob blob = new Blob((CompressionTypes)i, data);
                byte[] compressedData = blob.Compress();
                Assert.IsTrue(compressedData != null);
                Assert.IsTrue(compressedData.Length > 0);
                Assert.IsTrue((CompressionTypes)compressedData[0] == (CompressionTypes)i);
                Console.WriteLine(blob);
            }
        }

        #endregion
    }
}