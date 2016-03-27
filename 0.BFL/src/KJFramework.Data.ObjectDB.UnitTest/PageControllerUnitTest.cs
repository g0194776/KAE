using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.Data.ObjectDB.Controllers;
using KJFramework.Data.ObjectDB.Helpers;
using KJFramework.Data.ObjectDB.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Data.ObjectDB.UnitTest
{
    [TestClass]
    public class PageControllerUnitTest
    {
        #region Methods

        [TestMethod]
        public void CreateTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);


            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void CreatePageTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);

            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void CreateMultiPageTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page); 
            page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize*2);
            Assert.IsTrue(indexTable.UsedPageCounts == 2);

            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }


        [TestMethod]
        public void ReadPageTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            //store index-table updates.
            DBFileHelper.StoreIndexTable(allocator, indexTable);
            stream.Close();
            //read data from exist file.
            stream = new FileStream(file, FileMode.OpenOrCreate);
            allocator = new FileMemoryAllocator(stream);
            indexTable = (IndexTable)DBFileHelper.ReadIndexTable(allocator);
            pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            page = pageController.GetPageById(1U);
            Assert.IsNotNull(page);
            Assert.IsTrue(page.Id == 1U);



            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void ReadMultiPageTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize * 2);
            Assert.IsTrue(indexTable.UsedPageCounts == 2);
            //store index-table updates.
            DBFileHelper.StoreIndexTable(allocator, indexTable);
            stream.Close();
            //read data from exist file.
            stream = new FileStream(file, FileMode.OpenOrCreate);
            allocator = new FileMemoryAllocator(stream);
            indexTable = (IndexTable)DBFileHelper.ReadIndexTable(allocator);
            pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            Assert.IsTrue(indexTable.UsedPageCounts == 2);
            page = pageController.GetPageById(1U);
            Assert.IsNotNull(page);
            Assert.IsTrue(page.Id == 1U);
            page = pageController.GetPageById(2U);
            Assert.IsNotNull(page);
            Assert.IsTrue(page.Id == 2U);



            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void EnsureDataSizeTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[669];
            StorePosition position;
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);

            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void WriteDataTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[669];
            StorePosition position;
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(page.Store(data, position));


            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void CrossSegmentWriteDataTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[3700];
            StorePosition position;
            //1th.
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(page.Store(data, position));
            //2th.
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 1);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(page.Store(data, position));
            Assert.IsTrue(page.UsedSegmentsCount == 2);


            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }


        [TestMethod]
        public void CrossPageWriteDataTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[3700];
            StorePosition position;
            for (int i = 0; i < Global.SegmentsPerPage; i++)
            {
                //1~8th.
                Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
                Assert.IsTrue(position.FileId == 0);
                Assert.IsTrue(position.PageId == 1);
                Assert.IsTrue(position.SegmentId == i);
                Assert.IsTrue(position.StartOffset == 0);
                //try to write data.
                Assert.IsTrue(page.Store(data, position));
            }
            Assert.IsTrue(page.UsedSegmentsCount == Global.SegmentsPerPage);
            //9th.
            Assert.IsFalse(page.EnsureSize((uint)data.Length, out position));
            IPage secPage = pageController.CreatePage();
            Assert.IsNotNull(secPage);
            Assert.IsTrue(secPage.Id == 2U);
            Assert.IsTrue(secPage.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 2);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(secPage.Store(data, position));
            Assert.IsTrue(secPage.UsedSegmentsCount == 1);


            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }


        [TestMethod]
        public void ReadDataTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[669];
            data[0] = 0x0A;
            data[data.Length - 1] = 0x0B;
            StorePosition position;
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(page.Store(data, position));
            //store index-table updates.
            DBFileHelper.StoreIndexTable(allocator, indexTable);
            stream.Close();
            //read data from exist file.
            stream = new FileStream(file, FileMode.OpenOrCreate);
            allocator = new FileMemoryAllocator(stream);
            indexTable = (IndexTable)DBFileHelper.ReadIndexTable(allocator);
            pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            page = pageController.GetPageById(1U);
            Assert.IsNotNull(page);
            Assert.IsTrue(page.Id == 1U);
            Assert.IsTrue(page.UsedSegmentsCount == 1);
            IList<byte[]> actualData = page.GetAllData();
            Assert.IsNotNull(actualData);
            Assert.IsTrue(actualData.Count == 1);
            Assert.IsTrue(actualData[0].Length == data.Length);
            Assert.IsTrue(actualData[0][0] == 0x0A);
            Assert.IsTrue(actualData[0][actualData[0].Length - 1] == 0x0B);




            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }


        [TestMethod]
        public void CrossSegmentReadDataTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[3700];
            StorePosition position;
            //1th.
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(page.Store(data, position));
            //2th.
            Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 1);
            Assert.IsTrue(position.SegmentId == 1);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(page.Store(data, position));
            Assert.IsTrue(page.UsedSegmentsCount == 2);
            //store index-table updates.
            DBFileHelper.StoreIndexTable(allocator, indexTable);
            stream.Close();



            //read data from exist file.
            stream = new FileStream(file, FileMode.OpenOrCreate);
            allocator = new FileMemoryAllocator(stream);
            indexTable = (IndexTable)DBFileHelper.ReadIndexTable(allocator);
            pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            page = pageController.GetPageById(1U);
            Assert.IsNotNull(page);
            Assert.IsTrue(page.Id == 1U);
            Assert.IsTrue(page.UsedSegmentsCount == 2);
            IList<byte[]> actualData = page.GetAllData();
            Assert.IsNotNull(actualData);
            Assert.IsTrue(actualData.Count == 2);
            Assert.IsTrue(actualData[0].Length == data.Length);
            Assert.IsTrue(actualData[1].Length == data.Length);




            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }


        [TestMethod]
        public void CrossPageReadDataTest()
        {
            string file = string.Format("E:\\{0}.db4o", DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            Console.WriteLine("Test file path: " + file);
            if (File.Exists(file)) File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.OpenOrCreate);
            IndexTable indexTable = DBFileHelper.CreateNew(stream);
            IFileMemoryAllocator allocator = new FileMemoryAllocator(stream);
            PageController pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            IPage page = pageController.CreatePage();
            Assert.IsNotNull(page);
            Assert.IsTrue(page.UsedSegmentsCount == 0);
            Assert.IsTrue(stream.Length == Global.HeaderBoundary + Global.ServerPageSize);
            Assert.IsTrue(indexTable.UsedPageCounts == 1);
            byte[] data = new byte[3700];
            StorePosition position;
            for (int i = 0; i < Global.SegmentsPerPage; i++)
            {
                //1~8th.
                Assert.IsTrue(page.EnsureSize((uint)data.Length, out position));
                Assert.IsTrue(position.FileId == 0);
                Assert.IsTrue(position.PageId == 1);
                Assert.IsTrue(position.SegmentId == i);
                Assert.IsTrue(position.StartOffset == 0);
                //try to write data.
                Assert.IsTrue(page.Store(data, position));
            }
            Assert.IsTrue(page.UsedSegmentsCount == Global.SegmentsPerPage);
            //9th.
            Assert.IsFalse(page.EnsureSize((uint)data.Length, out position));
            IPage secPage = pageController.CreatePage();
            Assert.IsNotNull(secPage);
            Assert.IsTrue(secPage.Id == 2U);
            Assert.IsTrue(secPage.EnsureSize((uint)data.Length, out position));
            Assert.IsTrue(position.FileId == 0);
            Assert.IsTrue(position.PageId == 2);
            Assert.IsTrue(position.SegmentId == 0);
            Assert.IsTrue(position.StartOffset == 0);
            //try to write data.
            Assert.IsTrue(secPage.Store(data, position));
            Assert.IsTrue(secPage.UsedSegmentsCount == 1);
            //store index-table updates.
            DBFileHelper.StoreIndexTable(allocator, indexTable);
            stream.Close();



            //read data from exist file.
            stream = new FileStream(file, FileMode.OpenOrCreate);
            allocator = new FileMemoryAllocator(stream);
            indexTable = (IndexTable)DBFileHelper.ReadIndexTable(allocator);
            pageController = new PageController(0, allocator, indexTable);
            Assert.IsNotNull(pageController);
            Assert.IsTrue(indexTable.UsedPageCounts == 2);
            page = pageController.GetPageById(1U);
            Assert.IsNotNull(page);
            Assert.IsTrue(page.Id == 1U);
            Assert.IsTrue(page.UsedSegmentsCount == Global.SegmentsPerPage);
            IList<byte[]> actualData = page.GetAllData();
            Assert.IsNotNull(actualData);
            Assert.IsTrue(actualData.Count == Global.SegmentsPerPage);
            for (int i = 0; i < Global.SegmentsPerPage; i++)
                Assert.IsTrue(actualData[i].Length == data.Length);
            secPage = pageController.GetPageById(2U);
            Assert.IsNotNull(secPage);
            Assert.IsTrue(secPage.Id == 2U);
            Assert.IsTrue(secPage.UsedSegmentsCount == 1);
            actualData = secPage.GetAllData();
            Assert.IsNotNull(actualData);
            Assert.IsTrue(actualData.Count == 1);
            Assert.IsTrue(actualData[0].Length == data.Length);



            //clean resources.
            stream.Close();
            if (File.Exists(file)) File.Delete(file);
        }

        #endregion
    }
}