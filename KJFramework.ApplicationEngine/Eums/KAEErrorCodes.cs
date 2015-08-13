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
        /// <summary>
        ///     错误参数
        /// </summary>
        IllegalArgument = 0xF9,
        /// <summary>
        ///     KPP隧道通信超时
        /// </summary>
        TunnelCommunicationTimeout = 0xFA,
        /// <summary>
        ///     KPP隧道通信失败
        /// </summary>
        TunnelCommunicationFailed = 0xFB,
        /// <summary>
        ///     不支持的KPP应用等级
        /// </summary>
        NotSupportedApplicationLevel = 0xFC,
        /// <summary>
        ///     不支持的消息协议
        /// </summary>
        NotSupportedMessageIdentity = 0xFD,
        /// <summary>
        ///     不支持的网络类型
        /// </summary>
        NotSupportedNetworkType = 0xFE,
        /// <summary>
        ///     在当前KAE宿主中找不到具备指定KPP唯一编号的KPP实例
        /// </summary>
        SpecifiedKPPNotFound = 0xE9,
        /// <summary>
        ///     远程的KPP下载地址无法访问
        /// </summary>
        RemoteKPPAddressUnAcccessable = 0xEA,
        /// <summary>
        ///     下载远程KPP时失败
        /// </summary>
        DownloadKPPFailed = 0xEB,
        /// <summary>
        ///     目标KPP已经在指定的KAE宿主中上架
        /// </summary>
        KPPAlreadyInstalled = 0xEC,
        /// <summary>
        ///    未知系统错误
        /// </summary>
        Unknown = 0xFF
    }
}