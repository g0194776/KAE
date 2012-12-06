using KJFramework.Cache.Containers;
using KJFramework.Cache.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Cache.UnitTest
{
    [TestClass]
    public class MemoryChunkCacheContainerTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024*10);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10);
        }

        [TestMethod]
        public void InitializeTest1()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024 * 10 + 5);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10 + 5);
        }

        [TestMethod]
        public void InitializeTest2()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 85000);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 85000);
        }

        [TestMethod]
        public void RentTest()
        {
            //initialize 1 mbytes.
            IMemoryChunkCacheContainer container = new MemoryChunkCacheContainer(1024, 1024 * 10);
            Assert.IsTrue(container.SegmentSize == 1024);
            Assert.IsTrue(container.MemoryChunkSize == 1024 * 10);
            for (int i = 0; i < 10; i++) Assert.IsNotNull(container.Rent());
            Assert.IsNull(container.Rent());
        }

        [TestMethod]
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

        [TestMethod]
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