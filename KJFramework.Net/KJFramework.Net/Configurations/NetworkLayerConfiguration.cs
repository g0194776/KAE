using System;
using KJFramework.Attribute;

namespace KJFramework.Net.Configurations
{
    /// <summary>
    ///     网络层配置项
    /// </summary>
    public sealed class NetworkLayerConfiguration
    {
        /// <summary>
        ///     TCP接收器相关配置 [BufferSize - TCP端口缓冲区读取字节大小]
        /// </summary>
        [CustomerField("BufferSize")]
        public int BufferSize;
        /// <summary>
        ///     缓冲池最大缓冲上限（请不要小于0，最好为1024的倍数）
        /// </summary>
        [CustomerField("BufferPoolSize")]
        public int BufferPoolSize;
        /// <summary>
        ///     消息头长度
        /// </summary>
        [CustomerField("MessageHeaderLength")]
        public int MessageHeaderLength;
        /// <summary>
        ///     消息头部标示
        /// </summary>
        [CustomerField("MessageHeaderFlag")]
        public String MessageHeaderFlag;
        /// <summary>
        ///     消息头部标示长度
        /// </summary>
        [CustomerField("MessageHeaderFlagLength")]
        public int MessageHeaderFlagLength;
        /// <summary>
        ///     消息头结束标示
        /// </summary>
        [CustomerField("MessageHeaderEndFlag")]
        public String MessageHeaderEndFlag;
        /// <summary>
        ///     消息头结束标示长度
        /// </summary>
        [CustomerField("MessageHeaderEndFlagLength")]
        public int MessageHeaderEndFlagLength;
        /// <summary>
        ///     处理器路径
        /// </summary>
        [CustomerField("MessageDealerFolder")]
        public String MessageDealerFolder;
        /// <summary>
        ///     HOOK路径
        /// </summary>
        [CustomerField("MessageHookFolder")]
        public String MessageHookFolder;
        /// <summary>
        ///     拦截器路径
        /// </summary>
        [CustomerField("SpyFolder")]
        public String SpyFolder;
        /// <summary>
        ///     会话字符串模板
        /// </summary>
        [CustomerField("BasicSessionStringTemplate")]
        public String BasicSessionStringTemplate;
        /// <summary>
        ///     用户心跳间隔
        /// </summary>
        [CustomerField("UserHreatCheckTimeSpan")]
        public int UserHreatCheckTimeSpan;
        /// <summary>
        ///     用户心跳超时
        /// </summary>
        [CustomerField("UserHreatTimeout")]
        public int UserHreatTimeout;
        /// <summary>
        ///     用户心跳警告次数
        /// </summary>
        [CustomerField("UserHreatAlertCount")]
        public int UserHreatAlertCount;
        /// <summary>
        ///     FS心跳检查间隔
        /// </summary>
        [CustomerField("FSHreatCheckTimeSpan")]
        public int FSHreatCheckTimeSpan;
        /// <summary>
        ///     FS心跳超时
        /// </summary>
        [CustomerField("FSHreatTimeout")]
        public int FSHreatTimeout;
        /// <summary>
        ///     FS心跳警告次数
        /// </summary>
        [CustomerField("FSHreatAlertCount")]
        public int FSHreatAlertCount;
        /// <summary>
        ///     会话过期检测间隔
        /// </summary>
        [CustomerField("SessionExpireCheckTimeSpan")]
        public int SessionExpireCheckTimeSpan;
        /// <summary>
        ///     默认连接池的连接数量
        /// </summary>
        [CustomerField("DefaultConnectionPoolConnectCount")]
        public int DefaultConnectionPoolConnectCount;
        /// <summary>
        ///     优越的CPU使用率指标
        /// </summary>
        [CustomerField("PredominantCpuUsage")]
        public int PredominantCpuUsage;
        /// <summary>
        ///     优越的内存使用率指标
        /// </summary>
        [CustomerField("PredominantMemoryUsage")]
        public int PredominantMemoryUsage;
        /// <summary>
        ///     默认的通道群组层
        /// </summary>
        [CustomerField("DefaultChannelGroupLayer")]
        public int DefaultChannelGroupLayer;
        /// <summary>
        ///     默认可抛弃的队列数量
        /// </summary>
        [CustomerField("DefaultDecleardSize")]
        public int DefaultDecleardSize;
    }
}