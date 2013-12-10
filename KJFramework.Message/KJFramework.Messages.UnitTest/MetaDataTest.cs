using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Structs;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Messages.ValueStored.Helper;
using KJFramework.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.UnitTest
{
    [TestClass]
    public class MetaDataTest
    {
        #region Members

        private MetadataContainer _metadataObject;

        #endregion

        #region Method

        [TestInitialize]
        public void Initilize()
        {
            _metadataObject = new MetadataContainer();
        }

        [TestMethod]
        [Description("测试int16类型")]
        public void GetInt16Test()
        {
            short test = short.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new Int16ValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 13);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short>() == short.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试uint16类型")]
        public void GetUInt16Test()
        {
            ushort test = ushort.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new UInt16ValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 13);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort>() == ushort.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试int32类型")]
        public void GetInt32Test()
        {
            int test = int.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new Int32ValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 15);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int>() == int.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试uint32类型")]
        public void GetUInt32Test()
        {
            uint test = uint.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new UInt32ValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 15);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint>() == uint.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试int64类型")]
        public void GetInt64Test()
        {
            long test = long.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new Int64ValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 19);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long>() == long.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试uint64类型")]
        public void GetUInt64Test()
        {
            ulong test = ulong.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new UInt64ValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 19);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong>() == ulong.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试float类型")]
        public void GetFloatTest()
        {
            float test = 12.01f;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new FloatValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 15);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(Math.Abs(metadataObject2.GetAttribute(1).GetValue<float>() - 12.01f) < float.Epsilon);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试double类型")]
        public void GetDoubleTest()
        {
            double test = 12.022221;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new DoubleValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 19);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(Math.Abs(metadataObject2.GetAttribute(1).GetValue<double>() - 12.022221) < double.Epsilon);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Boolean类型")]
        public void GetBooleanTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new BooleanValueStored(true));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 12);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool>());
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Byte类型")]
        public void GetByteTest()
        {
            byte test = byte.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new ByteValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 12);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte>() == byte.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Char类型")]
        public void GetCharTest()
        {
            char test = char.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new CharValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 13);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char>() == char.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Decimal类型")]
        public void GetDecimalTest()
        {
            decimal test = decimal.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new DecimalValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 27);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<decimal>() == decimal.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试SByte类型")]
        public void GetSByteTest()
        {
            sbyte test = sbyte.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new SByteValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 12);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte>() == sbyte.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DateTime类型")]
        public void GetDateTimeTest()
        {
            DateTime test = DateTime.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new DateTimeValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 19);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<DateTime>() == DateTime.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Guid类型")]
        public void GetGuidTest()
        {
            Guid test = Guid.NewGuid();
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new GuidValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 27);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<Guid>() == test);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IPEndPoint类型")]
        public void GetIPEndPointTest()
        {
            IPEndPoint test = new IPEndPoint(IPAddress.Broadcast,IPEndPoint.MaxPort);
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new IPEndPointValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 23);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IPEndPoint>(), test));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IntPtr类型")]
        public void GetIntPtrTest()
        {
            IntPtr test = new IntPtr(IntPtr.Size);
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new IntPtrValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 15);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IntPtr>(), test));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试TimeSpan类型")]
        public void GetTimeSpanTest()
        {
            TimeSpan test = TimeSpan.MaxValue;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new TimeSpanValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 19);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<TimeSpan>(), test));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试BitFlag类型")]
        public void GetBitFlagTest()
        {
            BitFlag test = new BitFlag(0x12);
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new BitFlagValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 12);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<BitFlag>().GetData() == test.GetData());
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Blob类型")]
        public void GetBlobTest()
        {
            byte[] data = {0x01,0x02,0x03,0x04,0x21,0x22,0x31,0x32,0x41,0x42};
            Blob test = new Blob(CompressionTypes.GZip, data);
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new BlobValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            //Assert.IsTrue(bindData.Length == (data.Length + 11));
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            for (int i = 0; i < data.Length; i++)
                Assert.IsTrue(data[i] == metadataObject2.GetAttribute(1).GetValue<Blob>().Decompress()[i]);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试string类型")]
        public void GetStringTest()
        {
            string test = "Test";
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new StringValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 15);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint) bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string>() == test);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试string类型")]
        public void GetStringIsNullTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new StringValueStored(null));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 11);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string>() == null);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试string类型")]
        public void GetStringIsEmptyTest()
        {
            string test = string.Empty;
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new StringValueStored(test));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsNotNull(bindData);
            Assert.IsTrue(bindData.Length == 11);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string>() == null);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试CharArray类型")]
        public void GetCharArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            char[] array = new char[] { 'a','b','c','d', '1','&','@'};
            metadataObject1.SetAttribute(1, new CharArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1));
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[0] == 'a');
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[1] == 'b');
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[2] == 'c');
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[3] == 'd');
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[4] == '1');
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[5] == '&');
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>()[6] == '@');
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试BooleanArray类型")]
        public void GetBooleanArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            bool[] array = new bool[] { true, false, true, true, false, false, true };
            metadataObject1.SetAttribute(1, new BooleanArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<bool[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[0]);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[1] == false);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[2]);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[3]);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[4] == false);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[5] == false);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>()[6]);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试ByteArray类型")]
        public void GetByteArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            byte[] array = new byte[] { 0x01,0x02,0x03,0x04,0xAA,byte.MaxValue, byte.MinValue };
            metadataObject1.SetAttribute(1, new ByteArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<byte[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[0] == 0x01);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[1] == 0x02);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[2] == 0x03);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[3] == 0x04);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[4] == 0xAA);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[5] == byte.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>()[6] == byte.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试SByteArray类型")]
        public void GetSByteArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            sbyte[] array = new sbyte[] { 0x01, 0x02, 0x03, 0x04, sbyte.MinValue, sbyte.MaxValue};
            metadataObject1.SetAttribute(1, new SByteArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<sbyte[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>()[0] == 0x01);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>()[1] == 0x02);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>()[2] == 0x03);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>()[3] == 0x04);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>()[4] == sbyte.MinValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>()[5] == sbyte.MaxValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DecimalArray类型")]
        public void GetDecimalArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            decimal[] array = new decimal[] { decimal.MaxValue,decimal.MinValue };
            metadataObject1.SetAttribute(1, new DecimalArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<decimal[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<decimal[]>()[0] == decimal.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<decimal[]>()[1] == decimal.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DoubleArray类型")]
        public void GetDoubleArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            double[] array = new double[] { double.MaxValue, double.MinValue };
            metadataObject1.SetAttribute(1, new DoubleArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<double[]>());
            Assert.IsTrue(Math.Abs(metadataObject2.GetAttribute(1).GetValue<double[]>()[0] - double.MaxValue) < double.Epsilon);
            Assert.IsTrue(Math.Abs(metadataObject2.GetAttribute(1).GetValue<double[]>()[1] - double.MinValue) < double.Epsilon);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试FloatArray类型")]
        public void GetFloatArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            float[] array = new float[] { float.MaxValue, float.MinValue };
            metadataObject1.SetAttribute(1, new FloatArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<float[]>());
            Assert.IsTrue(Math.Abs(metadataObject2.GetAttribute(1).GetValue<float[]>()[0] - float.MaxValue) < float.Epsilon);
            Assert.IsTrue(Math.Abs(metadataObject2.GetAttribute(1).GetValue<float[]>()[1] - float.MinValue) < float.Epsilon);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int32Array类型")]
        public void GetInt32ArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            int[] array = new int[] { int.MaxValue, int.MinValue};
            metadataObject1.SetAttribute(1, new Int32ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<int[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int[]>()[0] == int.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int[]>()[1] == int.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int32Array大于10个元素类型")]
        public void GetInt32ArrayMoreThanTenTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            int[] array = new int[] { int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue, int.MaxValue, int.MinValue };
            metadataObject1.SetAttribute(1, new Int32ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<int[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int[]>()[0] == int.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int[]>()[array.Length-1] == int.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt32Array类型")]
        public void GetUInt32ArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            uint[] array = new uint[] { uint.MaxValue, uint.MinValue };
            metadataObject1.SetAttribute(1, new UInt32ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<uint[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint[]>()[0] == uint.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint[]>()[1] == uint.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt32Array大于10个元素类型")]
        public void GetUInt32ArrayMoreThanTenTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            uint[] array = new uint[] { uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue, uint.MaxValue, uint.MinValue};
            metadataObject1.SetAttribute(1, new UInt32ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<uint[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint[]>()[0] == uint.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint[]>()[array.Length - 1] == uint.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int16Array大于10个元素类型")]
        public void GetInt16ArrayMoreThanTenTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            short[] array = new short[] { short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue, short.MaxValue, short.MinValue};
            metadataObject1.SetAttribute(1, new Int16ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<short[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short[]>()[0] == short.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short[]>()[array.Length-1] == short.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int16Array类型")]
        public void GetInt16ArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            short[] array = new short[] { short.MaxValue, short.MinValue };
            metadataObject1.SetAttribute(1, new Int16ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<short[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short[]>()[0] == short.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short[]>()[1] == short.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt16Array类型")]
        public void GetUInt16ArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ushort[] array = new ushort[] { ushort.MaxValue, ushort.MinValue };
            metadataObject1.SetAttribute(1, new UInt16ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ushort[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort[]>()[0] == ushort.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort[]>()[1] == ushort.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt16Array大于10个元素类型")]
        public void GetUInt16ArrayMoreThanTenTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ushort[] array = new ushort[] { ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, ushort.MinValue };
            metadataObject1.SetAttribute(1, new UInt16ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ushort[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort[]>()[0] == ushort.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort[]>()[array.Length - 1] == ushort.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int64Array类型")]
        public void GetInt64ArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            long[] array = new long[] { long.MaxValue, long.MinValue };
            metadataObject1.SetAttribute(1, new Int64ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<long[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long[]>()[0] == long.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long[]>()[1] == long.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int64Array大于10个元素类型")]
        public void GetInt64ArrayMoreThanTenTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            long[] array = new long[] { long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue, long.MaxValue, long.MinValue };
            metadataObject1.SetAttribute(1, new Int64ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<long[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long[]>()[0] == long.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long[]>()[array.Length-1] == long.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt64Array类型")]
        public void GetUInt64ArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ulong[] array = new ulong[] { ulong.MaxValue, ulong.MinValue };
            metadataObject1.SetAttribute(1, new UInt64ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ulong[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong[]>()[0] == ulong.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong[]>()[1] == ulong.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt64Array大于10个元素类型")]
        public void GetUInt64ArrayMoreThanTenTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ulong[] array = new ulong[] { ulong.MaxValue, ulong.MinValue, ulong.MaxValue, ulong.MinValue, ulong.MaxValue, ulong.MinValue, ulong.MaxValue, ulong.MinValue, ulong.MaxValue, ulong.MinValue, ulong.MaxValue, ulong.MinValue, ulong.MaxValue, ulong.MinValue,};
            metadataObject1.SetAttribute(1, new UInt64ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ulong[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong[]>()[0] == ulong.MaxValue);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong[]>()[array.Length - 1] == ulong.MinValue);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DateTimeArray类型")]
        public void GetDateTimeArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            DateTime[] array = new DateTime[] {DateTime.MaxValue, DateTime.MinValue};
            metadataObject1.SetAttribute(1, new DateTimeArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<DateTime[]>());
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<DateTime[]>()[0], array[0]));
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<DateTime[]>()[1], array[1]));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试GuidArray类型")]
        public void GetGuidArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Guid[] array = new Guid[] { Guid.NewGuid(),Guid.NewGuid() };
            metadataObject1.SetAttribute(1, new GuidArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<Guid[]>());
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<Guid[]>()[0], array[0]));
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<Guid[]>()[1], array[1]));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IntPtrArray类型")]
        public unsafe void GetIntPtrArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            byte[] test = new byte[]{0x12,0x23};
            int y = 100;
            int* x = &y;
            IntPtr[] array;
            fixed (void* pVoid = test)
            {
                array = new IntPtr[] {new IntPtr(pVoid), new IntPtr(x), new IntPtr(int.MaxValue) };
            }
            metadataObject1.SetAttribute(1, new IntPtrArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<IntPtr[]>());
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IntPtr[]>()[0], array[0]));
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IntPtr[]>()[1], array[1]));
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IntPtr[]>()[2], array[2]));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试TimeSpanArray类型")]
        public void GetTimeSpanArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            TimeSpan[] array = new TimeSpan[] { TimeSpan.MaxValue, TimeSpan.MinValue };
            metadataObject1.SetAttribute(1, new TimeSpanArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<TimeSpan[]>());
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<TimeSpan[]>()[0], array[0]));
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<TimeSpan[]>()[1], array[1]));
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IPEndPointArray类型")]
        public void GetIPEndPointArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            IPEndPoint[] array = new IPEndPoint[] { new IPEndPoint(IPAddress.Broadcast, IPEndPoint.MaxPort), new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort) };
            metadataObject1.SetAttribute(1, new IPEndPointArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<IPEndPoint[]>());
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IPEndPoint[]>()[0], array[0]));
            Assert.IsTrue(Equals(metadataObject2.GetAttribute(1).GetValue<IPEndPoint[]>()[1], array[1]));
            Console.WriteLine(metadataObject2.ToString());
        }


        [TestMethod]
        [Description("测试智能对象类型")]
        public void GetIntellectObjectTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Test1 test1 = new Test1{ServicelId = 1,ProtocolId = 2};         
            metadataObject1.SetAttribute(1, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(test1)));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObject<Test1>((IntellectObjectValueStored)metadataObject2.GetAttribute(1)));
            Test1 test2 =
                IntellectObjectHelper.GetAttributeAsIntellectObject<Test1>(
                    (IntellectObjectValueStored) metadataObject2.GetAttribute(1));
            Assert.IsTrue(test2.ServicelId == 1);
            Assert.IsTrue(test2.ProtocolId == 2);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试string数组类型")]
        public void GetStringArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            string[] array = new string[] { "t", "te", "tes", "test", "", "end123876*^$" };
            metadataObject1.SetAttribute(1, new StringArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<string[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[0] == "t");
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[1] == "te");
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[2] == "tes");
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[3] == "test");
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[4] == null);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[5] == "end123876*^$");
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试string数组类型中存在空的情况")]
        public void GetStringArrayHasNullTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            string[] array = new string[3]; 
            array[0] = "test";
            array[2] = "end123876*^$";
            metadataObject1.SetAttribute(1, new StringArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<string[]>());
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<string[]>()[0]);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<string[]>()[2]);
            Assert.IsNull(metadataObject2.GetAttribute(1).GetValue<string[]>()[1]);
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[0] == "test");
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>()[2] == "end123876*^$");
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IntellectObjectArray数组类型")]
        public void GetIntellectObjectArrayTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Test1[] test = new Test1[2];
            test[0] = new Test1{ProtocolId = 1,ServicelId = 1};
            test[1] = new Test1{ProtocolId = 2,ServicelId = 2};
            metadataObject1.SetAttribute(1, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(test)));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1)));
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0] != null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1] != null);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0].ServicelId == 1);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0].ProtocolId == 1);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1].ServicelId == 2);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1].ProtocolId == 2);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IntellectObjectArray数组中有空对象情况类型")]
        public void GetIntellectObjectArrayHasNullTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Test1[] test = new Test1[4];
            test[0] = new Test1 { ProtocolId = 1, ServicelId = 1 };
            test[1] = new Test1 { ProtocolId = 2, ServicelId = 2 };
            test[3] = new Test1 {ProtocolId = 4, ServicelId = 4};
            metadataObject1.SetAttribute(1, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(test)));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0] != null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1] != null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1] == null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1] != null);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0].ServicelId == 1);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0].ProtocolId == 1);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1].ServicelId == 2);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1].ProtocolId == 2);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[3].ServicelId == 4);
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[3].ProtocolId == 4);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IntellectObjectArray数组中全为空对象情况类型")]
        public void GetIntellectObjectArrayAllNullTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Test1[] test = new Test1[4];
            metadataObject1.SetAttribute(1, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(test)));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsTrue(bindData.Length == 23);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1)));
            Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1)).Length == 4);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[0] == null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[1] == null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[2] == null);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1))[3] == null);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试可空值类型")]
        public void GetNullValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            metadataObject1.SetAttribute(1, new NullValueStored());
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            Assert.IsTrue(bindData.Length == 11);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsNotNull(metadataObject2.GetAttribute(1));
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong?>() == null);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试BooleanArray类型")]
        public void GetBooleanArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            bool[] array = new bool[0];
            metadataObject1.SetAttribute(1, new BooleanArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<bool[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试ByteArray类型")]
        public void GetByteArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            byte[] array = new byte[0];
            metadataObject1.SetAttribute(1, new ByteArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<byte[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试CharArray类型")]
        public void GetCharArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            char[] array = new char[0];
            metadataObject1.SetAttribute(1, new CharArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<char[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DateTimeArray类型")]
        public void GetDateTimeArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            DateTime[] array = new DateTime[0];
            metadataObject1.SetAttribute(1, new DateTimeArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<DateTime[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<DateTime[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DecimalArray类型")]
        public void GetDecimalArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            decimal[] array = new decimal[0];
            metadataObject1.SetAttribute(1, new DecimalArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<decimal[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<decimal[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DoubleArray类型")]
        public void GetDoubleArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            double[] array = new double[0];
            metadataObject1.SetAttribute(1, new DoubleArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<double[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<double[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试FloatArray类型")]
        public void GetFloatArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            float[] array = new float[0];
            metadataObject1.SetAttribute(1, new FloatArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<float[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<float[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试GuidArray类型")]
        public void GetGuidArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Guid[] array = new Guid[0];
            metadataObject1.SetAttribute(1, new GuidArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<bool[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int16Array类型")]
        public void GetInt16ArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            short[] array = new short[0];
            metadataObject1.SetAttribute(1, new Int16ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<short[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int32Array类型")]
        public void GetInt32ArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            int[] array = new int[0];
            metadataObject1.SetAttribute(1, new Int32ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<int[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试Int64Array类型")]
        public void GetInt64ArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            long[] array = new long[0];
            metadataObject1.SetAttribute(1, new Int64ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<long[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt16Array类型")]
        public void GetUInt16ArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ushort[] array = new ushort[0];
            metadataObject1.SetAttribute(1, new UInt16ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ushort[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt32Array类型")]
        public void GetUInt32ArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            uint[] array = new uint[0];
            metadataObject1.SetAttribute(1, new UInt32ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<uint[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试UInt64Array类型")]
        public void GetUInt64ArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ulong[] array = new ulong[0];
            metadataObject1.SetAttribute(1, new UInt64ArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ulong[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IntPtrArray类型")]
        public void GetIntPtrArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            IntPtr[] array = new IntPtr[0];
            metadataObject1.SetAttribute(1, new IntPtrArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<IntPtr[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<IntPtr[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IPEndPointArray类型")]
        public void GetIPEndPointArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            IPEndPoint[] array = new IPEndPoint[0];
            metadataObject1.SetAttribute(1, new IPEndPointArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<IPEndPoint[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<IPEndPoint[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试SByteArray类型")]
        public void GetSByteArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            sbyte[] array = new sbyte[0];
            metadataObject1.SetAttribute(1, new SByteArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<sbyte[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试StringArray类型")]
        public void GetStringArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            string[] array = new string[0];
            metadataObject1.SetAttribute(1, new StringArrayValueStored(array));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 15);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<string[]>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<string[]>().Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试IIntellectObjectArray类型")]
        public void GetIntellectObjectArrayWithLengthZeroTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Test1[] array = new Test1[] { };
            metadataObject1.SetAttribute(1, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(array)));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 15);
            Assert.IsNotNull(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1)) != null);
            //Assert.IsTrue(IntellectObjectHelper.GetAttributeAsIntellectObjectArray<Test1>((IntellectObjectArrayValueStored)metadataObject2.GetAttribute(1)).Length == 0);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetBooleanDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            bool defaultValue = DefaultValue.Boolean;
            metadataObject1.SetAttribute(1, new BooleanValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<bool>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<bool>() == DefaultValue.Boolean);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetByteDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            byte defaultValue = DefaultValue.Byte;
            metadataObject1.SetAttribute(1, new ByteValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<byte>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<byte>() == DefaultValue.Byte);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetCharDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            char defaultValue = DefaultValue.Char;
            metadataObject1.SetAttribute(1, new CharValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<char>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<char>() == DefaultValue.Char);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetDateTimeDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            DateTime defaultValue = DefaultValue.DateTime;
            metadataObject1.SetAttribute(1, new DateTimeValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<DateTime>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<DateTime>() == DefaultValue.DateTime);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetDecimalDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            decimal defaultValue = DefaultValue.Decimal;
            metadataObject1.SetAttribute(1, new DecimalValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<decimal>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<decimal>() == DefaultValue.Decimal);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetDoubleDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            double defaultValue = DefaultValue.Double;
            metadataObject1.SetAttribute(1, new DoubleValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<double>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<double>() == DefaultValue.Double);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetFloatDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            float defaultValue = DefaultValue.Float;
            metadataObject1.SetAttribute(1, new FloatValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<float>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<float>() == DefaultValue.Float);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetGuidDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            Guid defaultValue = DefaultValue.Guid;
            metadataObject1.SetAttribute(1, new GuidValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<Guid>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<Guid>() == DefaultValue.Guid);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetInt16DefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            short defaultValue = DefaultValue.Int16;
            metadataObject1.SetAttribute(1, new Int16ValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<short>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<short>() == DefaultValue.Int16);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetUInt16DefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ushort defaultValue = DefaultValue.UInt16;
            metadataObject1.SetAttribute(1, new UInt16ValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ushort>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ushort>() == DefaultValue.UInt16);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetInt32DefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            int defaultValue = DefaultValue.Int32;
            metadataObject1.SetAttribute(1, new Int32ValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<int>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<int>() == DefaultValue.Int32);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetUInt32DefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            uint defaultValue = DefaultValue.UInt32;
            metadataObject1.SetAttribute(1, new UInt32ValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<uint>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<uint>() == DefaultValue.UInt32);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetInt64DefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            long defaultValue = DefaultValue.Int64;
            metadataObject1.SetAttribute(1, new Int64ValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<long>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<long>() == DefaultValue.Int64);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetUInt64DefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            ulong defaultValue = DefaultValue.UInt64;
            metadataObject1.SetAttribute(1, new UInt64ValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<ulong>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<ulong>() == DefaultValue.UInt64);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetIntPtrDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            IntPtr defaultValue = DefaultValue.IntPtr;
            metadataObject1.SetAttribute(1, new IntPtrValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<IntPtr>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<IntPtr>() == DefaultValue.IntPtr);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetSByteDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            sbyte defaultValue = DefaultValue.SByte;
            metadataObject1.SetAttribute(1, new SByteValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<sbyte>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<sbyte>() == DefaultValue.SByte);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试DefaultValue的存储与解析")]
        public void GetTimeSpanDefaultValueTest()
        {
            MetadataContainer metadataObject1 = new MetadataContainer();
            TimeSpan defaultValue = DefaultValue.TimeSpan;
            metadataObject1.SetAttribute(1, new TimeSpanValueStored(defaultValue));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataObject1);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Assert.IsTrue(bindData.Length == 11);
            Assert.IsNotNull(metadataObject2.GetAttribute(1).GetValue<TimeSpan>());
            Assert.IsTrue(metadataObject2.GetAttribute(1).GetValue<TimeSpan>() == DefaultValue.TimeSpan);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试ResourceBlock类型")]
        public void GetResourceBlockTest()
        {
            ResourceBlock metadata = new ResourceBlock();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] { null, null }));
            TestObject1[] intellectObjects = new TestObject1[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] { "Kevin", null, "Jee" }));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow };
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            MetadataContainer metadataContainer = new MetadataContainer();
            metadataContainer.SetAttribute(1, new ResourceBlockStored(metadata));
            MetadataContainer metadataContainer1 = new MetadataContainer();
            metadataContainer1.SetAttribute(1, new ResourceBlockStored(metadataContainer));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataContainer1);
            Assert.IsNotNull(bindData);
            ResourceBlock metadata2;
            metadata2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Console.WriteLine(metadata2.ToString());
        }

        [TestMethod]
        [Description("测试ResourceBlockArray类型")]
        public void GetResourceBlockArrayTest()
        {
            ResourceBlock metadata = new ResourceBlock();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] { null, null }));
            TestObject1[] intellectObjects = new TestObject1[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] { "Kevin", null, "Jee" }));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow };
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            ResourceBlock[] resourceBlocks = new[]{metadata, metadata, metadata};
            MetadataContainer metadataContainer = new MetadataContainer();
            metadataContainer.SetAttribute(1, new ResourceBlockArrayStored(resourceBlocks));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataContainer);
            Assert.IsNotNull(bindData);
            ResourceBlock metadata2;
            metadata2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Console.WriteLine(metadata2.ToString());
        }

        [TestMethod]
        [Description("测试ResourceBlockArray类型")]
        public void GetResourceBlockArrayHasNullTest()
        {
            ResourceBlock metadata = new ResourceBlock();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] { null, null }));
            TestObject1[] intellectObjects = new TestObject1[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] { "Kevin", null, "Jee" }));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow };
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            ResourceBlock[] resourceBlocks = new[] { metadata, null, metadata };
            MetadataContainer metadataContainer = new MetadataContainer();
            metadataContainer.SetAttribute(1, new ResourceBlockArrayStored(resourceBlocks));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataContainer);
            Assert.IsNotNull(bindData);
            ResourceBlock metadata2;
            metadata2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Console.WriteLine(metadata2.ToString());
        }

        [TestMethod]
        [Description("测试ResourceBlockArray与ResourceBlock混合多重嵌套含空元素类型，这特么都行我特么就不信还有测不过的！！！！！！！！！")]
        public void GetResourceBlockArrayMultipleNestedHasNullTest()
        {
            ResourceBlock metadata = new ResourceBlock();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] { null, null }));
            TestObject1[] intellectObjects = new TestObject1[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] { "Kevin", null, "Jee" }));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow };
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            ResourceBlock[] resourceBlocks = new[] { metadata, null, metadata };
            MetadataContainer metadataContainer = new MetadataContainer();
            metadataContainer.SetAttribute(1, new ResourceBlockArrayStored(resourceBlocks));          
            metadataContainer.SetAttribute(2, new StringValueStored("多重嵌套测试"));
            metadataContainer.SetAttribute(3, new ResourceBlockArrayStored(resourceBlocks));
            MetadataContainer metadataContainer1 = new MetadataContainer();
            metadataContainer1.SetAttribute(1, new ResourceBlockStored(metadataContainer));
            metadataContainer1.SetAttribute(2, new ResourceBlockStored(metadata));
            MetadataContainer metadataContainer2 = new MetadataContainer();
            metadataContainer2.SetAttribute(1, new ResourceBlockStored(metadataContainer1));
            MetadataContainer metadataContainer3 = new MetadataContainer();
            metadataContainer3.SetAttribute(1, new ResourceBlockStored(metadataContainer2));
            metadataContainer3.SetAttribute(2, new ResourceBlockArrayStored(new ResourceBlock[]{metadataContainer2,null,metadataContainer2}));
            MetadataContainer metadataContainer4 = new MetadataContainer();
            metadataContainer4.SetAttribute(1, new ResourceBlockStored(metadataContainer3));
            metadataContainer4.SetAttribute(2, new StringValueStored("多重嵌套测试"));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadataContainer4);
            Assert.IsNotNull(bindData);
            ResourceBlock metadata2;
            metadata2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Console.WriteLine(metadata2.ToString());
        }

        //[TestMethod]
        //public void SetT()
        //{
        //    _metadataObject.SetAttributeAsT<uint>(2, 12);

        //    _metadataObject.GetAttributeAsString(2);
        //    //Console.WriteLine(_metadataObject.GetAttributeAsT(2));
        //}

        [TestMethod]
        [Description("内存片段代理器跨内存段回写void*测试")]
        public unsafe void WriteMemoryTest1()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte.
             *                                                       ↓ write back position  
             *      1: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyx
             *      2: xxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      3: yyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            Dictionary<byte,BaseValueStored> dic = new Dictionary<byte, BaseValueStored>();
            dic.Add(1,new UInt32ValueStored(12));
            MarkRange* markRange = stackalloc MarkRange[dic.Count];
            markRange[0].Id = 3;
            markRange[0].Flag = 5<<8  | (int)PropertyTypes.Int32 ;
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize - 1)));
            Assert.IsTrue(proxy.SegmentCount == 1);
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == MemoryAllotter.SegmentSize - 1);
            MemoryPosition wrapperMarkRangeStartPosition = proxy.GetPosition();   
            proxy.Skip((uint)(5));
            MemoryPosition position0 = proxy.GetPosition();
            Assert.IsTrue(position0.SegmentIndex == 1);
            Assert.IsTrue(position0.SegmentOffset == 4);
            Assert.IsTrue(proxy.SegmentCount == 2);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize)));
            Assert.IsTrue(proxy.SegmentCount == 3);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 2);
            Assert.IsTrue(position1.SegmentOffset == 4);
            proxy.WriteBackMemory(wrapperMarkRangeStartPosition, markRange, 5);
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] bytes = proxy.GetBytes();
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes[255] == 3);
            Assert.IsTrue(bytes[256] == (byte)PropertyTypes.Int32);
            Assert.IsTrue(bytes[257] == 5);
            Assert.IsTrue(bytes[258] == 0);
            Assert.IsTrue(bytes[259] == 0);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize * 2 + 4);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段回写void*值测试")]
        public unsafe void WriteMemoryTest2()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte.
             *                  ↓ write back position  
             *      1: yyyyyxxxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      2: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      3: yyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            Dictionary<byte, BaseValueStored> dic = new Dictionary<byte, BaseValueStored>();
            dic.Add(1, new UInt32ValueStored(12));
            MarkRange* markRange = stackalloc MarkRange[dic.Count];
            markRange[0].Id = 3;
            markRange[0].Flag = (5 << 8) | (int)PropertyTypes.Int32;
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString("testA");
            MemoryPosition position = proxy.GetPosition();
            Assert.IsTrue(position.SegmentIndex == 0);
            Assert.IsTrue(position.SegmentOffset == 5);
            proxy.Skip(5);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize + (MemoryAllotter.SegmentSize - 10) + 4)));
            Assert.IsTrue(proxy.SegmentCount == 3);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 2);
            Assert.IsTrue(position1.SegmentOffset == 4);
            proxy.WriteBackMemory(position, markRange, 5);
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize * 2 + 4);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
            Assert.IsTrue(bytes[5] == 3);
            Assert.IsTrue(bytes[6] == (byte)PropertyTypes.Int32);
            Assert.IsTrue(bytes[7] == 5);
            Assert.IsTrue(bytes[8] == 0);
            Assert.IsTrue(bytes[9] == 0);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段回写void*值测试")]
        public unsafe void WriteMemoryTest3()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte. 
             *      1: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *         ↓ write back position 
             *      2: xxxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      3: yyyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            Dictionary<byte, BaseValueStored> dic = new Dictionary<byte, BaseValueStored>();
            dic.Add(1, new UInt32ValueStored(12));
            MarkRange* markRange = stackalloc MarkRange[dic.Count];
            markRange[0].Id = 3;
            markRange[0].Flag = (5 << 8) | (int)PropertyTypes.Int32;
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize)));
            Assert.IsTrue(proxy.SegmentCount == 1);
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 0);
            Assert.IsTrue(position1.SegmentOffset == 256);
            proxy.Skip(5);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize)));
            proxy.WriteBackMemory(position1, markRange, 5);
            Assert.IsTrue(proxy.SegmentCount == 3);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize * 2 + 5);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
            Assert.IsTrue(bytes[256] == 3);
            Assert.IsTrue(bytes[257] == 6);
            Assert.IsTrue(bytes[258] == 5);
            Assert.IsTrue(bytes[259] == 0);
            Assert.IsTrue(bytes[260] == 0);
        }

        [TestMethod]
        [Description("内存片段代理器跨内存段回写void*值测试")]
        public unsafe void WriteMemoryTest4()
        {
            /*
             *  segments:
             *  x - write back bytes.
             *  y - string bytes
             *  □ - un-use byte.
             *      1: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *                       ↓ write back position  
             *      2: yyyyyyyyyyyyyyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      3: xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
             *      4: xxxxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
             *      5: yyyy□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□□
             */
            Dictionary<byte, BaseValueStored> dic = new Dictionary<byte, BaseValueStored>();
            for (int i = 1; i <= 102; i++)
            {
                dic.Add((byte)i, new UInt32ValueStored(12));
            }
            MarkRange* markRange = stackalloc MarkRange[102];
            for (int i = 0; i < 102; i++)
            {
                markRange[i].Id = 3;
                markRange[i].Flag = (5 << 8) | (int)PropertyTypes.Int32;
            }
            MemoryAllotter.Instance.Initialize();
            IMemorySegmentProxy proxy = MemorySegmentProxyFactory.Create();
            Assert.IsTrue(proxy.SegmentCount == 0);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize + 10)));
            MemoryPosition position1 = proxy.GetPosition();
            Assert.IsTrue(position1.SegmentIndex == 1);
            Assert.IsTrue(position1.SegmentOffset == 10);
            proxy.Skip((uint) dic.Count * 5);
            MemoryPosition position2 = proxy.GetPosition();
            Assert.IsTrue(proxy.SegmentCount == 4);
            Assert.IsTrue(position2.SegmentIndex == 3);
            Assert.IsTrue(position2.SegmentOffset == 8);
            proxy.WriteString(new string('*', (int)(MemoryAllotter.SegmentSize)));
            MemoryPosition position3 = proxy.GetPosition();
            Assert.IsTrue(position3.SegmentIndex == 4);
            Assert.IsTrue(position3.SegmentOffset == 8);
            proxy.WriteBackMemory(position1, markRange, 510);
            Assert.IsTrue(proxy.SegmentCount == 5);
            byte[] bytes = proxy.GetBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length == MemoryAllotter.SegmentSize * 4 + 8);
            IntellectObjectEngineUnitTest.PrintBytes(bytes);
            for (int i = 0; i < 102; i++)
            {
                Assert.IsTrue(bytes[266+5*i] == 3);
                Assert.IsTrue(bytes[267 + 5*i] == (byte) PropertyTypes.Int32);
                Assert.IsTrue(bytes[268 + 5 * i] == 5);
                Assert.IsTrue(bytes[269 + 5 * i] == 0);
                Assert.IsTrue(bytes[270 + 5 * i] == 0);
            }
        }

        [TestMethod]
        [Description("测试metadata Object")]
        public void MetaDataObjectTest()
        {
            MetadataContainer metadata = new MetadataContainer();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] {null, null}));
            TestObject1[] intellectObjects = new TestObject1[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] {"Kevin", null, "Jee"}));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 {Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow};
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadata);
            MetadataContainer metadataObject2;
            metadataObject2 = MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            Console.WriteLine(metadataObject2.ToString());
        }

        [TestMethod]
        [Description("测试metadata Object")]
        public void ExtremeBindTest()
        {
            MetadataContainer metadata = new MetadataContainer();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] { null, null }));
            IntellectObject[] intellectObjects = new IntellectObject[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] { "Kevin", null, "Jee" }));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow };
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            CodeTimer.Initialize();
            CodeTimer.Time("Bind object test: 100000", 1, delegate
            {
                for (int i = 0; i < 100000; i++) MetadataObjectEngine.ToBytes(metadata);
            }); 
            CodeTimer.Time("Bind object test: 100000", 1, delegate
            {
                for (int i = 0; i < 100000; i++) MetadataObjectEngine.ToBytes(metadata);
            });
            CodeTimer.Time("Bind object test: 100000", 1, delegate
            {
                for (int i = 0; i < 100000; i++) MetadataObjectEngine.ToBytes(metadata);
            });
        }

        [TestMethod]
        [Description("测试metadata Object")]
        public void ExtremePickupTest()
        {
            MetadataContainer metadata = new MetadataContainer();
            metadata.SetAttribute(1, new StringArrayValueStored(new string[] { null, null }));
            IntellectObject[] intellectObjects = new IntellectObject[] { null, new TestObject1 { Haha = "..." } };
            metadata.SetAttribute(2, new IntellectObjectArrayValueStored(IntellectObjectHelper.BindIntellectObjectArray(intellectObjects)));
            metadata.SetAttribute(3, new StringArrayValueStored(new string[] { "Kevin", null, "Jee" }));
            metadata.SetAttribute(4, new Int32ArrayValueStored(new int[] { 9988, 9999 }));
            metadata.SetAttribute(5, new Int32ValueStored(111111));
            metadata.SetAttribute(6, new Int32ValueStored(222222));
            metadata.SetAttribute(7, new ByteArrayValueStored(Encoding.Default.GetBytes("haha")));
            metadata.SetAttribute(8, new DateTimeValueStored(DateTime.Now));
            IntellectObject intellectObject = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow };
            metadata.SetAttribute(9, new IntellectObjectValueStored(IntellectObjectHelper.BindIntellectObject(intellectObject)));
            metadata.SetAttribute(10, new Int64ValueStored(10));
            metadata.SetAttribute(11, new NullValueStored());
            metadata.SetAttribute(12, new DoubleValueStored(16));
            byte[] bindData = MetadataObjectEngine.ToBytes(metadata);
            CodeTimer.Initialize();
            CodeTimer.Time("Pickup object test: 100000", 1, delegate
            {
                for (int i = 0; i < 100000; i++) MetadataObjectEngine.GetObject(bindData, 0, (uint)bindData.Length);
            });
        }

        #endregion

        public class Test1 : IntellectObject
        {
            [IntellectProperty(0)]
            public int ProtocolId { get; set; }
            [IntellectProperty(1)]
            public int ServicelId { get; set; }
        }

        public class Test2 : IntellectObject
        {
            private TestObject objects = new TestObject
                {
                    Uu = new string[] {null, null},
                    Pp = new[] {null, new TestObject1 {Haha = "..."}},
                    Jj = new[] {"Kevin", null, "Jee"},
                    Mm = new[] {9988, 9999},
                    Wocao = 111111,
                    Wokao = 222222,
                    Metadata1 = 99,
                    Metadata = Encoding.Default.GetBytes("haha"),
                    Time = DateTime.Now,
                    Obj = new TestObject1 {Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow},
                    NullableValue1 = 10,
                    NullableValue3 = 16
                };
        }
    }
}
