using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.Test
{
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
}