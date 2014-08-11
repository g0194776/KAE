using KJFramework.Attribute;

namespace KJFramework.Platform.Deploy.CSN.Common.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModelÅäÖÃÏî
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   CSN¿ª·ÅµÄ¶Ë¿ÚºÅ
        /// </summary>
        [CustomerField("PublisherPort")]
        public int PublisherPort;
        /// <summary>
        ///   CSN¿ª·ÅµÄ¶Ë¿ÚºÅ
        /// </summary>
        [CustomerField("CommandPort")]
        public int CommandPort;
        /// <summary>
        ///   CSN¿ª·ÅµÄ¶Ë¿ÚºÅ
        /// </summary>
        [CustomerField("HostPort")]
        public int HostPort;
        /// <summary>
        ///    CSN服务内部配置信息更新后的数据发布者端口
        /// </summary>
        [CustomerField("UpdatingPublisher")]
        public int UpdatingPublisher;
        /// <summary>
        ///     ·þÎñÖÐÐÄµÄµØÖ·
        /// </summary>
        [CustomerField("CenterAddress")]
        public string CenterAddress;
        /// <summary>
        ///     ·þÎñÖÐÐÄµÄ¶Ë¿Ú
        /// </summary>
        [CustomerField("CenterPort")]
        public int CenterPort;
        /// <summary>
        ///     ×¢²á³¬Ê±Ê±¼ä
        /// </summary>
        [CustomerField("RegistTimeout")]
        public int RegistTimeout;
        /// <summary>
        ///     ÐÄÌø¼ä¸ô
        /// </summary>
        [CustomerField("HeartBeatInterval")]
        public int HeartBeatInterval;
        /// <summary>
        ///     ÖØÁ¬Ê±¼ä¼ä¸ô
        /// </summary>
        [CustomerField("ReconnectTimeout")]
        public int ReconnectTimeout;
        /// <summary>
        ///     »º´æ¶ÔÏó³¬Ê±¼ì²é¼ä¸ô
        /// </summary>
        [CustomerField("CacheTimeoutCheckInterval")]
        public int CacheTimeoutCheckInterval;
        /// <summary>
        ///     »º´æ¶ÔÏó´æ»îÊ±¼ä
        /// </summary>
        [CustomerField("CacheLiveTime")]
        public string CacheLiveTime;
        /// <summary>
        ///     ×î´óÊý¾Ý¶Î´óÐ¡
        ///     <para>* ´Ë×Ö¶ÎÓÃÓÚ·Ö°üÊý¾ÝµÄ´óÐ¡µÄÅÐ¶Ï</para>
        /// </summary>
        [CustomerField("MaxDataChunkSize")]
        public int MaxDataChunkSize;
    }
}