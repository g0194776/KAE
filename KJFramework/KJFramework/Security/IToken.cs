namespace KJFramework.Security
{
    /// <summary>
    ///     令牌元接口，提供了相关的属性结构。
    /// </summary>
    public interface IToken
    {
        /// <summary>
        ///     获取令牌内容
        /// </summary>
        byte[] Content { get; }
    }
}