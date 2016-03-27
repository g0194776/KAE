using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.Test
{
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
        [IntellectProperty(105)]
        public long[] Longs { get; set; }
    }
}