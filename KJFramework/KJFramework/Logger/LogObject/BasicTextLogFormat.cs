namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     基础的日志记录项格式，提供了所有分隔符为“空”的分割格式。
    /// </summary>
    public class BasicTextLogFormat : ITextLogFormatter
    {
        #region ITextLogFormat Members

        /// <summary>
        ///    获取上分割符
        /// </summary>
        public string Up
        {
            get { return ""; }
        }

        /// <summary>
        ///    获取上分割符
        /// </summary>
        public string Down
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        ///     获取左侧分隔符
        /// </summary>
        public string LeftSplitChar
        {
            get { return ""; }
        }

        #endregion
    }
}
