namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     类型令牌
    /// </summary>
    internal unsafe struct TypeToken
    {
        /// <summary>
        ///     获取当前类型令牌的编号
        /// </summary>
        public ulong Id;
        /// <summary>
        ///     获取或设置一个值，该值表示了当前类型的数据是否在临时缓冲区内
        /// </summary>
        public bool IsTemporaryStore;
        /// <summary>
        ///     当前类型的数据存储位置
        /// </summary>
        public StorePositionData Positions;
        /// <summary>
        ///     获取或设置当前类型数据在临时缓冲区内的位置
        /// </summary>
        public fixed ushort Offsets [10];
    }
}