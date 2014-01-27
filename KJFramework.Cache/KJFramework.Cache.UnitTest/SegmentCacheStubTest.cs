using System;
using System.Text;
using KJFramework.Cache.Cores;
using KJFramework.Cache.Indexers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KJFramework.Cache.UnitTest
{
    [TestClass]
    public class SegmentCacheStubTest
    {
        [Test]
        public void ConstructorTest()
        {
            byte[] data = new byte[102400];
            ICacheIndexer cacheIndexer = new CacheIndexer {BeginOffset = 0, CacheBuffer = data, SegmentSize = 1024};
            SegmentCacheStub stub = new SegmentCacheStub(cacheIndexer);
            Assert.IsFalse(stub.IsUsed);
            Assert.IsFalse(stub.GetLease().CanTimeout);
            Assert.IsFalse(stub.GetLease().IsDead);
        }

        [Test]
        public void ConstructorWithTimeoutTest()
        {
            byte[] data = new byte[102400];
            ICacheIndexer cacheIndexer = new CacheIndexer { BeginOffset = 0, CacheBuffer = data, SegmentSize = 1024 };
            SegmentCacheStub stub = new SegmentCacheStub(cacheIndexer, DateTime.Now.AddMinutes(3));
            Assert.IsFalse(stub.IsUsed);
            Assert.IsTrue(stub.GetLease().CanTimeout);
            Assert.IsFalse(stub.GetLease().IsDead);
        }

        [Test]
        public void SetValueTest()
        {
            byte[] data = new byte[102400];
            ICacheIndexer cacheIndexer = new CacheIndexer { BeginOffset = 0, CacheBuffer = data, SegmentSize = 1024 };
            SegmentCacheStub stub = new SegmentCacheStub(cacheIndexer, DateTime.Now.AddMinutes(3));
            Assert.IsFalse(stub.IsUsed);
            Assert.IsTrue(stub.GetLease().CanTimeout);
            Assert.IsFalse(stub.GetLease().IsDead);
            stub.Cache.SetValue(Encoding.Default.GetBytes("1234567890"));
            Assert.IsTrue(stub.IsUsed);
        }

        [Test]
        public void GetValueTest()
        {
            byte[] data = new byte[102400];
            ICacheIndexer cacheIndexer = new CacheIndexer { BeginOffset = 0, CacheBuffer = data, SegmentSize = 1024 };
            SegmentCacheStub stub = new SegmentCacheStub(cacheIndexer, DateTime.Now.AddMinutes(3));
            Assert.IsFalse(stub.IsUsed);
            Assert.IsTrue(stub.GetLease().CanTimeout);
            Assert.IsFalse(stub.GetLease().IsDead);
            byte[] bytes = stub.Cache.GetValue();
            Assert.IsFalse(stub.IsUsed);
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 1024);
        }

        [Test]
        public void GetValueWithUsedTest()
        {
            byte[] data = new byte[102400];
            ICacheIndexer cacheIndexer = new CacheIndexer { BeginOffset = 0, CacheBuffer = data, SegmentSize = 1024 };
            SegmentCacheStub stub = new SegmentCacheStub(cacheIndexer, DateTime.Now.AddMinutes(3));
            Assert.IsFalse(stub.IsUsed);
            Assert.IsTrue(stub.GetLease().CanTimeout);
            Assert.IsFalse(stub.GetLease().IsDead);
            stub.Cache.SetValue(Encoding.Default.GetBytes("1234567890"));
            Assert.IsTrue(stub.IsUsed);
            byte[] bytes = stub.Cache.GetValue();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 10);
        }

        [Test]
        public void InitializeTest()
        {
            byte[] data = new byte[102400];
            ICacheIndexer cacheIndexer = new CacheIndexer { BeginOffset = 0, CacheBuffer = data, SegmentSize = 1024 };
            SegmentCacheStub stub = new SegmentCacheStub(cacheIndexer, DateTime.Now.AddMinutes(3));
            Assert.IsFalse(stub.IsUsed);
            Assert.IsTrue(stub.GetLease().CanTimeout);
            Assert.IsFalse(stub.GetLease().IsDead);
            stub.Cache.SetValue(Encoding.Default.GetBytes("1234567890"));
            Assert.IsTrue(stub.IsUsed);
            byte[] bytes = stub.Cache.GetValue();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 10);
            stub.Initialize();
            Assert.IsFalse(stub.IsUsed);
        }
    }
}