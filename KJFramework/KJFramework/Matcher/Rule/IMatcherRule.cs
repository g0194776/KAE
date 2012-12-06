using System;
using System.Collections.Generic;

namespace KJFramework.Matcher.Rule
{
    /// <summary>
    ///     匹配器匹配规则元接口，提供了相关的基本操作
    /// </summary>
    public interface IMatcherRule : IEnumerable<String>
    {
        /// <summary>
        ///     匹配检查
        /// </summary>
        /// <param name="arg">要匹配的内容</param>
        /// <returns>返回匹配结果</returns>
        bool Check(String arg);
        /// <summary>
        ///     获取匹配项的数量
        /// </summary>
        int Count { get; }
    }
}