using System;

namespace KJFramework.Net.Channel
{
    /// <summary>
    ///     基础的通道信息, 提供了相关的通道信息结构
    /// </summary>
    public class BasicChannelInfomation
    {
        #region IChannelInfomation 成员

        private int _port;

        /// <summary>
        ///     获取或设置对方服务端口
        /// </summary>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        /// <summary>
        ///     获取通道的创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return DateTime.Now; }
        }

        #endregion
    }
}
