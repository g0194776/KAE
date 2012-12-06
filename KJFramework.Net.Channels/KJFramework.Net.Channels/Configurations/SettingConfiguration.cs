using KJFramework.Attribute;

namespace KJFramework.Net.Channels.Configurations
{
    /// <summary>
    ///     相关配置项 
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   传输通道缓冲区大小
        /// </summary>
        [CustomerField("RecvBufferSize")]
        public int RecvBufferSize;
        /// <summary>
        ///    底层SocketAsyncEventArgs缓存个数
        ///     <para>* 此类型缓存将会持有内存缓冲区</para>
        /// </summary>
        [CustomerField("BuffStubPoolSize")]
        public int BuffStubPoolSize;
        /// <summary>
        ///    底层SocketAsyncEventArgs缓存个数
        /// </summary>
        [CustomerField("NoBuffStubPoolSize")]
        public int NoBuffStubPoolSize;
        /// <summary>
        ///    最大消息包长度, 如果超过此长度则进行分包处理
        /// </summary>
        [CustomerField("MaxMessageDataLength")]
        public int MaxMessageDataLength;
        /// <summary>
        ///    获取或设置内存片段的大小
        /// </summary>
        [CustomerField("SegmentSize")]
        public int SegmentSize;
        /// <summary>
        ///    获取或设置内存片段的缓冲区总长度
        /// </summary>
        [CustomerField("SegmentBuffer")]
        public int SegmentBuffer;
    }
}