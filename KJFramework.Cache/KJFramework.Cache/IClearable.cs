namespace KJFramework.Cache
{
    /// <summary>
    ///     可清除对象元接口，提供了相关的基本操作
    /// </summary>
    public interface IClearable
    {
        /// <summary>
        ///     清除对象自身
        /// </summary>
        void Clear();
    }
}