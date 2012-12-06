namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     可兼容对象元接口， 提供了相关的基本操作。
    /// </summary>
    public interface ICompatibleObject
    {
        /// <summary>
        ///     获取一个值，该值标示了当前是否存在未知参数。
        /// </summary>
        bool HasParameter { get; }
        /// <summary>
        ///     获取具有指定编号的未知参数
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回未知参数</returns>
        IUnknownParameter GetParameter(int id);
        /// <summary>
        ///     获取所有未知参数
        /// </summary>
        /// <returns>返回未知参数集合</returns>
        IUnknownParameter[] GetParameters();
    }
}