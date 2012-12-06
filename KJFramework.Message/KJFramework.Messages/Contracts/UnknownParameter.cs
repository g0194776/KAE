namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     未知参数，提供了相关的基本操作。
    /// </summary>
    internal class UnknownParameter : IUnknownParameter
    {
        #region Members

        protected int _id;
        protected byte[] _content;

        #endregion

        #region Implementation of IUnknownParameter

        /// <summary>
        ///     获取参数编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        /// <summary>
        ///     获取参数元数据
        /// </summary>
        public byte[] Content
        {
            get { return _content; }
            internal set { _content = value; }
        }

        #endregion
    }
}