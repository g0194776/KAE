using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.ServiceModel.Core.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.ServiceModel.UnitTest
{
    public class Test1 : IntellectObject
    {
        [IntellectProperty(0)]
        public int Int1 { get; set; }
        [IntellectProperty(1)]
        public int Int2 { get; set; }
        [IntellectProperty(2)]
        public Test2 T2 { get; set; }
        [IntellectProperty(3)]
        public int Int4 { get; set; }
    }

    public class Test2 : IntellectObject
    {
        [IntellectProperty(0)]
        public string Str1 { get; set; }
        [IntellectProperty(1)]
        public double Int2 { get; set; }
        [IntellectProperty(2)]
        public float Int3 { get; set; }
        [IntellectProperty(3)]
        public Test3 T3 { get; set; }
        [IntellectProperty(4)]
        public ushort Int4 { get; set; }
    }

    public class Test3 : IntellectObject
    {
        [IntellectProperty(0)]
        public string Str1 { get; set; }
        [IntellectProperty(1)]
        public uint Int2 { get; set; }
        [IntellectProperty(2)]
        public IntPtr Int3 { get; set; }
        [IntellectProperty(4)]
        public DateTime Int4 { get; set; }
    }


    [TestClass]
    public class MetadataGeneratorTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            IMetadataTypeGenerator generator = new MetadataTypeGenerator();
            Dictionary<string, string> dictionary = generator.Generate(typeof (Test1));
        }
    }
}