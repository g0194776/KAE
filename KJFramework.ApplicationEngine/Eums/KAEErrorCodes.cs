namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///     KAE通用错误码枚举
    /// </summary>
    public enum KAEErrorCodes : byte
    {
        OK = 0x00,
        SpecifiedKAEHostHasNoAnyApplication = 0x01,
        CommunicaitonFailedWithAPMS = 0x02,
        NullResultWithTargetedAppCRC = 0x03,
        IllegalSupportedInformation = 0x04,
        IllegalArgument = 0xF9,
        TunnelCommunicationTimeout = 0xFA,
        TunnelCommunicationFailed = 0xFB,
        NotSupportedApplicationLevel = 0xFC,
        NotSupportedMessageIdentity = 0xFD,
        NotSupportedNetworkType = 0xFE,
        SpecifiedKPPNotFound = 0xE9,
        Unknown = 0xFF
    }
}