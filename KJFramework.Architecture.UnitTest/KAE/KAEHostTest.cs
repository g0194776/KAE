using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.Net.Channels.HostChannels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.Remoting;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class KAEHostTest
    {
        #region Methods.

        [Test]
        [Description("已装配清单的方式启动一个KAE宿主")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FailRunWithRemotingInstallingFileMode()
        {
            KAEHost host = new KAEHost(string.Format("{0}filenotfound.kl", DriveInfo.GetDrives()[0].Name));
            host.Start();
        }

        [Test]
        [Description("已装配清单的方式启动一个KAE宿主，但是装配清单文件内部什么内容都没有")]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void RemotingInstallingFileMode_Empty()
        {
            KAEHost host = new KAEHost(string.Format("{0}empty.kl", Path.GetFullPath("../../KAE/Scripts/")));
            host.Start();
        }
        [Test]
        [Description("已装配清单的方式启动一个KAE宿主，装配清单中的名称不符合规则")]
        [ExpectedException(typeof(FormatException))]
        public void RemotingInstallingFileMode_BadFormatPackageName()
        {
            KAEHost host = new KAEHost(string.Format("{0}demo-pkgs.kl", Path.GetFullPath("../../KAE/Scripts/")));
            host.Start();
        }

        [Test]
        [Description("已装配清单的方式启动一个KAE宿主，但无法从远程KIS获取到任何信息")]
        [ExpectedException(typeof(RemotingException))]
        public void RemotingInstallingFileMode_CannotGetRealAddress()
        {
            KAEHost host = new KAEHost(string.Format("{0}demo-pkgs2.kl", Path.GetFullPath("../../KAE/Scripts/")));
            host.Start();
        }

        [Test]
        [Description("最基础的KAE宿主初始化操作测试")]
        public void ElementaryCommunicationTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            foreach (string tempFile in Directory.GetFiles(path, "*.kpp")) if (tempFile != file) File.Delete(tempFile);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path, null);
            try
            {
                host.Start();
                Assert.IsTrue(host.Status == KAEHostStatus.Started || host.Status == KAEHostStatus.Prepared);
                FieldInfo field = typeof(KAEHostNetworkResourceManager).GetField("_resources", BindingFlags.NonPublic | BindingFlags.Static);
                Assert.IsNotNull(field);
                Dictionary<ProtocolTypes, Tuple<IHostTransportChannel, Uri>> channels = (Dictionary<ProtocolTypes, Tuple<IHostTransportChannel, Uri>>)field.GetValue(null);
                Assert.IsNotNull(channels);
                Assert.IsTrue(channels.Count > 1);
                Assert.IsTrue(channels.ContainsKey(ProtocolTypes.INTERNAL_SPECIAL_RESOURCE));
                Assert.IsTrue(channels.ContainsKey(ProtocolTypes.Json));
                Assert.IsTrue(channels.ContainsKey(ProtocolTypes.Xml));
                Assert.IsTrue(channels.ContainsKey(ProtocolTypes.Metadata));
                Assert.IsTrue(channels.ContainsKey(ProtocolTypes.Intellegence));
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                host.Stop();
                Assert.IsTrue(host.Status == KAEHostStatus.Stopped);
            }
        }

        
        #endregion

    }
}