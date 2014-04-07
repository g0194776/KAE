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
        Metadata = 0x03
    }
}