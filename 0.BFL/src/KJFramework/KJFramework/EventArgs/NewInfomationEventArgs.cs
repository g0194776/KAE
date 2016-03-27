using System;
namespace KJFramework.EventArgs
{
    /// <summary>
    ///     新信息事件
    /// </summary>
    public class NewInfomationEventArgs : System.EventArgs, IDisposable
    {
        #region 成员

        private String _infomation;
        /// <summary>
        ///     获取收集到的信息
        /// </summary>
        public string Infomation
        {
            get { return _infomation; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     新信息事件
        /// </summary>
        /// <param name="infomation">收集到的新信息</param>
        public NewInfomationEventArgs(String infomation)
        {
            _infomation = infomation;
        }

        #endregion

        #region 析构函数

        ~NewInfomationEventArgs()
        {
            Dispose();
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}