using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace ServerTest
{
    public class InObject3 : IntellectObject
    {
        [IntellectProperty(0)]
        public string Name { get; set; }
    }
}