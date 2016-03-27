using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     描述参数接口，提供了相关的基本属性结构
    /// </summary>
    public interface IDescriptionArgument : IDisposable
    {
        /// <summary>
        ///     获取或设置参数顺序编号
        /// </summary>
        int Id { get; set; }
        /// <summary>
        ///     获取或设置参数全名称
        /// </summary>
        String FullName { get; set; }
        /// <summary>
        ///     获取或设置参数名称
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     获取或设置一个标示，表示当前参数是否可以为空
        /// </summary>
        bool CanNull { get; set; }
        /// <summary>
        ///     获取或设置参数类型
        /// </summary>
        Type ParameterType { get; set; }
    }
}