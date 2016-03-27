using System;
using System.Net;
using System.Text;
using KJFramework.Messages.Helpers;
using NUnit.Framework;

namespace KJFramework.Messages.UnitTest
{
    [TestFixture]
    public class DataHelperUnitTest
    {
        [Test]
        [Description("准字节数测试")]
        public void NormalBindTest()
        {
            Test1 test1 = new Test1 { ProtocolId = 1, ServicelId = 2 };
            byte[] data = DataHelper.ToBytes<Test1>(test1);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 14);
            PrintBytes(data);
        }

        [Test]
        [Description("准字节数解析测试")]
        public void NormalPickupTest()
        {
            Test1 test1 = new Test1 { ProtocolId = 1, ServicelId = 2 };
            byte[] data = DataHelper.ToBytes<Test1>(test1);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 14);
            PrintBytes(data);

            Test1 newObj = DataHelper.GetObject<Test1>(data);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
        }

        [Test]
        [Description("带有浮动数据类型的标准测试")]
        public void NormalBindTest1()
        {
            Test2 test2 = new Test2 { ProtocolId = 1, ServicelId = 2 };
            byte[] data = DataHelper.ToBytes<Test2>(test2);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 14);
            PrintBytes(data);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "" };
            data = DataHelper.ToBytes<Test2>(test2);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 19);
            PrintBytes(data);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "YangJie" };
            data = DataHelper.ToBytes<Test2>(test2);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 26);
            PrintBytes(data);
        }

        [Test]
        [Description("带有浮动数据类型的标准解析测试")]
        public void NormalPickupTest1()
        {
            Test2 test2 = new Test2 { ProtocolId = 1, ServicelId = 2 };
            byte[] data = DataHelper.ToBytes<Test2>(test2);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 14);
            PrintBytes(data);
            Test2 newObj = DataHelper.GetObject<Test2>(data);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "" };
            data = DataHelper.ToBytes<Test2>(test2);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 19);
            PrintBytes(data);
            newObj = DataHelper.GetObject<Test2>(data);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Name == null);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "YangJie我~" };
            data = DataHelper.ToBytes<Test2>(test2);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 30);
            PrintBytes(data);
            newObj = DataHelper.GetObject<Test2>(data);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Name == "YangJie我~");
        }

        [Test]
        [Description("带有固定数据类型数组的标准测试")]
        public void NormalBindTest2()
        {
            Test3 test3 = new Test3 { ProtocolId = 1, ServicelId = 2 };
            test3.Numbers = new[] { 1111, 2222 };
            byte[] data = DataHelper.ToBytes<Test3>(test3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 36);
            PrintBytes(data);
        }

        [Test]
        [Description("带有固定数据类型数组的标准解析测试")]
        public void NormalPickupTest2()
        {
            Test3 test3 = new Test3 { ProtocolId = 1, ServicelId = 2 };
            test3.Numbers = new[] { 1111, 2222 };
            byte[] data = DataHelper.ToBytes<Test3>(test3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 36);
            PrintBytes(data);

            Test3 newObj = DataHelper.GetObject<Test3>(data);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Numbers);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Numbers.Length == 2);
            Assert.IsTrue(newObj.Numbers[0] == 1111);
            Assert.IsTrue(newObj.Numbers[1] == 2222);
        }

        [Test]
        [Description("带有浮动数据类型数组的标准测试")]
        public void NormalBindTest3()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            byte[] data = DataHelper.ToBytes<Test4>(test4);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 34);
            PrintBytes(data);
        }

        [Test]
        [Description("带有浮动数据类型数组的标准解析测试")]
        public void NormalPickupTest3()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", null, "jie" };
            byte[] data = DataHelper.ToBytes<Test4>(test4);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 36);
            PrintBytes(data);

            Test4 newObj = DataHelper.GetObject<Test4>(data);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Names);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Names.Length == 3);
            Assert.IsTrue(newObj.Names[0] == "yang");
            Assert.IsTrue(newObj.Names[1] == null);
            Assert.IsTrue(newObj.Names[2] == "jie");
        }

        [Test]
        [Description("智能对象数组的序列化测试")]
        public void NormalBindTest4()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            Test4[] array = new[] {test4, test4, test4};
            byte[] data = DataHelper.ToBytes<Test4[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 112);
            PrintBytes(data);
        }

        [Test]
        [Description("智能对象数组的反序列化测试")]
        public void NormalPickupTest4()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            Test4[] array = new[] { test4, test4, test4 };
            byte[] data = DataHelper.ToBytes<Test4[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 112);
            PrintBytes(data);

            Test4[] results = DataHelper.GetObject<Test4[]>(data);
            Assert.IsNotNull(results);
            Assert.IsTrue(array.Length == results.Length);
            for (int i = 0; i < results.Length; i++) results[i] = array[i];
        }

        [Test]
        [Description("智能对象数组的序列化测试")]
        public void NormalBindTest5()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            Test4[] array = new[] { null, test4, null };
            byte[] data = DataHelper.ToBytes<Test4[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 44);
            PrintBytes(data);
        }

        [Test]
        [Description("智能对象数组的反序列化测试")]
        public void NormalPickupTest5()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            Test4[] array = new[] { null, test4, null };
            byte[] data = DataHelper.ToBytes<Test4[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 44);
            PrintBytes(data);

            Test4[] results = DataHelper.GetObject<Test4[]>(data);
            Assert.IsNotNull(results);
            Assert.IsTrue(array.Length == results.Length);
            for (int i = 0; i < results.Length; i++) results[i] = array[i];
        }

        [Test]
        [Description("智能对象数组的序列化测试")]
        public void NormalBindTest6()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            Test4[] array = new[] { (Test4) null, (Test4) null, (Test4) null };
            byte[] data = DataHelper.ToBytes<Test4[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 10);
            PrintBytes(data);
        }

        [Test]
        [Description("智能对象数组的反序列化测试")]
        public void NormalPickupTest6()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            Test4[] array = new[] { (Test4)null, (Test4)null, (Test4)null };
            byte[] data = DataHelper.ToBytes<Test4[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 10);
            PrintBytes(data);

            Test4[] results = DataHelper.GetObject<Test4[]>(data);
            Assert.IsNotNull(results);
            Assert.IsTrue(array.Length == results.Length);
            for (int i = 0; i < results.Length; i++) results[i] = array[i];
        }

        [Test]
        [Description("bool序列化测试")]
        public void BooleanBindTest()
        {
            byte[] data = DataHelper.ToBytes<bool>(true);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 1);
            PrintBytes(data);
        }

        [Test]
        [Description("int?序列化测试")]
        public void NullableInt32BindTest()
        {
            byte[] data = DataHelper.ToBytes<int?>((int?)6);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);
        }

        [Test]
        [Description("int? - null value序列化测试")]
        public void Nullable_NullValue_BindTest()
        {
            byte[] data = DataHelper.ToBytes<int?>(null);
            Assert.IsNull(data);
        }

        [Test]
        [Description("bool反序列化测试")]
        public void BooleanPickupTest()
        {
            byte[] data = DataHelper.ToBytes<bool>(true);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 1);
            PrintBytes(data);

            bool result = DataHelper.GetObject<bool>(data);
            Assert.IsTrue(result);
        }

        [Test]
        [Description("sbyte序列化测试")]
        public void SByteBindTest()
        {
            byte[] data = DataHelper.ToBytes<sbyte>((sbyte)0x0F);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 1);
            PrintBytes(data);
        }

        [Test]
        [Description("sbyte反序列化测试")]
        public void SBytePickupTest()
        {
            byte[] data = DataHelper.ToBytes<sbyte>((sbyte)0x0F);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 1);
            PrintBytes(data);

            sbyte result = DataHelper.GetObject<sbyte>(data);
            Assert.IsTrue(result == 0x0F);
        }

        [Test]
        [Description("byte序列化测试")]
        public void ByteBindTest()
        {
            byte[] data = DataHelper.ToBytes<byte>((byte)0xFF);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 1);
            PrintBytes(data);
        }

        [Test]
        [Description("byte反序列化测试")]
        public void BytePickupTest()
        {
            byte[] data = DataHelper.ToBytes<byte>((byte)0xFF);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 1);
            PrintBytes(data);

            byte result = DataHelper.GetObject<byte>(data);
            Assert.IsTrue(result == 0xFF);
        }

        [Test]
        [Description("decimal序列化测试")]
        public void DecimalBindTest()
        {
            byte[] data = DataHelper.ToBytes<decimal>((decimal)0xFF);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 16);
            PrintBytes(data);
        }

        [Test]
        [Description("decimal反序列化测试")]
        public void DecimalPickupTest()
        {
            byte[] data = DataHelper.ToBytes<decimal>((decimal)0xFF);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 16);
            PrintBytes(data);

            decimal result = DataHelper.GetObject<decimal>(data);
            Assert.IsTrue(result == 0xFF);
        }

        [Test]
        [Description("byte数组序列化测试")]
        public void ByteArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<byte[]>(Encoding.UTF8.GetBytes("kevin"));
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 5);
            PrintBytes(data);
        }

        [Test]
        [Description("byte数组反序列化测试")]
        public void ByteArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<byte[]>(Encoding.UTF8.GetBytes("kevin"));
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 5);
            PrintBytes(data);

            byte[] result = DataHelper.GetObject<byte[]>(data);
            Assert.IsTrue(result.Length == 5);
            Assert.IsTrue(Encoding.UTF8.GetString(result) == "kevin");
        }

        [Test]
        [Description("char序列化测试")]
        public void CharBindTest()
        {
            byte[] data = DataHelper.ToBytes<char>('m');
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 2);
            PrintBytes(data);
        }

        [Test]
        [Description("char反序列化测试")]
        public void CharPickupTest()
        {
            byte[] data = DataHelper.ToBytes<char>('m');
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 2);
            PrintBytes(data);

            char result = DataHelper.GetObject<char>(data);
            Assert.IsTrue(result == 'm');
        }

        [Test]
        [Description("double序列化测试")]
        public void DoubleBindTest()
        {
            byte[] data = DataHelper.ToBytes<double>((double)3.5);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);
        }

        [Test]
        [Description("double反序列化测试")]
        public void DoublePickupTest()
        {
            byte[] data = DataHelper.ToBytes<double>((double)3.5);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);

            double result = DataHelper.GetObject<double>(data);
            Assert.IsTrue(result == 3.5);
        }

        [Test]
        [Description("float序列化测试")]
        public void FloatBindTest()
        {
            byte[] data = DataHelper.ToBytes<float>((float)6.6);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);
        }

        [Test]
        [Description("float反序列化测试")]
        public void FloatPickupTest()
        {
            byte[] data = DataHelper.ToBytes<float>((float)6.6);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);

            float result = DataHelper.GetObject<float>(data);
            Assert.IsTrue(result == (float)6.6);
        }

        [Test]
        [Description("Int16序列化测试")]
        public void Int16BindTest()
        {
            byte[] data = DataHelper.ToBytes<short>((short)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 2);
            PrintBytes(data);
        }

        [Test]
        [Description("Int16反序列化测试")]
        public void Int16PickupTest()
        {
            byte[] data = DataHelper.ToBytes<short>((short)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 2);
            PrintBytes(data);

            short result = DataHelper.GetObject<short>(data);
            Assert.IsTrue(result == (short)3);
        }

        [Test]
        [Description("Int32序列化测试")]
        public void Int32BindTest()
        {
            byte[] data = DataHelper.ToBytes<int>(3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);
        }

        [Test]
        [Description("Int32反序列化测试")]
        public void Int32PickupTest()
        {
            byte[] data = DataHelper.ToBytes<int>(3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);

            int result = DataHelper.GetObject<int>(data);
            Assert.IsTrue(result == (int)3);
        }

        [Test]
        [Description("Int64序列化测试")]
        public void Int64BindTest()
        {
            byte[] data = DataHelper.ToBytes<long>((long)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);
        }

        [Test]
        [Description("Int64反序列化测试")]
        public void Int64PickupTest()
        {
            byte[] data = DataHelper.ToBytes<long>((long)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);

            long result = DataHelper.GetObject<long>(data);
            Assert.IsTrue(result == (long)3);
        }

        [Test]
        [Description("Int16数组序列化测试")]
        public void Int16ArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<short[]>(new short[] {1, 2, 3, 4, 5, 6});
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            PrintBytes(data);
        }

        [Test]
        [Description("Int16数组反序列化测试")]
        public void Int16ArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<short[]>(new short[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            PrintBytes(data);

            short[] result = DataHelper.GetObject<short[]>(data);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);
            Assert.IsTrue(result[5] == 6);
        }

        [Test]
        [Description("Int32数组序列化测试")]
        public void Int32ArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<int[]>(new[] {1, 2, 3, 4, 5, 6});
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 24);
            PrintBytes(data);
        }

        [Test]
        [Description("Int32数组反序列化测试")]
        public void Int32ArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<int[]>(new[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 24);
            PrintBytes(data);

            int[] result = DataHelper.GetObject<int[]>(data);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);
            Assert.IsTrue(result[5] == 6);
        }

        [Test]
        [Description("Int64数组序列化测试")]
        public void Int64ArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<long[]>(new long[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 48);
            PrintBytes(data);
        }

        [Test]
        [Description("Int64数组反序列化测试")]
        public void Int64ArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<long[]>(new long[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 48);
            PrintBytes(data);

            long[] result = DataHelper.GetObject<long[]>(data);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);
            Assert.IsTrue(result[5] == 6);
        }

        [Test]
        [Description("UInt16数组序列化测试")]
        public void UInt16ArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<ushort[]>(new ushort[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            PrintBytes(data);
        }

        [Test]
        [Description("UInt16数组反序列化测试")]
        public void UInt16ArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<ushort[]>(new ushort[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            PrintBytes(data);

            ushort[] result = DataHelper.GetObject<ushort[]>(data);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);
            Assert.IsTrue(result[5] == 6);
        }

        [Test]
        [Description("UInt32数组序列化测试")]
        public void UInt32ArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<uint[]>(new uint[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 24);
            PrintBytes(data);
        }

        [Test]
        [Description("UInt32数组反序列化测试")]
        public void UInt32ArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<uint[]>(new uint[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 24);
            PrintBytes(data);

            uint[] result = DataHelper.GetObject<uint[]>(data);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);
            Assert.IsTrue(result[5] == 6);
        }

        [Test]
        [Description("UInt64数组序列化测试")]
        public void UInt64ArrayBindTest()
        {
            byte[] data = DataHelper.ToBytes<ulong[]>(new ulong[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 48);
            PrintBytes(data);
        }

        [Test]
        [Description("UInt64数组反序列化测试")]
        public void UInt64ArrayPickupTest()
        {
            byte[] data = DataHelper.ToBytes<ulong[]>(new ulong[] { 1, 2, 3, 4, 5, 6 });
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 48);
            PrintBytes(data);

            ulong[] result = DataHelper.GetObject<ulong[]>(data);
            Assert.IsTrue(result.Length == 6);
            Assert.IsTrue(result[0] == 1);
            Assert.IsTrue(result[1] == 2);
            Assert.IsTrue(result[2] == 3);
            Assert.IsTrue(result[3] == 4);
            Assert.IsTrue(result[4] == 5);
            Assert.IsTrue(result[5] == 6);
        }

        [Test]
        [Description("UInt16序列化测试")]
        public void UInt16BindTest()
        {
            byte[] data = DataHelper.ToBytes<ushort>((ushort)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 2);
            PrintBytes(data);
        }

        [Test]
        [Description("UInt16反序列化测试")]
        public void UInt16PickupTest()
        {
            byte[] data = DataHelper.ToBytes<ushort>((ushort)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 2);
            PrintBytes(data);

            ushort result = DataHelper.GetObject<ushort>(data);
            Assert.IsTrue(result == (ushort) 3);
        }

        [Test]
        [Description("UInt32序列化测试")]
        public void UInt32BindTest()
        {
            byte[] data = DataHelper.ToBytes<uint>((uint)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);
        }

        [Test]
        [Description("UInt32反序列化测试")]
        public void UInt32PickupTest()
        {
            byte[] data = DataHelper.ToBytes<uint>((uint)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);

            uint result = DataHelper.GetObject<uint>(data);
            Assert.IsTrue(result == (uint)3);
        }

        [Test]
        [Description("UInt64序列化测试")]
        public void UInt64BindTest()
        {
            byte[] data = DataHelper.ToBytes<ulong>((ulong)3);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);
        }

        [Test]
        [Description("UInt64反序列化测试")]
        public void UInt64PickupTest()
        {
            byte[] data = DataHelper.ToBytes<ulong>((ulong)5);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);

            ulong result = DataHelper.GetObject<ulong>(data);
            Assert.IsTrue(result == (ulong)5);
        }

        [Test]
        [Description("string序列化测试")]
        public void StringBindTest()
        {
            string testStr = "~~~~~~tes祖国t";
            byte[] data = DataHelper.ToBytes<string>(testStr);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == Encoding.UTF8.GetBytes(testStr).Length);
            PrintBytes(data);
        }

        [Test]
        [Description("string反序列化测试")]
        public void StringPickupTest()
        {
            string testStr = "~~~~~~tes祖国t";
            byte[] data = DataHelper.ToBytes<string>(testStr);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == Encoding.UTF8.GetBytes(testStr).Length);
            PrintBytes(data);

            string result = DataHelper.GetObject<string>(data);
            Assert.IsTrue(result == testStr);
        }

        [Test]
        [Description("DateTime序列化测试")]
        public void DateTimeBindTest()
        {
            DateTime now = DateTime.Now;
            byte[] data = DataHelper.ToBytes<DateTime>(now);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);
        }

        [Test]
        [Description("DateTime反序列化测试")]
        public void DateTimePickupTest()
        {
            DateTime now = DateTime.Now;
            byte[] data = DataHelper.ToBytes<DateTime>(now);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);

            DateTime result = DataHelper.GetObject<DateTime>(data);
            Assert.IsTrue(result == now);
        }

        [Test]
        [Description("DateTime数组序列化测试")]
        public void DateTimeArrayBindTest()
        {
            DateTime[] array = new[] { DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now };
            byte[] data = DataHelper.ToBytes<DateTime[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length*8);
            PrintBytes(data);
        }

        [Test]
        [Description("DateTime数组反序列化测试")]
        public void DateTimeArrayPickupTest()
        {
            DateTime[] array = new[] { DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now };
            byte[] data = DataHelper.ToBytes<DateTime[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 8);
            PrintBytes(data);

            DateTime[] result = DataHelper.GetObject<DateTime[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("TimeSpan序列化测试")]
        public void TimeSpanBindTest()
        {
            TimeSpan now = new TimeSpan(1, 2, 3, 4);
            byte[] data = DataHelper.ToBytes<TimeSpan>(now);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);
        }

        [Test]
        [Description("TimeSpan反序列化测试")]
        public void TimeSpanPickupTest()
        {
            TimeSpan now = new TimeSpan(1, 2, 3, 4);
            byte[] data = DataHelper.ToBytes<TimeSpan>(now);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 8);
            PrintBytes(data);

            TimeSpan result = DataHelper.GetObject<TimeSpan>(data);
            Assert.IsTrue(result == now);
        }

        [Test]
        [Description("TimeSpan数组序列化测试")]
        public void TimeSpanArrayBindTest()
        {
            TimeSpan[] array = new[] { new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 3, 4) };
            byte[] data = DataHelper.ToBytes<TimeSpan[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 8);
            PrintBytes(data);
        }

        [Test]
        [Description("TimeSpan数组反序列化测试")]
        public void TimeSpanArrayPickupTest()
        {
            TimeSpan[] array = new[] { new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 3, 4), new TimeSpan(1, 2, 8, 4), new TimeSpan(1, 2, 3, 4) };
            byte[] data = DataHelper.ToBytes<TimeSpan[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 8);
            PrintBytes(data);

            TimeSpan[] result = DataHelper.GetObject<TimeSpan[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("Guid序列化测试")]
        public void GuidBindTest()
        {
            Guid guid = Guid.NewGuid();
            byte[] data = DataHelper.ToBytes<Guid>(guid);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 16);
            PrintBytes(data);
        }

        [Test]
        [Description("Guid反序列化测试")]
        public void GuidPickupTest()
        {
            Guid guid = Guid.NewGuid();
            byte[] data = DataHelper.ToBytes<Guid>(guid);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 16);
            PrintBytes(data);

            Guid result = DataHelper.GetObject<Guid>(data);
            Assert.IsTrue(result == guid);
        }

        [Test]
        [Description("Guid数组序列化测试")]
        public void GuidArrayBindTest()
        {
            Guid[] array = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            byte[] data = DataHelper.ToBytes<Guid[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length*16);
            PrintBytes(data);
        }

        [Test]
        [Description("Guid数组反序列化测试")]
        public void GuidArrayPickupTest()
        {
            Guid[] array = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            byte[] data = DataHelper.ToBytes<Guid[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 16);
            PrintBytes(data);

            Guid[] result = DataHelper.GetObject<Guid[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("IPEndPoint序列化测试")]
        public void IPEndPointBindTest()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 6666);
            byte[] data = DataHelper.ToBytes<IPEndPoint>(iep);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            PrintBytes(data);
        }

        [Test]
        [Description("IPEndPoint反序列化测试")]
        public void IPEndPointPickupTest()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 6666);
            byte[] data = DataHelper.ToBytes<IPEndPoint>(iep);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 12);
            PrintBytes(data);

            IPEndPoint result = DataHelper.GetObject<IPEndPoint>(data);
            Assert.IsTrue(result.Equals(iep));
        }

        [Test]
        [Description("IPEndPoint数组序列化测试")]
        public void IPEndPointArrayBindTest()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 6666);
            IPEndPoint[] array = new[] {iep, iep, iep, iep, iep};
            byte[] data = DataHelper.ToBytes<IPEndPoint[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length*12);
            PrintBytes(data);
        }

        [Test]
        [Description("IPEndPoint数组反序列化测试")]
        public void IPEndPointArrayPickupTest()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 6666);
            IPEndPoint[] array = new[] { iep, iep, iep, iep, iep };
            byte[] data = DataHelper.ToBytes<IPEndPoint[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 12);
            PrintBytes(data);

            IPEndPoint[] result = DataHelper.GetObject<IPEndPoint[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i].Equals(array[i]));
        }

        [Test]
        [Description("IntPtr序列化测试")]
        public void IntPtrBindTest()
        {
            IntPtr intPtr = new IntPtr(16666);
            byte[] data = DataHelper.ToBytes<IntPtr>(intPtr);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);
        }

        [Test]
        [Description("IntPtr反序列化测试")]
        public void IntPtrPickupTest()
        {
            IntPtr intPtr = new IntPtr(16666);
            byte[] data = DataHelper.ToBytes<IntPtr>(intPtr);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4);
            PrintBytes(data);

            IntPtr result = DataHelper.GetObject<IntPtr>(data);
            Assert.IsTrue(result == intPtr);
        }

        [Test]
        [Description("IntPtr数组序列化测试")]
        public void IntPtrArrayBindTest()
        {
            IntPtr[] array = new[] { new IntPtr(16666), new IntPtr(16666), new IntPtr(16666), new IntPtr(16666), new IntPtr(16666) };
            byte[] data = DataHelper.ToBytes<IntPtr[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 4);
            PrintBytes(data);
        }

        [Test]
        [Description("IntPtr数组反序列化测试")]
        public void IntPtrArrayPickupTest()
        {
            IntPtr[] array = new[] { new IntPtr(16666), new IntPtr(16666), new IntPtr(16666), new IntPtr(16666), new IntPtr(16666) };
            byte[] data = DataHelper.ToBytes<IntPtr[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == array.Length * 4);
            PrintBytes(data);

            IntPtr[] result = DataHelper.GetObject<IntPtr[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("string数组序列化测试")]
        public void StringArrayBindTest()
        {
            string testStr = "~~~~~~test";
            string[] array = new[] {testStr, testStr, testStr, testStr, testStr};
            byte[] data = DataHelper.ToBytes<string[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4 + ((testStr.Length + 2)*array.Length));
            PrintBytes(data);
        }

        [Test]
        [Description("string数组反序列化测试")]
        public void StringArrayPickupTest()
        {
            string testStr = "~~~~~~test";
            string[] array = new[] { testStr, testStr, testStr, testStr, testStr };
            byte[] data = DataHelper.ToBytes<string[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 4 + ((testStr.Length + 2) * array.Length));
            PrintBytes(data);

            string[] result = DataHelper.GetObject<string[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("string数组序列化测试, 数组中部分元素为null")]
        public void StringArrayAndLessElementsBindTest()
        {
            string testStr = "~~~~~~test";
            string[] array = new[] { null, testStr, null, testStr, null };
            byte[] data = DataHelper.ToBytes<string[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == (4 + ((testStr.Length + 2)*array.Length)) - (testStr.Length*3));
            PrintBytes(data);
        }

        [Test]
        [Description("string数组反序列化测试, 数组中部分元素为null")]
        public void StringArrayAndLessElementsPickupTest()
        {
            string testStr = "~~~~~~test";
            string[] array = new[] { null, testStr, null, testStr, null };
            byte[] data = DataHelper.ToBytes<string[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == (4 + ((testStr.Length + 2) * array.Length)) - (testStr.Length * 3));
            PrintBytes(data);

            string[] result = DataHelper.GetObject<string[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("string数组序列化测试, 数组中部分元素为null")]
        public void StringArrayAndLossAllElementsBindTest()
        {
            string[] array = new[] { (string) null, (string) null, (string) null, (string) null, (string) null };
            byte[] data = DataHelper.ToBytes<string[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == (4 + (array.Length * 2)));
            PrintBytes(data);
        }

        [Test]
        [Description("string数组反序列化测试, 数组中部分元素为null")]
        public void StringArrayAndLossAllElementsPickupTest()
        {
            string[] array = new[] { (string)null, (string)null, (string)null, (string)null, (string)null };
            byte[] data = DataHelper.ToBytes<string[]>(array);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == (4 + (array.Length * 2)));
            PrintBytes(data);

            string[] result = DataHelper.GetObject<string[]>(data);
            Assert.IsTrue(result.Length == array.Length);
            for (int i = 0; i < array.Length; i++) Assert.IsTrue(result[i] == array[i]);
        }

        [Test]
        [Description("string序列化测试")]
        public void LongStringBindTest()
        {
            string testStr = new string('*', 666666);
            byte[] data = DataHelper.ToBytes<string>(testStr);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == testStr.Length);
            PrintBytes(data);
        }

        [Test]
        [Description("string反序列化测试")]
        public void LongStringPickupTest()
        {
            string testStr = new string('*', 666666);
            byte[] data = DataHelper.ToBytes<string>(testStr);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == testStr.Length);
            PrintBytes(data);

            string result = DataHelper.GetObject<string>(data);
            Assert.IsTrue(result == testStr);
        }

        public static void PrintBytes(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (i % 4 == 0) Console.Write("    ");
                Console.Write(data[i] + " ");
            }
            Console.WriteLine();
        }
    }
}