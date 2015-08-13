namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    KAE宿主接口
    /// </summary>
    public interface IKAEHost
    {
        #region Members.

        /// <summary>
        ///    获取内部运行的应用数量
        /// </summary>
        ushort ApplicationCount { get; }
        /// <summary>
        ///    获取工作目录
        /// </summary>
        string WorkRoot { get; }
        /// <summary>
        ///    获取当前KAE宿主实例的唯一名称
        /// </summary>
        string UniqueName { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        void Start();
        /// <summary>
        ///    开启KAE宿主
        /// </summary>
        void Stop();

        #endregion
    }
}