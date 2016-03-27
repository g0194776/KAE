using System;

namespace KJFramework.Net.Listener
{
    /// <summary>
    ///     端口监听器详细信息基类，提供了相关的基本信息。
    /// </summary>
    public class BasicPortListenerInfomation : IPortListenerInfomation
    {
        #region IPortListenerInfomation 成员

        private int _listenerID;

        /// <summary>
        ///     获取或设置监听器唯一ID
        ///         * 可以使用Listener.GetHashCode()获得
        /// </summary>
        public int ListenerID
        {
            get
            {
                return _listenerID;
            }
            set
            {
                _listenerID = value;
            }
        }

        private int _itemID;

        /// <summary>
        ///     获取或设置监听器分组ID
        /// </summary>
        public int ItemID
        {
            get
            {
                return _itemID;
            }
            set
            {
                _itemID = value;
            }
        }

        private int _serviceID;

        /// <summary>
        ///     获取或设置监听器服务ID
        /// </summary>
        public int ServiceID
        {
            get
            {
                return _serviceID;
            }
            set
            {
                _serviceID = value;
            }
        }

        private Object _tag;

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        #endregion
    }
}
