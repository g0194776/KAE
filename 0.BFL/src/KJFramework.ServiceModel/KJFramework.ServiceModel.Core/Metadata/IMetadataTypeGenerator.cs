using System;
using System.Collections.Generic;

namespace KJFramework.ServiceModel.Core.Metadata
{
    /// <summary>
    ///     元数据类型生成器元接口，提供了相关的基本操作
    /// </summary>
    public interface IMetadataTypeGenerator
    {
        /// <summary>
        ///     为一个指定类型生成元数据描述(XML)
        /// </summary>
        /// <param name="type">需要生成元数据的类型</param>
        /// <returns>返回描述信息(XML)</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        Dictionary<string, string> Generate(Type type);
    }
}