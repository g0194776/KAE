using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace ServerTest.Contract
{
    public class SpeendObject : IntellectObject
    {
        [IntellectProperty(0)]
        public string Title { get; set; }
    }
}