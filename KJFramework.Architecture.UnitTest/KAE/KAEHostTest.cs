using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Channels.HostChannels;
using NUnit.Framework;

namespace KJFramework.Architecture.UnitTest.KAE
{
    public class KAEHostTest
    {
        #region Methods.

        [Test]
        public void StartTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path);
            try
            {
                host.Start();
                FieldInfo field = host.GetType().GetField("_hostChannels", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(field);
                List<IHostTransportChannel> channels = (List<IHostTransportChannel>) field.GetValue(host);
                Assert.IsNotNull(channels);
                Assert.IsTrue(channels.Count > 0);
                Assert.IsTrue(host.Status == KAEHostStatus.Started);
            }
            finally
            {
                if (File.Exists(file)) File.Delete(file);
                host.Stop();
                Assert.IsTrue(host.Status == KAEHostStatus.Stopped);
            }
        }

        [Test]
        public void StartWithCustomizeNetworkTest()
        {
            KPPResourceTest resource = new KPPResourceTest();
            Console.Write("#Packing package...");
            string file = resource.PackTestWithoutDelete();
            string path = Path.GetDirectoryName(file);
            Assert.IsTrue(Directory.Exists(path));
            Console.WriteLine("Done");
            KAEHost host = new KAEHost(path);
            try
            {
                host.Start();
                FieldInfo field = host.GetType().GetField("_hostChannels", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(field);
                List<IHostTransportChannel> channels = (List<IHostTransportChannel>)field.GetValue(host);
                Assert.IsNotNull(channels);
                Assert.IsTrue(channels.Count > 1);
                Assert.IsTrue(host.Status == KAEHostStatus.Started);
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