using System;
using System.Diagnostics;
using KJFramework.Net.Channels.Events;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     数据段线性链表节点
    /// </summary>
    [DebuggerDisplay("Remaining: {RemainingSize}")]
    public class CSNSegmentNode : ICloneable
    {
        #region Constructor

        /// <summary>
        ///     数据段线性链表节点
        /// </summary>
        public CSNSegmentNode(CSNSegmentReceiveEventArgs value)
        {
            Args = value;
        }

        #endregion

        #region Members

        /// <summary>
        ///     当前值
        /// </summary>
        public CSNSegmentReceiveEventArgs Args { get; private set; }
        /// <summary>
        ///     获取或设置剩余数据长度
        /// </summary>
        public int RemainingSize { get { return Args.BytesTransferred - Args.Stub.Cache.Segment.UsedBytes; } }
        /// <summary>
        ///     下一个节点
        /// </summary>
        public CSNSegmentNode Next { get; set; }

        #endregion

        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            return new CSNSegmentNode(Args) { Next = Next };
        }

        #endregion
    }
}