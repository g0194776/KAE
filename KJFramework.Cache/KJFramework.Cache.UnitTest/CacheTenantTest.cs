using System;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Cores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KJFramework.Cache.UnitTest
{
    internal class TestClass : IClearable
    {
        public string Name = "YangJie";

        #region Implementation of IClearable

        /// <summary>
        ///     清除对象自身
        /// </summary>
        public void Clear()
        {
            Name = null;
        }

        #endregion
    }

    [TestClass]
    public class CacheTenantTest
    {
        [Test]
        public void LocalRentTest1()
        {
            ICacheTenant tenant = new CacheTenant();
            ICacheContainer<string, string> cacheContainer = tenant.Rent<string, string>("CATEGORY-1");
            Assert.IsNotNull(cacheContainer);
            Assert.IsTrue(cacheContainer.ExpireTime == DateTime.MaxValue);
        }

        [Test]
        public void LocalRentTest2()
        {
            ICacheTenant tenant = new CacheTenant();
            IFixedCacheContainer<TestClass> fixedCacheContainer = tenant.Rent<TestClass>("CATEGORY-1", 100000);
            Assert.IsNotNull(fixedCacheContainer);
            Assert.IsTrue(fixedCacheContainer.Capacity == 100000);
            IFixedCacheStub<TestClass> fixedCacheStub = fixedCacheContainer.Rent();
            Assert.IsNotNull(fixedCacheStub);
            fixedCacheContainer.Giveback(fixedCacheStub);
            Assert.IsTrue(fixedCacheStub.Cache.Name == null);
        }

        [Test]
        public void GetTest()
        {
            ICacheTenant tenant = new CacheTenant();
            ICacheContainer<string, string> cacheContainer = tenant.Rent<string, string>("CATEGORY-1");
            Assert.IsNotNull(cacheContainer);
            Assert.IsTrue(cacheContainer.ExpireTime == DateTime.MaxValue);
            ICacheContainer<string, string> container = tenant.Get<ICacheContainer<string, string>>("CATEGORY-1");
            Assert.IsNotNull(container);
        }

        [Test]
        public void GetAndNullTest()
        {
            ICacheTenant tenant = new CacheTenant();
            ICacheContainer<string, string> cacheContainer = tenant.Rent<string, string>("CATEGORY-1");
            Assert.IsNotNull(cacheContainer);
            Assert.IsTrue(cacheContainer.ExpireTime == DateTime.MaxValue);
            tenant.Discard("CATEGORY-1");
            ICacheContainer<string, string> container = tenant.Get<ICacheContainer<string, string>>("CATEGORY-1");
            Assert.IsNull(container);
        }

        [Test]
        public void DiscardTest()
        {
            ICacheTenant tenant = new CacheTenant();
            ICacheContainer<string, string> cacheContainer = tenant.Rent<string, string>("CATEGORY-1");
            Assert.IsNotNull(cacheContainer);
            Assert.IsTrue(cacheContainer.ExpireTime == DateTime.MaxValue);
            tenant.Discard("CATEGORY-1");
            ICacheContainer<string, string> container = tenant.Get<ICacheContainer<string, string>>("CATEGORY-1");
            Assert.IsNull(container);
        }
    }
}