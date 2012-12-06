namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     协议类型
    /// </summary>
    public enum ProtocolType
    {
        // 摘要:
        //     未知协议。
        Unknown = 0,
        //
        // 摘要:
        //     IPv6 逐跳选项头。
        IPv6HopByHopOptions = 1,
        //
        // 摘要:
        //     未指定的协议。
        Unspecified = 2,
        //
        // 摘要:
        //     网际协议。
        IP = 3,
        //
        // 摘要:
        //     网际消息控制协议。
        Icmp = 4,
        //
        // 摘要:
        //     网际组管理协议。
        Igmp = 5,
        //
        // 摘要:
        //     网关到网关协议。
        Ggp = 6,
        //
        // 摘要:
        //     Internet 协议版本 4。
        IPv4 = 7,
        //
        // 摘要:
        //     传输控制协议。
        Tcp = 8,
        //
        // 摘要:
        //     超文本传输协议。
        Http = 9,
        //
        // 摘要:
        //     PARC 通用数据包协议。
        Pup = 10,
        //
        // 摘要:
        //     用户数据报协议。
        Udp = 11,
        //
        // 摘要:
        //     Internet 数据报协议。
        Idp = 12,
        //
        // 摘要:
        //     Internet 协议版本 6 (IPv6)。
        IPv6 = 13,
        //
        // 摘要:
        //     IPv6 路由头。
        IPv6RoutingHeader = 14,
        //
        // 摘要:
        //     IPv6 片段头。
        IPv6FragmentHeader = 15,
        //
        // 摘要:
        //     IPv6 封装式安全措施负载头。
        IPSecEncapsulatingSecurityPayload = 16,
        //
        // 摘要:
        //     IPv6 身份验证头。有关详细信息，请参见位于 http://www.ietf.org 中的 RFC 2292 的 2.2.1 节。
        IPSecAuthenticationHeader = 17,
        //
        // 摘要:
        //     用于 IPv6 的 Internet 控制消息协议。
        IcmpV6 = 18,
        //
        // 摘要:
        //     IPv6 No Next 头。
        IPv6NoNextHeader = 19,
        //
        // 摘要:
        //     IPv6 目标选项头。
        IPv6DestinationOptions = 20,
        //
        // 摘要:
        //     网络磁盘协议（非正式）。
        ND = 21,
        //
        // 摘要:
        //     原始 IP 数据包协议。
        Raw = 22,
        //
        // 摘要:
        //     Internet 数据包交换协议。
        Ipx = 23,
        //
        // 摘要:
        //     顺序包交换协议。
        Spx = 24,
        //
        // 摘要:
        //     顺序包交换协议第 2 版。
        SpxII = 25,
    }
}
