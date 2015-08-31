using System.Net;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Proxies;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Mocks;
using System;
using System.IO;
using System.Management.Instrumentation;
using System.Runtime.Remoting;

namespace KJFramework.Architecture.UnitTest.KAE
{
    [TestFixture]
    public class RemotingApplicationDownloaderTestCases
    {
        #region Methods.

        [Test]
        [Description("传递一个不存在的装配清单文件地址")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void IllegalDestinationInstallingFilePath()
        {
            IRemotingApplicationDownloader downloader = new RemotingApplicationDownloader();
            downloader.DownloadFromList(Path.GetFullPath("."), string.Format("{0}.kl", DateTime.Now.Ticks));
        }

        [Test]
        [Description("传递一个什么内容都没有的装配清单文件地址")]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void EmptyInstallingFile()
        {
            string file = Path.Combine(Path.GetFullPath("."), string.Format("{0}.kl", DateTime.Now.Ticks));
            if (File.Exists(file)) File.Delete(file);
            File.Create(file).Close();
            try
            {
                IRemotingApplicationDownloader downloader = new RemotingApplicationDownloader();
                downloader.DownloadFromList(Path.GetFullPath("."), file);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        [Test]
        [Description("传递一个内部拥有非法格式内容的装配清单文件地址")]
        [ExpectedException(typeof(JsonReaderException))]
        public void IllegalContentFormatInstallingFile()
        {
            string file = Path.Combine(Path.GetFullPath("."), string.Format("{0}.kl", DateTime.Now.Ticks));
            if (File.Exists(file)) File.Delete(file);
            using (StreamWriter writer = File.CreateText(file))
            {
                writer.WriteLine("sdfis98f98s7f234#@$@#$@$&%^&");
                writer.Flush();
            }
            try
            {
                IRemotingApplicationDownloader downloader = new RemotingApplicationDownloader();
                downloader.DownloadFromList(Path.GetFullPath("."), file);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        [Test]
        [Description("传递一个具有错误KPP完整包名格式的装配清单文件地址")]
        [ExpectedException(typeof(FormatException))]
        public void IllegalPackageNameFormat()
        {
            string content = "{packages: [{\"package\":\"test-app0-v1.0.0.0\"},{\"package\":\"test-app1-v1.0.0.0\"}]}";
            string file = Path.Combine(Path.GetFullPath("."), string.Format("{0}.kl", DateTime.Now.Ticks));
            if (File.Exists(file)) File.Delete(file);
            using (StreamWriter writer = File.CreateText(file))
            {
                writer.WriteLine(content);
                writer.Flush();
            }
            try
            {
                IRemotingApplicationDownloader downloader = new RemotingApplicationDownloader();
                downloader.DownloadFromList(Path.GetFullPath("."), file);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        [Test]
        [Description("无法从远程KIS获取KPP详细信息")]
        [ExpectedException(typeof(RemotingException))]
        public void GetRemotingKPPInformationFail()
        {
            FakedInternalResourceFactory factory = new FakedInternalResourceFactory();
            IRemotingKISProxy kisProxy;
            DynamicMock mockedObject = new DynamicMock(typeof(IRemotingKISProxy));
            mockedObject.ExpectAndReturn("GetReallyRemotingAddress", null, "test.app0", "v1.0.0.0");
            kisProxy = (IRemotingKISProxy)mockedObject.MockInstance;
            factory.Resources.Add(KAESystemInternalResource.KISProxy, kisProxy);
            KAESystemInternalResource.Factory = factory;


            string content = "{packages: [{\"package\":\"test.app0-v1.0.0.0\"},{\"package\":\"test.app1-v1.0.0.0\"}]}";
            string file = Path.Combine(Path.GetFullPath("."), string.Format("{0}.kl", DateTime.Now.Ticks));
            if (File.Exists(file)) File.Delete(file);
            using (StreamWriter writer = File.CreateText(file))
            {
                writer.WriteLine(content);
                writer.Flush();
            }
            try
            {
                IRemotingApplicationDownloader downloader = new RemotingApplicationDownloader();
                downloader.DownloadFromList(Path.GetFullPath("."), file);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                mockedObject.Verify();
            }
        }


        [Test]
        [Description("无法从一个KIS返回的包详细地址中下载指定的KPP")]
        [ExpectedException(typeof(WebException))]
        public void DownloadInformationFail()
        {
            FakedInternalResourceFactory factory = new FakedInternalResourceFactory();
            IRemotingKISProxy kisProxy;
            DynamicMock mockedObject = new DynamicMock(typeof(IRemotingKISProxy));
            mockedObject.ExpectAndReturn("GetReallyRemotingAddress",
                new PackageInfo
                {
                    CRC = 88888888,
                    Description = "desc",
                    Identity = "XXXXX-XXXXX-XXXXX-XXXXX",
                    Level = "Stable",
                    Name = "test.app0",
                    PackageName = "test.app0.v1.0.0.0",
                    Url = "http://app.kae.com/download/test.app0.v1.0.0.0.kpp"
                }, "test.app0", "v1.0.0.0");
            kisProxy = (IRemotingKISProxy)mockedObject.MockInstance;
            factory.Resources.Add(KAESystemInternalResource.KISProxy, kisProxy);
            KAESystemInternalResource.Factory = factory;


            string content = "{packages: [{\"package\":\"test.app0-v1.0.0.0\"},{\"package\":\"test.app1-v1.0.0.0\"}]}";
            string file = Path.Combine(Path.GetFullPath("."), string.Format("{0}.kl", DateTime.Now.Ticks));
            if (File.Exists(file)) File.Delete(file);
            using (StreamWriter writer = File.CreateText(file))
            {
                writer.WriteLine(content);
                writer.Flush();
            }
            try
            {
                IRemotingApplicationDownloader downloader = new RemotingApplicationDownloader();
                downloader.DownloadFromList(Path.GetFullPath("."), file);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                mockedObject.Verify();
            }
        }


    #endregion
    }
}