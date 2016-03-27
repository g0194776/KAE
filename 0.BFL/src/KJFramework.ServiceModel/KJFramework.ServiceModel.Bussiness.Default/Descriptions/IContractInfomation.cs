using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     契约信息元接口，提供了相关的属性结构
    /// </summary>
    public interface IContractInfomation
    {
        /// <summary>
        ///     获取或设置契约开放时间
        /// </summary>
        DateTime OpenTime { get; set; }
        /// <summary>
        ///     获取或设置契约名称
        /// </summary>
        String ContractName { get; set; }
        /// <summary>
        ///     获取或设置契约别名
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     获取或设置契约全名称
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        ///     获取或设置契约描述
        /// </summary>
        String Description { get; set; }
        /// <summary>
        ///     获取或设置契约版本
        /// </summary>
        String Version { get; set; }
    }
}