using System;
namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     策略信息接口，提供了相关的基本属性。
    /// </summary>
    public interface IPolicyInfomation
    {
        /// <summary>
        ///     获取或设置名称
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     获取或设略版本
        /// </summary>
        String Version { get; set; }
        /// <summary>
        ///     获取或设置描述信息
        /// </summary>
        String Description { get; set; }
    }
}