using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace ServerTest.Contract
{
    public class InObject : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public string Info { get; set; }
        [IntellectProperty(1, IsRequire = true)]
        public int Inf1 { get; set; }
        [IntellectProperty(2, IsRequire = true)]
        public DateTime Info2 { get; set; }
        [IntellectProperty(3, IsRequire = true)]
        public string[] Info3 { get; set; }
        [IntellectProperty(4, IsRequire = true)]
        public int[] Info4 { get; set; }
        [IntellectProperty(5, IsRequire = false)]
        public InObject2[] Info5 { get; set; }
    }
}