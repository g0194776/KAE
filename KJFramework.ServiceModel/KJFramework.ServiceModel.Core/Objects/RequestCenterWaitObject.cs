using System;
using System.Threading;
using KJFramework.Cache;
using KJFramework.Net.Channels.Identities;
using KJFramework.Timer;

namespace KJFramework.ServiceModel.Core.Objects
{
    /// <summary>
    ///     请求中心等待对象
    /// </summary>
    public class RequestCenterWaitObject : IClearable
    {
        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 成员

        /// <summary>
        ///     获取或设置超时器
        /// </summary>
        public LightTimer Timer { get; set; }
        /// <summary>
        ///     获取或设置同步器
        /// </summary>
        public AutoResetEvent ResetEvent { get; set; }
        /// <summary>
        ///     获取或设置唯一键值
        /// </summary>
        public TransactionIdentity Key { get; set; }

        public DateTime Time { get; set; }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// 清除对象自身
        /// </summary>
        public void Clear()
        {
            Key = null;
            ResetEvent = null;
            Timer = null;
        }

        #endregion
    }
}