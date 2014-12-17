using System.Diagnostics;

namespace KJFramework.Net.Transaction.Objects
{
    /// <summary>
    ///    KJFramework网络协议标准结构
    /// </summary>
    [DebuggerDisplay("P: {ProtocolId}, S: {ServiceId}, D: {DetailsId}")]
    public struct Protocols
    {
        /// <summary>
        ///    网络编号
        /// </summary>
        public int ProtocolId;
        /// <summary>
        ///    服务编号
        /// </summary>
        public int ServiceId;
        /// <summary>
        ///    详细服务编号
        /// </summary>
        public int DetailsId;
    }
}