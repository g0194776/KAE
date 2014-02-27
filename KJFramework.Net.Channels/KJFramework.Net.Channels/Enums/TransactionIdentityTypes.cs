namespace KJFramework.Net.Channels.Enums
{
    /// <summary>
    ///   事务唯一标示类型
    /// </summary>
    public enum TransactionIdentityTypes : byte
    {
        /// <summary>
        ///   TCP事务唯一标示
        /// </summary>
        TCP = 0x00,
        /// <summary>
        ///   UDP事务唯一标示
        /// </summary>
        UDP = 0x01,
        /// <summary>
        ///   NamedPipe事务唯一标示
        /// </summary>
        NamedPipe = 0x02,
        /// <summary>
        ///   HTTP事务唯一标示
        /// </summary>
        HTTP = 0x03,
        /// <summary>
        ///   未知
        /// </summary>
        Unknown = 0x04
    }
}