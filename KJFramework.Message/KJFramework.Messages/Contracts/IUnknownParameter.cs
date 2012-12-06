namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     未知参数元接口，提供了相关的基本操作。
    /// </summary>
    public interface IUnknownParameter
    {
        /// <summary>
        ///     获取参数编号
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     获取参数元数据
        /// </summary>
        byte[] Content { get; }
    }
}