namespace KJFramework.Basic.Enum
{
    /// <summary>
    ///     会话类型
    /// </summary>
    /// <remarks>
    ///     不用的会话类型保证了通讯间的信任与唯一性
    /// </remarks>
    public enum SessionType
    {
        /// <summary>
        ///     基础会话
        /// </summary>
        /// <remarks>
        ///     支持一切基本的通讯唯一安全。
        /// </remarks>
        Base,
        /// <summary>
        ///     文件传送会话
        /// </summary>
        /// <remarks>
        ///     支持一切文件传输的唯一安全。
        /// </remarks>
        FileTransfer,
        /// <summary>
        ///     视频传送会话
        /// </summary>
        /// <remarks>
        ///     支持了Peer2Peer的视频传输终结点唯一安全。
        /// </remarks>
        Video,
        /// <summary>
        ///     语音传送会话
        /// </summary>
        /// <remarks>
        ///     支持了Peer2Peer的语音传输终结点唯一安全。
        /// </remarks>
        Voice
    }
}
