namespace KJFramework.Messages.ValueStored.StoredHelper
{
    /// <summary>
    ///     值类型存储的动态生成抽象类
    /// </summary>
    public abstract class PropertyValueStored<T>
    {
        /// <summary>
        ///     获取设置的存储value值
        /// </summary>
        /// <param name="value">内部值</param>
        /// <typeparam name="K">内部值的真实类型</typeparam>
        /// <returns>返回转换后的内部值</returns>
        public abstract K Get<K>(T value);
    }
}
