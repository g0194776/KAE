using System;
namespace KJFramework.Security
{
    /// <summary>
    ///     证书元接口，提供了相关的基本安全属性。
    /// </summary>
    public interface ICertificate
    {
        /// <summary>
        ///     获取或设置证书类别
        /// </summary>
        String Category { get; set; }
        /// <summary>
        ///     获取或设置证书授权人
        /// </summary>
        String Certigier { get; set; }
        /// <summary>
        ///     获取或设置证书初始化时间
        /// </summary>
        DateTime InitializeTime { get; set; }
        /// <summary>
        ///     获取或设置证书过期时间
        ///         * 如果过期时间为null, 则表示当前证书永不过期。
        /// </summary>
        DateTime? ExpiredTime { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值指示了当前证书是否已经过期了
        /// </summary>
        bool IsExpired { get; set; }
    }
}