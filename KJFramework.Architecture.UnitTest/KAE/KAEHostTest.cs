using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Managers;
using KJFramework.ApplicationEngine.Proxies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.Remoting;
using KJFramework.Net.HostChannels;
using Rhino.Mocks;
using Uri = KJFramework.Net.Uri.Uri;

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
            KAEHost host = new KAEHost(string.Format("{0}filenotfound.kl", DriveInfo.GetDrives()[0].Name), BuildConfigurationProxy());
            host.Start();
        }

        [Test]
        [Description("已装配清单的方式启动一个KAE宿主，但是装配清单文件内部什么内容都没有")]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void RemotingInstallingFileMode_Empty()
        {
            KAEHost host = new KAEHost(string.Format("{0}empty.kl", Path.GetFullPath("../../KAE/Scripts/")), BuildConfigurationProxy());
            host.Start();
        }
        [Test]
        [Description("已装配清单的方式启动一个KAE宿主，装配清单中的名称不符合规则")]
        [ExpectedException(typeof(FormatException))]
        public void RemotingInstallingFileMode_BadFormatPackageName()
        {
            KAEHost host = new KAEHost(string.Format("{0}demo-pkgs.kl", Path.GetFullPath("../../KAE/Scripts/")), BuildConfigurationProxy());
            host.Start();
        }

        [Test]
        [Description("已装配清单的方式启动一个KAE宿主，但无法从远程KIS获取到任何信息")]
        [ExpectedException(typeof(RemotingException))]
        public void RemotingInstallingFileMode_CannotGetRealAddress()
        {
            KAEHost host = new KAEHost(string.Format("{0}demo-pkgs2.kl", Path.GetFullPath("../../KAE/Scripts/")), BuildConfigurationProxy());
            host.Start();
        }

        [Test]
        [Description("使用本地模式启动KAE宿主，但是不包含任何本地KPP")]
        public void LocalMode_RunningWithNoApp()
        {
            KAESettings.IsTDDTesting = true;
            IRemoteConfigurationProxy remoteConfigurationProxy = BuildConfigurationProxy();
            KAEHost host = new KAEHost(".", installingListFile: null, configurationProxy: remoteConfigurationProxy);
            host.Start();
            Assert.IsTrue(host.Status == KAEHostStatus.Prepared);
        }

        [Test]
        [Description("最基础的KAE宿主初始化操作测试")]
        public void ElementaryCommunicationTest()
        {
            KAESettings.IsTDDTesting = true;
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            foreach (string tempFile in Directory.GetFiles(path, "*.kpp")) if (tempFile != file) File.Delete(tempFile);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path, installingListFile: null, configurationProxy: BuildConfigurationProxy());
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

        /// <summary>
        ///    Build a mocking instance of remoting configuration proxy for testing.
        /// </summary>
        /// <returns>Returning a mocked IRemoteConfigurationProxy instance.</returns>
        public static IRemoteConfigurationProxy BuildConfigurationProxy()
        {
            IRemoteConfigurationProxy remoteConfigurationProxy = MockRepository.GenerateMock<IRemoteConfigurationProxy>();
            remoteConfigurationProxy.Expect(x => x.GetPartialConfig("SECTIONS")).Return("<configSections><section name=\"CustomerConfig\" type=\"KJFramework.Configurations.CustomerConfig, KJFramework\"/></configSections>");
            remoteConfigurationProxy.Expect(x => x.GetPartialConfig("KAEWorker:APP-SETTINGS")).Return(null);
            remoteConfigurationProxy.Expect(x => x.GetPartialConfig("CustomerConfig.System")).Return("<System><!--系统编码器--><Encoder Id=\"Default\" Num=\"65001\"/></System>");
            remoteConfigurationProxy.Expect(x => x.GetPartialConfig("KJFRAMEWORK-FAMILY")).Return("<KJFramework>      <!--KJFramework 网络层配置文件，提供了相关的基础配置，请不要擅自修改。-->      <KJFramework.Net          BufferSize=\"10240\"          BufferPoolSize=\"4096\"          MessageHeaderLength=\"80\"          MessageHeaderFlag=\"#KJMS\"          MessageHeaderFlagLength=\"5\"          MessageHeaderEndFlag=\"€\"          MessageHeaderEndFlagLength=\"1\"          MessageDealerFolder=\"D:Dealers\"          MessageHookFolder=\"D:Hooks\"          SpyFolder=\"D:Spys\"          BasicSessionStringTemplate=\"BASE-KEY:{USERID:{0}}-TIME:{1}\"          UserHreatCheckTimeSpan=\"10000\"          UserHreatTimeout=\"15000\"          UserHreatAlertCount=\"3\"          FSHreatCheckTimeSpan=\"10000\"          FSHreatTimeout=\"15000\"          FSHreatAlertCount=\"3\"          SessionExpireCheckTimeSpan=\"5000\"          DefaultConnectionPoolConnectCount=\"1024\"          PredominantCpuUsage=\"10\"          PredominantMemoryUsage=\"150\"          DefaultChannelGroupLayer=\"3\"          DefaultDecleardSize=\"20\"/>      <KJFramework.Net.Channels        RecvBufferSize=\"20480\"        BuffStubPoolSize=\"1000\"        NoBuffStubPoolSize=\"100000\"        MaxMessageDataLength=\"19456000\"        SegmentSize=\"5120\"        SegmentBuffer=\"10240000\"        NamedPipeBuffStubPoolSize=\"1000\"/>      <KJFramework.Net.Transaction TransactionTimeout=\"00:00:30\" TransactionCheckInterval=\"30\" />      <KJFramework.Data.Synchronization TranTimeout=\"00:00:30\" TranChkInterval=\"25000\"/>    </KJFramework>");
            remoteConfigurationProxy.Expect(x => x.GetPartialConfig("Configuration.DotNetFramework")).Return("<startup>    <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.5\" />  </startup>  <runtime>    <legacyUnhandledExceptionPolicy enabled=\"true\"/>  </runtime>");
            remoteConfigurationProxy.Stub(x => x.GetField("KAEWorker", "RRCS-Address")).Return("127.0.0.1:8000");
            remoteConfigurationProxy.Stub(x => x.GetField("KAEWorker", "KIS-Address")).Return("http://www.kae.com/information/");
            remoteConfigurationProxy.Stub(x => x.GetField("KAEWorker", "GreyPolicyAddress", null)).Return("http://www.kae.com/upgrade/");
            remoteConfigurationProxy.Stub(x => x.GetField("KAEWorker", "GreyPolicyInternal", null)).Return("00:05:00");
            return remoteConfigurationProxy;
        }
        
        #endregion

    }
}