using KJFramework.Cache.Containers;
using KJFramework.Cache.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KJFramework.Cache.UnitTest
{
    [TestClass]
    public class MemoryChunkCacheContainerTest
    {
        [Test]
        public void InitializeTest()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024*10);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10);
        }

        [Test]
        public void InitializeTest1()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024 * 10 + 5);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10 + 5);
        }

        [Test]
        public void InitializeTest2()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 85000);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 85000);
        }

        [Test]
        public void RentTest()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024 * 10);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10);
            for (int i = 0; i < 10; i++) Assert.IsNotNull(container.Rent());
            Assert.IsNull(container.Rent());
        }

        [Test]
        public void GivebackTest()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024 * 10);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10);
            for (int i = 0; i < 10; i++)
            {
                IMemorySegment memorySegment = container.Rent();
                Assert.IsNotNull(memorySegment);
                container.Giveback(memorySegment);
            }
            Assert.IsNotNull(container.Rent());
        }

        [Test]
        public void UsedOffsetTest()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024 * 10);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10);
            IMemorySegment segment = container.Rent();
            Assert.IsNotNull(segment);
            Assert.IsTrue(segment.UsedBytes == 0);
            Assert.IsTrue(segment.UsedOffset == segment.Segment.Offset);
            segment.UsedOffset = 10;
            Assert.IsTrue(segment.UsedBytes == 10);
            Assert.IsTrue(segment.UsedOffset == segment.Segment.Offset + 10);
        }
    }
}