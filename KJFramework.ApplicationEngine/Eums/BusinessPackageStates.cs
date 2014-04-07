namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///    业务包裹状态枚举
    /// </summary>
    public enum BusinessPackageStates : byte
    {
        /// <summary>
        ///    KAE宿主接收到了外部的网络REQ
        /// </summary>
        ReceivedOutsideRequest = 0x00,
        /// <summary>
        ///    KAE宿主已经将外部REQ转发到了内部的指定应用上
        /// </summary>
        Delivered = 0x01,
        /// <summary>
        ///    内部应用已经接受到了KAE宿主所转发的REQ
        /// </summary>
        ReceiveDeliveryRequest = 0x02,
        /// <summary>
        ///    内部应用已给KAE宿主发送RSP消息
        /// </summary>
        SentDeliveryResponse = 0x03,
        /// <summary>
        ///    应用已发送RSP消息给KAE宿主
        /// </summary>
        ReceivedDeliveryResponse = 0x04,
        /// <summary>
        ///     已完成KAE宿主与应用的通信圆环
        /// </summary>
        End = 0x05,
        /// <summary>
        ///    异常状态
        /// </summary>
        Error = 0x06
    }
}