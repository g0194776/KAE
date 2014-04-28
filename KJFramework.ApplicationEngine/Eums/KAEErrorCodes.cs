namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///     KAE通用错误码枚举
    /// </summary>
    public enum KAEErrorCodes : byte
    {
        OK = 0x00,
        TunnelCommunicationTimeout = 0xFA,
        TunnelCommunicationFailed = 0xFB,
        NotSupportedApplicationLevel = 0xFC,
        NotSupportedMessageIdentity = 0xFD,
        NotSupportedNetworkType = 0xFE,
        Unknown = 0xFF
    }
}