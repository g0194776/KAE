using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_CHANNEL_DISCONNECTED(Object sender, ChannelDisconnectedEventArgs e);
    /// <summary>
    ///     通道已经断开事件
    /// </summary>
    public class ChannelDisconnectedEventArgs : System.EventArgs
    {
        private int _key;
        /// <summary>
        ///     通道唯一标示
        /// </summary>
        public int Key
        {
            get { return _key; }
        }

        /// <summary>
        ///     通道已经断开事件
        /// </summary>
        /// <param name="Key">通道唯一标示类型</param>
        public ChannelDisconnectedEventArgs(int Key)
        {
            _key = Key;
        }
    }
}
