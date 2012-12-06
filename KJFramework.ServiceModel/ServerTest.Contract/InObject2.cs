using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace ServerTest.Contract
{
    public class InObject2 : IntellectObject
    {
        [IntellectProperty(0, IsRequire = true)]
        public string Info { get; set; }
        [IntellectProperty(1, IsRequire = true)]
        public int Inf1 { get; set; }
    }
}