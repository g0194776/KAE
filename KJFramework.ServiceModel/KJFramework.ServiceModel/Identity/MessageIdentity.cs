namespace KJFramework.ServiceModel.Identity
{
    /// <summary>
    ///     消息唯一标示
    /// </summary>
    public struct MessageIdentity
    {
        /// <summary>
        ///     获取或设置协议编号
        /// </summary>
        public byte ProtocolId { get; set; }
        /// <summary>
        ///     获取或设置服务编号
        /// </summary>
        public byte ServiceId { get; set; }
        /// <summary>
        ///     获取或设置详细分类编号
        /// </summary>
        public byte DetailsId { get; set; }
    }
}