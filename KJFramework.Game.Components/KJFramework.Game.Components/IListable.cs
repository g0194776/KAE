namespace KJFramework.Game.Components
{
    /// <summary>
    ///     可提供链表样式元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">链表成员类型</typeparam>
    public interface IListable<T>
    {
        /// <summary>
        ///     下一个成员
        /// </summary>
        T Next { get; set; }
        /// <summary>
        ///     上一个成员
        /// </summary>
        T Previous { get; set; }
        /// <summary>
        ///     当前成员
        /// </summary>
        T Current { get; set; }
    }
}