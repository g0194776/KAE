namespace KJFramework.ServiceModel.Metadata
{
    /// <summary>
    ///     二进制参数上下文元接口，提供了相关的基本操作。
    /// </summary>
    public interface IBinaryArgContext
    {
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的参数是否存在未抛出的异常
        /// </summary>
        bool HasException { get; set; }
        /// <summary>
        ///     获取或设置异常信息
        /// </summary>
        System.Exception Exception { get; set; }
        /// <summary>
        ///     获取或设置参数唯一编号
        /// </summary>
        byte Id { get; set; }
        /// <summary>
        ///     获取或设置二进制参数上下文元数据
        /// </summary>
        byte[] Data { get; set; }
    }
}