using System;

namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///    协议类型
    /// </summary>
    [Flags]
    public enum ProtocolTypes : byte
    {
        /// <summary>
        ///    JSON格式
        /// </summary>
        Json = 0x01,
        /// <summary>
        ///    XML格式
        /// </summary>
        Xml = 0x02,
        /// <summary>
        ///    智能对象格式
        /// </summary>
        Intellegence = 0x04,
        /// <summary>
        ///    元数据插槽格式
        /// </summary>
        Metadata = 0x08,
        /// <summary>
        ///    留给KAE宿主自己的，基于元数据插槽格式的网络通信协议
        /// </summary>
        INTERNAL_SPECIAL_RESOURCE = 0x0F,
        /// <summary>
        ///     所有协议
        /// </summary>
        All = 0xFF
    }
}