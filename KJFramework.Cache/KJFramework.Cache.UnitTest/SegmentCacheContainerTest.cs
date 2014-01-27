using System;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Cores.Segments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KJFramework.Cache.UnitTest
{
    [TestClass]
    public class SegmentCacheContainerTest
    {
        [Test]
        public void ConstructorTest()
        {
            ISegmentCachePolicy policy = new SegmentCachePolicy(
                                                                new SegmentSizePair {Size = 20480, Percent = 0.4F},
                                                                new SegmentSizePair {Size = 816, Percent = 0.3F},
                                                                new SegmentSizePair {Size = 1024, Percent = 0.2F},
                                                                new SegmentSizePair {Size = 64, Percent = 0.1F});
            SegmentCacheContainer<int> cacheContainer = new SegmentCacheContainer<int>(85000, policy);
        }

        [Test]
        public void AddTest()
        {
            ISegmentCachePolicy policy = new SegmentCachePolicy(
                                                                new SegmentSizePair { Size = 2048, Percent = 0.4F },
                                                                new SegmentSizePair { Size = 256, Percent = 0.3F },
                                                                new SegmentSizePair { Size = 1024, Percent = 0.2F },
                                                                new SegmentSizePair { Size = 64, Percent = 0.1F });
            SegmentCacheContainer<int> cacheContainer = new SegmentCacheContainer<int>(85000, policy);
            Random random = new Random();
            byte[] data = new byte[random.Next(100, 10000)];
            data[0] = 1;
            data[data.Length - 1] = 2;
            bool result = cacheContainer.Add(0, data);
            Assert.IsTrue(result);
        }

        [Test]
        public void GetTest()
        {
            ISegmentCachePolicy policy = new SegmentCachePolicy(
                                                                   new SegmentSizePair { Size = 2048, Percent = 0.4F },
                                                                   new SegmentSizePair { Size = 256, Percent = 0.3F },
                                                                   new SegmentSizePair { Size = 1024, Percent = 0.2F },
                                                                   new SegmentSizePair { Size = 64, Percent = 0.1F });
            SegmentCacheContainer<int> cacheContainer = new SegmentCacheContainer<int>(85000, policy);
            Random random = new Random();
            byte[] data = new byte[random.Next(100, 10000)];
            data[0] = 1;
            data[data.Length - 1] = 2;
            bool result = cacheContainer.Add(0, data);
            Assert.IsTrue(result);
            byte[] temp = cacheContainer.Get(0);
            Assert.IsNotNull(temp);
            Assert.IsTrue(temp.Length == data.Length);
        }

        [Test]
        public void RemoveTest()
        {
            ISegmentCachePolicy policy = new SegmentCachePolicy(
                                                                   new SegmentSizePair { Size = 2048, Percent = 0.4F },
                                                                   new SegmentSizePair { Size = 256, Percent = 0.3F },
                                                                   new SegmentSizePair { Size = 1024, Percent = 0.2F },
                                                                   new SegmentSizePair { Size = 64, Percent = 0.1F });
            SegmentCacheContainer<int> cacheContainer = new SegmentCacheContainer<int>(85000, policy);
            Random random = new Random();
            byte[] data = new byte[random.Next(100, 10000)];
            data[0] = 1;
            data[data.Length - 1] = 2;
            bool result = cacheContainer.Add(0, data);
            Assert.IsTrue(result);
            cacheContainer.Remove(0);
            byte[] temp = cacheContainer.Get(0);
            Assert.IsNull(temp);
        }

        [Test]
        public void IsExistsTest()
        {
            ISegmentCachePolicy policy = new SegmentCachePolicy(
                                                                   new SegmentSizePair { Size = 2048, Percent = 0.4F },
                                                                   new SegmentSizePair { Size = 256, Percent = 0.3F },
                                                                   new SegmentSizePair { Size = 1024, Percent = 0.2F },
                                                                   new SegmentSizePair { Size = 64, Percent = 0.1F });
            SegmentCacheContainer<int> cacheContainer = new SegmentCacheContainer<int>(85000, policy);
            Random random = new Random();
            byte[] data = new byte[random.Next(100, 10000)];
            data[0] = 1;
            data[data.Length - 1] = 2;
            Assert.IsFalse(cacheContainer.IsExists(0));
            bool result = cacheContainer.Add(0, data);
            Assert.IsTrue(result);
            Assert.IsTrue(cacheContainer.IsExists(0));
            cacheContainer.Remove(0);
            byte[] temp = cacheContainer.Get(0);
            Assert.IsNull(temp);
            Assert.IsFalse(cacheContainer.IsExists(0));
        }
    }
}