using System;

namespace KJFramework.Security
{
    /// <summary>
    ///     授权元接口，提供了相关的基本属性结构
    /// </summary>
    public interface IAuthorization
    {
        /// <summary>
        ///     获取授权人
        /// </summary>
        String Certigier { get; }
        /// <summary>
        ///     获取授权类别
        /// </summary>
        String Category { get; }
        /// <summary>
        ///     获取授权日期
        /// </summary>
        DateTime Time { get; }
    }
}