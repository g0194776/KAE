using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Test
{
    public class TestObjectExtend
    {

        private int _miaomiao;
        [IntellectProperty(20)]
        public int Miaomiao
        {
            get { return _miaomiao; }
            set { _miaomiao = value; }
        }
    }
}