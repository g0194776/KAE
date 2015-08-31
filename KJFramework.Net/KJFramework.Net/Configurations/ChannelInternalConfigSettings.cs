using System;

namespace KJFramework.Net.Configurations
{
    /// <summary>
    ///    KJFramework网络底层内部配置项集合
    /// </summary>
    [Serializable]
    internal sealed class ChannelInternalConfigSettings
    {
        #region Members.

        /// <summary>
        ///    获取或设置基于老版本缓冲区的总体大小
        /// </summary>
        public int RecvBufferSize { get; set; }
        /// <summary>
        ///     获取或设置SocketAsyncEventArgs缓存的数量
        /// </summary>
        public int BuffStubPoolSize { get; set; }
        /// <summary>
        ///     获取或设置基于命名管道的缓冲池大小
        /// </summary>
        public int NamedPipeBuffStubPoolSize { get; set; }
        /// <summary>
        ///     获取或设置发送不关联任何BUFF的缓冲对象个数
        /// </summary>
        public int NoBuffStubPoolSize { get; set; }
        /// <summary>
        ///     获取或设置最大消息长度
        /// </summary>
        public int MaxMessageDataLength { get; set; }
        /// <summary>
        ///     获取或设置内存缓冲区中每一个内存分片的大小
        /// </summary>
        public int SegmentSize { get; set; }

        #endregion
    }
}