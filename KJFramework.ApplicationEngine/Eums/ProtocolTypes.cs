namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///    协议类型
    /// </summary>
    public enum ProtocolTypes : byte
    {
        /// <summary>
        ///    JSON格式
        /// </summary>
        Json = 0x00,
        /// <summary>
        ///    XML格式
        /// </summary>
        Xml = 0x01,
        /// <summary>
        ///    智能对象格式
        /// </summary>
        Intellegence = 0x02,
        /// <summary>
        ///    元数据插槽格式
        /// </summary>
        Metadata = 0x03,
        /// <summary>
        ///    留给KAE宿主自己的，基于元数据插槽格式的网络通信协议
        /// </summary>
        INTERNAL_SPECIAL_RESOURCE = 0xFF
    }
}