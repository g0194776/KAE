using KJFramework.Messages.Objects;

namespace KJFramework.Messages.Test
{
    [System.Serializable]
    public class TestObject2 : IClassSerializeObject
    {
        private int _nice;
        public int Nice
        {
            get { return _nice; }
            set { _nice = value; }
        }
    }
}