using System;
using System.Collections.Generic;
using System.Xml;
using KJFramework.Matcher.Rule;

namespace KJFramework.Matcher
{
    /// <summary>
    ///     匹配器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IMatcher
    {
        /// <summary>
        ///     根据一个文件进行匹配操作
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="path">文件全路径</param>
        /// <param name="rule">匹配规则</param>
        /// <returns>返回匹配后的结果</returns>
        T Match<T>(String path, IMatcherRule rule);
        /// <summary>
        ///     匹配操作
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <param name="tag">附属条件</param>
        /// <param name="rule">匹配规则</param>
        /// <returns>返回匹配后的一个字典集合</returns>
        Dictionary<String, XmlNode> Match(String path, String tag, IMatcherRule rule);
    }
}