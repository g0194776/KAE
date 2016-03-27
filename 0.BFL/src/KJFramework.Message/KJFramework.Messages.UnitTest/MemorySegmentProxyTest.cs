using System;
using System.Net;
using System.Text;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Messages.UnitTest
{
    [TestClass]
    public unsafe class MemorySegmentProxyTest
    {
        #region Methods

        [TestMethod]
        [Description("内存片段代理器初始化测试")]
        public void InitializeTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
        }

        [TestMethod]
        [Description("内存片段代理器跳过指定字节长度测试")]
        public void SkipTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.Skip(8);
            Assert.IsTrue(proxy.SegmentCount == 1);
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == 8);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段跳过指定字节长度测试")]
        public void MultiSkipTest()
        {            
            /*
             *  segments:
             *  x - skip bytes
             *  □ - un-use byte.
             *      1: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      2: xxxx□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.Skip(MemoryAllotter.SegmentSize + 4);
            Assert.IsTrue(proxy.SegmentCount == 2);
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 1);
            Assert.IsTrue(position.SegmentOffset == 4);
        }

        [TestMethod]
        [Description("内存片段代理器回写int值测试")]
        public void WriteBackInt32Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == 0);
            proxy.Skip(4);
            Assert.IsTrue(proxy.SegmentCount == 1);
            proxy.WriteInt32(10);
            Assert.IsTrue(proxy.SegmentCount == 1);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 0);
            Assert.IsTrue(position1.SegmentOffset == 8);
            proxy.WriteBackInt32(position, 5);
            Assert.IsTrue(proxy.SegmentCount == 1);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 8);
            Assert.IsTrue(BitConverter.ToInt32(bytes, 0) == 5);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段回写int值测试")]
        public void MultiWriteBackInt32Test()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte.
             *                  ↓ write back position  
             *      1: yyyyyxxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      2: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      3: yyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString("kevin");
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == 5);
            proxy.Skip(4);
            proxy.WriteString(new string('*', (int) (MemoryAllotter.SegmentSize + (MemoryAllotter.SegmentSize - 9) + 4)));
            Assert.IsTrue(proxy.SegmentCount == 3);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 2);
            Assert.IsTrue(position1.SegmentOffset == 4);
            proxy.WriteBackInt32(position, 5);
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize*2 + 4);
            Assert.IsTrue(BitConverter.ToInt32(bytes, 5) == 5);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段回写int值测试")]
        public void MultiWriteBackInt32Test1()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte.
             *                                                       ↓ write back position  
             *      1: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyx
             *      2: xxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      3: yyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize - 1)));
            Assert.IsTrue(proxy.SegmentCount == 1);
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == MemoryAllotter.SegmentSize - 1);
            proxy.Skip(4);
            Assert.IsTrue(proxy.SegmentCount == 2);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize + 1)));
            Assert.IsTrue(proxy.SegmentCount == 3);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 2);
            Assert.IsTrue(position1.SegmentOffset == 4);
            proxy.WriteBackInt32(position, 5);
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize * 2 + 4);
            Assert.IsTrue(bytes[position.SegmentIndex * MemoryAllotter.SegmentSize + position.SegmentOffset] == 5);
            Assert.IsTrue(bytes[position.SegmentIndex * MemoryAllotter.SegmentSize + position.SegmentOffset + 1] == 0);
            Assert.IsTrue(bytes[position.SegmentIndex * MemoryAllotter.SegmentSize + position.SegmentOffset + 2] == 0);
            Assert.IsTrue(bytes[position.SegmentIndex * MemoryAllotter.SegmentSize + position.SegmentOffset + 3] == 0);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段回写int值测试")]
        public void MultiWriteBackInt32Test2()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte.       
             *      1: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *          ↓ write back position  
             *      2: xxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      3: yyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize)));
            Assert.IsTrue(proxy.SegmentCount == 1);
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == MemoryAllotter.SegmentSize);
            proxy.Skip(4);
            Assert.IsTrue(proxy.SegmentCount == 2);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize)));
            Assert.IsTrue(proxy.SegmentCount == 3);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 2);
            Assert.IsTrue(position1.SegmentOffset == 4);
            proxy.WriteBackInt32(position, 5);
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize * 2 + 4);
            Assert.IsTrue(bytes[(position.SegmentIndex + 1) * MemoryAllotter.SegmentSize] == 5);
            Assert.IsTrue(bytes[(position.SegmentIndex + 1) * MemoryAllotter.SegmentSize + 1] == 0);
            Assert.IsTrue(bytes[(position.SegmentIndex + 1) * MemoryAllotter.SegmentSize + 2] == 0);
            Assert.IsTrue(bytes[(position.SegmentIndex + 1) * MemoryAllotter.SegmentSize + 3] == 0);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入int值测试")]
        public void WriteInt32Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            int x = 10;
            proxy.WriteInt32(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 4);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入short测试")]
        public void WriteInt16Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            short x = 10;
            proxy.WriteInt16(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 2);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入long测试")]
        public void WriteInt64Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            long x = 10;
            proxy.WriteInt64(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 8);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入uint值测试")]
        public void WriteUInt32Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            uint x = 10;
            proxy.WriteUInt32(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 4);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入ushort测试")]
        public void WriteUInt16Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            ushort x = 10;
            proxy.WriteUInt16(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 2);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入ulong测试")]
        public void WriteUInt64Test()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            ulong x = 10;
            proxy.WriteUInt64(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 8);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入float测试")]
        public void WriteFloatTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            float x = 10F;
            proxy.WriteFloat(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 4);
            Assert.IsTrue(BitConverter.ToSingle(bytes, 0) == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入double测试")]
        public void WriteDoubleTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            double x = 10.6;
            proxy.WriteDouble(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 8);
            Assert.IsTrue(BitConverter.ToDouble(bytes, 0) == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入byte测试")]
        public void WriteByteTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            byte x = 0x0a;
            proxy.WriteByte(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 1);
            Assert.IsTrue(bytes[0] == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入sbyte测试")]
        public void WriteSByteTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            sbyte x = 0x0a;
            proxy.WriteSByte(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 1);
            Assert.IsTrue(bytes[0] == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入decimal测试")]
        public void WriteDecimalTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            decimal x = 0x0a;
            proxy.WriteDecimal(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 16);
            fixed (byte* pByte = bytes) Assert.IsTrue(*(decimal*)pByte == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入bool测试")]
        public void WriteBooleanTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            bool x = true;
            proxy.WriteBoolean(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 1);
            fixed (byte* pByte = bytes)
            Assert.IsTrue(*((bool*)pByte) == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入string测试")]
        public void WriteStringTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            string x = "Wow! what's pretty feather!";
            proxy.WriteString(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == Encoding.UTF8.GetByteCount(x));
            Assert.IsTrue(Encoding.UTF8.GetString(bytes) == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入guid测试")]
        public void WriteGuidTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            Guid x = Guid.NewGuid();
            proxy.WriteGuid(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 16);
            fixed (byte* pByte = bytes) Assert.IsTrue(*(Guid*)pByte == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入datetime测试")]
        public void WriteDateTimeTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            DateTime x = DateTime.Now;
            proxy.WriteDateTime(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 8);
            fixed (byte* pByte = bytes) Assert.IsTrue(new DateTime(*(long*)pByte) == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入timespan测试")]
        public void WriteTimeSpanTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            TimeSpan x = new TimeSpan(0, 1, 0);
            proxy.WriteTimeSpan(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 8);
            fixed (byte* pByte = bytes) Assert.IsTrue(new TimeSpan(*(long*)pByte) == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入ipendpoint测试")]
        public void WriteIPEndPointTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            IPEndPoint x = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 8888);
            proxy.WriteIPEndPoint(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 12);
            IPEndPointIntellectTypeProcessor processor = new IPEndPointIntellectTypeProcessor();
            IPEndPoint newObj = (IPEndPoint) processor.Process(IntellectTypeProcessorMapping.DefaultAttribute, bytes);
            Assert.IsTrue(newObj.Equals(x));
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入intptr测试")]
        public void WriteIntPtrTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            IntPtr x = new IntPtr(10);
            proxy.WriteIntPtr(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 4);
            Assert.IsTrue(BitConverter.ToInt32(bytes, 0) == x.ToInt32());
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入char测试")]
        public void WriteCharTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            char x = 'c';
            proxy.WriteChar(x);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == 2);
            Assert.IsTrue(bytes[0] == x);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("内存片段代理器写入memory-block测试")]
        public void WriteMemoryTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            int[] x = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            fixed (int* pArr = x)
                proxy.WriteMemory(pArr, (uint)(x.Length * 4));
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == x.Length*4);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
        }

        [TestMethod]
        [Description("多内存片段写入测试")]
        public void MultiSegmentWriteTest()
        {
            /*
             *  segments:
             *  x - string1 byte
             *  y - string2 byte
             *  □ - un-use byte.
             *      1: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      2: yyyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            string content1 = new string('*', (int) MemoryAllotter.SegmentSize);
            proxy.WriteString(content1);
            Assert.IsTrue(proxy.SegmentCount == 1);
            proxy.WriteString("kevin");
            Assert.IsTrue(proxy.SegmentCount == 2);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == MemoryAllotter.SegmentSize + "kevin".Length);
            string str = Encoding.UTF8.GetString(data);
            Console.WriteLine("result: \r\n     " + str);
            Assert.IsTrue(str.Substring((int) MemoryAllotter.SegmentSize) == "kevin");
        }

        [TestMethod]
        [Description("多内存片段写入测试(含有int做结尾值)")]
        public void MultiSegmentWriteTest1()
        {
            /*
             *  segments:
             *  y - int byte
             *  x - string byte
             *  □ - un-use byte.
             *      1: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxyy
             *      2: yy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            string content1 = new string('*', (int)MemoryAllotter.SegmentSize- 2);
            proxy.WriteString(content1);
            Assert.IsTrue(proxy.SegmentCount == 1);
            int intValue = 10;
            proxy.WriteInt32(intValue);
            Assert.IsTrue(proxy.SegmentCount == 2);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == MemoryAllotter.SegmentSize + 2);
            string str = Encoding.UTF8.GetString(data, 0, (int) (MemoryAllotter.SegmentSize - 2));
            Console.WriteLine("result: \r\n     " + str);
            int newInt = BitConverter.ToInt32(data, (int) (MemoryAllotter.SegmentSize - 2));
            Console.WriteLine("result: \r\n     " + newInt);
            Assert.IsTrue(newInt == intValue);
        }

        [TestMethod]
        [Description("多内存片段写入测试(含有memory-block做结尾值)")]
        public void MultiSegmentWriteTest2()
        {
            /*
             *  segments:
             *  x - memory block bytes
             *  □ - un-use byte.
             *      1: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      2: xxxx□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            int elements = (int) (MemoryAllotter.SegmentSize/4 + 1);
            int[] content1 = new int[elements];
            for (int i = 0; i < elements; i++) content1[i] = i;
            fixed (int* pArr = content1) proxy.WriteMemory(pArr, (uint) (elements*4));
            Assert.IsTrue(proxy.SegmentCount == 2);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == elements * 4);
            IntellectObjectEngineUnitTest.PrintBytes(data);
        }

        [TestMethod]
        [Description("多内存片段写入测试(含有memory-block做结尾值)")]
        public void MultiSegmentWriteTest3()
        {
            /*
             *  segments:
             *  x - memory block bytes
             *  □ - un-use byte.
             *      1: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      2: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      3: xxxx□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            int elements = (int)((MemoryAllotter.SegmentSize / 4)*2 + 1);
            int[] content1 = new int[elements];
            for (int i = 0; i < elements; i++) content1[i] = i;
            fixed (int* pArr = content1) proxy.WriteMemory(pArr, (uint)(elements * 4));
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == elements * 4);
            IntellectObjectEngineUnitTest.PrintBytes(data);
        }

        [TestMethod]
        [Description("多内存片段写入，并回收内部资源测试")]
        public void RecoverTest()
        {
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            string content1 = new string('*', (int)MemoryAllotter.SegmentSize);
            proxy.WriteString(content1);
            Assert.IsTrue(proxy.SegmentCount == 1);
            proxy.WriteString("kevin");
            Assert.IsTrue(proxy.SegmentCount == 2);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(proxy.SegmentCount == 0);
            Assert.IsTrue(data.Length == MemoryAllotter.SegmentSize + "kevin".Length);
            string str = Encoding.UTF8.GetString(data);
            Console.WriteLine("result: \r\n     " + str);
            Assert.IsTrue(str.Substring((int)MemoryAllotter.SegmentSize) == "kevin");
        }

        [TestMethod]
        [Description("获取内存位置标记测试")]
        public void GetPositionTest()
        {
            /*
             *  segments:
             *  x - memory block bytes
             *  □ - un-use byte.
             *      1: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      2: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      3: xxxx□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            int elements = (int)((MemoryAllotter.SegmentSize / 4) * 2 + 1);
            int[] content1 = new int[elements];
            for (int i = 0; i < elements; i++) content1[i] = i;
            fixed (int* pArr = content1) proxy.WriteMemory(pArr, (uint)(elements * 4));
            Assert.IsTrue(proxy.SegmentCount == 3);
            MemoryPosition position = proxy.GetPosition();
            //base on zero.
            Assert.IsTrue(position.SegmentIndex == 2);
            Assert.IsTrue(position.SegmentOffset == 4);
            byte[] data = proxy.GetBytes();
            Assert.IsTrue(proxy.SegmentCount == 0);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == elements * 4);
            position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == 0);
            IntellectObjectEngineUnitTest.PrintBytes(data);
        }

        #endregion
    }
}