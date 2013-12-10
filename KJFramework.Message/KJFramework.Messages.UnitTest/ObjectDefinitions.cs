using System;
using System.Net;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.UnitTest
{
    public enum Colors : byte
    {
        Red = 0x000001,
        Yellow = 0x000002
    }

    public class OneFieldMessage : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public int X { get; set; }
    }

    public class TestObjectByteArray : IntellectObject
    {
        [IntellectProperty(0)]
        public byte[] Array { get; set; }
    }

    public class TestObject : IntellectObject
    {
        private TestObject1 _obj;
        [IntellectProperty(7)]
        public TestObject1 Obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        private int[] _mm;
        [IntellectProperty(0)]
        public int[] Mm
        {
            get { return _mm; }
            set { _mm = value; }
        }

        private TestObject1[] _pp;
        [IntellectProperty(27)]
        public TestObject1[] Pp
        {
            get { return _pp; }
            set { _pp = value; }
        }


        private String[] _uu;
        [IntellectProperty(28)]
        public String[] Uu
        {
            get { return _uu; }
            set { _uu = value; }
        }

        private String[] _jj;
        [IntellectProperty(26)]
        public String[] Jj
        {
            get { return _jj; }
            set { _jj = value; }
        }

        private int _wokao;
        [IntellectProperty(4)]
        public int Wokao
        {
            get { return _wokao; }
            set { _wokao = value; }
        }

        private int _wocao;
        [IntellectProperty(2)]
        public int Wocao
        {
            get { return _wocao; }
            set { _wocao = value; }
        }

        private string _woqunimade;
        [IntellectProperty(3)]
        public string Woqunimade
        {
            get { return _woqunimade; }
            set { _woqunimade = value; }
        }

        private byte[] _metadata;
        [IntellectProperty(13)]
        public byte[] Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }


        private byte _metadata1;
        [IntellectProperty(15)]
        public byte Metadata1
        {
            get { return _metadata1; }
            set { _metadata1 = value; }
        }

        private DateTime _time;
        [IntellectProperty(100)]
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        [IntellectProperty(101)]
        public long? NullableValue1 { get; set; }
        [IntellectProperty(102)]
        public short? NullableValue2 { get; set; }
        [IntellectProperty(103)]
        public double? NullableValue3 { get; set; }
        [IntellectProperty(104)]
        public int? NullableValue4 { get; set; }
    }

    public class Test1 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
    }

    public class TestObject1 : IntellectObject
    {
        private string _haha;
        [IntellectProperty(0)]
        public string Haha
        {
            get { return _haha; }
            set { _haha = value; }
        }

        private Colors _colors;
        [IntellectProperty(1)]
        public Colors Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }
    }

    public class Test2 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public string Name { get; set; }
    }

    public class Test3 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public int[] Numbers { get; set; }
        [IntellectProperty(3)]
        public int DetailsId { get; set; }
    }

    public class Test4 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public string[] Names { get; set; }
    }

    public class Test5 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public Test2 InnerObject { get; set; }
    }

    public class Test6 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public Test2[] InnerObjects { get; set; }
    }

    public class Test7 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public Colors Color { get; set; }
    }

    public class Test8 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public IntPtr Ptr { get; set; }
    }

    public class Test9 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }

        public int this[int index]
        {
            get { return 1; }
        }
    }

    public class Test10 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServicelId { get; set; }
        [IntellectProperty(2)]
        public byte[] Data { get; set; }
    }

    public class Test11 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public Guid Guid { get; set; }
        [IntellectProperty(2)]
        public int ServiceId { get; set; }
    }

    public class Test12 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public ulong DetailsId { get; set; }
        [IntellectProperty(2)]
        public int ServiceId { get; set; }
    }

    public class Test13 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public sbyte DetailsId { get; set; }
        [IntellectProperty(2)]
        public int ServiceId { get; set; }
    }

    public class Test14 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public decimal DetailsId { get; set; }
        [IntellectProperty(2)]
        public int ServiceId { get; set; }
    }

    public class Test15 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public ushort DetailsId { get; set; }
        [IntellectProperty(2)]
        public int ServiceId { get; set; }
    }

    public class Test16 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public uint DetailsId { get; set; }
        [IntellectProperty(2)]
        public int ServiceId { get; set; }
    }

    public class Test17 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public int? DetailsId { get; set; }
    }

    public class Test18 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public int[] Users { get; set; }
    }

    public class Test19 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public BitFlag Flag { get; set; }
    }

    public class Test20 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public IPEndPoint Iep { get; set; }
    }

    public class Test21 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public Guid Identity { get; set; }
    }

    public class Test22 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public TimeSpan Time { get; set; }
    }

    public class Test23 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public string Name { get; set; }
    }

    public class Test24 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public int[] Values { get; set; }
    }

    public class Test25 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public short[] Values { get; set; }
    }

    public class Test26 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public float[] Values { get; set; }
    }

    public class Test27 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public bool[] Values { get; set; }
    }

    public class Test28 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public Guid[] Values { get; set; }
    }

    public class Test29 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public double[] Values { get; set; }
    }

    public class Test30 : IntellectObject
    {
        [IntellectProperty(0)]
        public byte[] Values { get; set; }
    }

    public class Test31 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public long[] Values { get; set; }
    }

    public class Test32 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public char[] Values { get; set; }
    }

    public class Test33 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public DateTime[] Values { get; set; }
    }

    public class Test34 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public decimal[] Values { get; set; }
    }

    public class Test35 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public IntPtr[] Values { get; set; }
    }

    public class Test36 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public sbyte[] Values { get; set; }
    }

    public class Test37 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public TimeSpan[] Values { get; set; }
    }

    public class Test38 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public ushort[] Values { get; set; }
    }

    public class Test39 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public uint[] Values { get; set; }
    }

    public class Test40 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public ulong[] Values { get; set; }
    }

    public class Test41 : IntellectObject
    {
        [IntellectProperty(0)]
        public string[] Values { get; set; }
    }

    public class Test42 : IntellectObject
    {
        [IntellectProperty(0)]
        public int[] Values { get; set; }
    }

    public class Test43 : IntellectObject
    {
        [IntellectProperty(0)]
        public string Value { get; set; }
    }

    public class Test44 : IntellectObject
    {
        [IntellectProperty(0, AllowDefaultNull = true)]
        public string Value { get; set; }
    }

    public class Test45 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public int Y { get; set; }
    }

    public class Test46 : IntellectObject
    {
        [IntellectProperty(0, AllowDefaultNull = true)]
        public int? Value { get; set; }
    }

    public class Test47 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public uint Y { get; set; }
    }

    public class Test48 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public short Y { get; set; }
    }

    public class Test49 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public ushort Y { get; set; }
    }

    public class Test50 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public Guid Y { get; set; }
    }

    public class Test51 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public DateTime Y { get; set; }
    }

    public class Test52 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public TimeSpan Y { get; set; }
    }

    public class Test53 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public IntPtr Y { get; set; }
    }

    public class Test54 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public double Y { get; set; }
    }

    public class Test55 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public float Y { get; set; }
    }

    public class Test56 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public long Y { get; set; }
    }

    public class Test57 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public ulong Y { get; set; }
    }

    public class Test58 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public decimal Y { get; set; }
    }

    public class Test59 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public char Y { get; set; }
    }

    public class Test60 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public bool Y { get; set; }
    }

    public class Test61 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public byte Y { get; set; }
    }

    public class Test62 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public sbyte Y { get; set; }
    }

    public class Test63 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1, AllowDefaultNull = true)]
        public int Y { get; set; }
        [IntellectProperty(2)]
        public int Z { get; set; }
        [IntellectProperty(3, AllowDefaultNull = true)]
        public Guid N { get; set; }
        [IntellectProperty(4)]
        public TimeSpan? M { get; set; }
    }

    internal class Test64 : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
    }


    public class Test65 : IntellectObject
    {
        public Test65()
        {
        }

        public Test65(string name)
        {
            Name = name;
        }

        [IntellectProperty(0)]
        public string Name { get; private set; }
    }

    public class Test66 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public IPEndPoint[] Ieps { get; set; }
    }

    public class Test67 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public Test1[] Objs { get; set; }
    }

    public class Test68 : IntellectObject
    {
        [IntellectProperty(0)]
        public int ProtocolId { get; set; }
        [IntellectProperty(1)]
        public int ServiceId { get; set; }
        [IntellectProperty(2)]
        public Blob Blob { get; set; }
    }

    public class CMode : IntellectObject
    {
        [IntellectProperty(0)]
        public int X { get; set; }
        [IntellectProperty(1)]
        public int Y { get; set; }
    }

    public class CMode2 : CMode
    {
        [IntellectProperty(2)]
        public int Z { get; set; }
    }

    public class CMode3
    {
        [IntellectProperty(0)]
        public int X { get; set; }
    }
}