using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.UnitTest
{
    public class TestObject : IntellectObject
    {
        [IntellectProperty(0)]
        public TestObject1 Obj { get; set; }
        [IntellectProperty(1)]
        public int[] Mm { get; set; }
        [IntellectProperty(2)]
        public TestObject1[] Pp { get; set; }
        [IntellectProperty(3)]
        public String[] Uu { get; set; }
        [IntellectProperty(4)]
        public String[] Jj { get; set; }
        [IntellectProperty(5)]
        public int Wokao { get; set; }
        [IntellectProperty(6)]
        public int Wocao { get; set; }
        [IntellectProperty(7)]
        public string Woqunimade { get; set; }
        [IntellectProperty(8)]
        public byte[] Metadata { get; set; }
        [IntellectProperty(9)]
        public byte Metadata1 { get; set; }
        [IntellectProperty(10)]
        public DateTime Time { get; set; }
        [IntellectProperty(11)]
        public long? NullableValue1 { get; set; }
        [IntellectProperty(12)]
        public short? NullableValue2 { get; set; }
        [IntellectProperty(13)]
        public double? NullableValue3 { get; set; }
        [IntellectProperty(14)]
        public int? NullableValue4 { get; set; }
    }
}