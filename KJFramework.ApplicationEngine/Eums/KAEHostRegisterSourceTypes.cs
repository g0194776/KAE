namespace KJFramework.ApplicationEngine.Eums
{
    /// <summary>
    ///    KAE宿主的注册来源
    /// </summary>
    public enum KAEHostRegisterSourceTypes : byte
    {
        /// <summary>
        ///     通过服务直接TCP链接的注册
        ///     <para>* 如果是通过此方式进行注册的KAE HOST，在KAE HOST跟RRCS的长连接断开后，RRCS内部就会立刻清除KAE HOST所当初注册的所有协议信息</para>
        /// </summary>
        Service = 0x00,
        /// <summary>
        ///     通过RRCS API方式进行的注册
        ///     <para>* 如果是通过此方式进行注册的KAE HOST，在KAE HOST跟RRCS的长连接断开后，RRCS内部不会立刻清除KAE HOST所当初注册的所有协议信息，
        ///     而是等到当KAE HOST把注册时RRCS分配给它的GUID传入一个注销接口时，才会清除当时注册的所有协议信息</para>
        /// </summary>
        WebAPI = 0x01
    }
}