using System;
using System.Diagnostics;
using System.Threading;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Cores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KJFramework.Cache.UnitTest
{
    [TestClass]
    public class CacheContainerTest
    {
        [Test]
        public void AddTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1");
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
        }

        [Test]
        public void AddForTimeSpanTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1", new TimeSpan(0, 0, 0, 3));
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            Assert.IsTrue(readonlyCacheStub.Lease.CanTimeout);
        }

        [Test]
        public void AddForDateTimeTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1", DateTime.Now.AddSeconds(3));
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            Assert.IsTrue(readonlyCacheStub.Lease.CanTimeout);
        }

        [Test]
        public void RemoveTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1");
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            cacheContainer.Remove("index1");
        }

        [Test]
        public void GetTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1");
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            IReadonlyCacheStub<string> cacheStub = cacheContainer.Get("index1");
            Assert.IsNotNull(cacheStub);
        }

        [Test]
        public void GetWithTimeoutTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1", new TimeSpan(0, 0, 0, 3));
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            Assert.IsTrue(readonlyCacheStub.Lease.CanTimeout);
            //sleep 13s.
            Thread.Sleep(13000);
            Assert.IsTrue(readonlyCacheStub.Lease.IsDead);
            IReadonlyCacheStub<string> cacheStub = cacheContainer.Get("index1");
            Assert.IsNull(cacheStub);
        }

        [Test]
        public void IsExistsTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1");
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            Assert.IsFalse(readonlyCacheStub.Lease.CanTimeout);
            Assert.IsTrue(cacheContainer.IsExists("index1"));
        }

        [Test]
        public void IsExistsWithTimeoutTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1", new TimeSpan(0, 0, 0, 3));
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            Assert.IsTrue(readonlyCacheStub.Lease.CanTimeout);
            //sleep 13s.
            Thread.Sleep(13000);
            Assert.IsTrue(readonlyCacheStub.Lease.IsDead);
            Assert.IsFalse(cacheContainer.IsExists("index1"));
        }

        [Test]
        public void DiscardTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1");
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            cacheContainer.Discard();
            //can & cannot notify it.
            Assert.IsTrue(readonlyCacheStub.Lease.IsDead);
        }

        [Test]
        public void DiscardWithTimeoutTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1");
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            cacheContainer.Discard();
            //can & cannot notify it.
            Assert.IsTrue(readonlyCacheStub.Lease.IsDead);
            Thread.Sleep(13000);
            Assert.IsTrue(cacheContainer.IsDead);
            System.Exception exception = null;
            try
            {
                Assert.IsFalse(cacheContainer.IsExists("index1"));
            }
            catch (System.Exception ex)
            {
                exception = ex;
                Debug.WriteLine(ex.Message);
            }
            Assert.IsNotNull(exception);
        }

        [Test]
        public void RenewTest()
        {
            CacheContainer<string, string> cacheContainer = new CacheContainer<string, string>("CATEGORY-1");
            IReadonlyCacheStub<string> readonlyCacheStub = cacheContainer.Add("index1", "value1", new TimeSpan(0, 0, 0, 3));
            DateTime exTiem1 = readonlyCacheStub.Lease.ExpireTime;
            Assert.IsNotNull(readonlyCacheStub);
            Assert.IsNotNull(readonlyCacheStub.Cache);
            Assert.IsFalse(readonlyCacheStub.Lease.IsDead);
            Assert.IsTrue(readonlyCacheStub.Lease.CanTimeout);
            cacheContainer.Renew(new TimeSpan(0, 0, 0, 5));
            DateTime exTiem2 = readonlyCacheStub.Lease.ExpireTime;
            Assert.IsTrue(exTiem2 > exTiem1);
            //can & cannot notify it.
            Thread.Sleep(13000);
            Assert.IsTrue(readonlyCacheStub.Lease.IsDead);
            Assert.IsTrue(cacheContainer.IsDead);
            System.Exception exception = null;
            try
            {
                Assert.IsFalse(cacheContainer.IsExists("index1"));
            }
            catch (System.Exception ex)
            {
                exception = ex;
                Debug.WriteLine(ex.Message);
            }
            Assert.IsNotNull(exception);
        }
    }
}