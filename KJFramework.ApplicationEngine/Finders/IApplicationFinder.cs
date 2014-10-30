using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Resources;

namespace KJFramework.ApplicationEngine.Finders
{
    /// <summary>
    ///    应用查找器接口
    /// </summary>
    public interface IApplicationFinder
    {
        /// <summary>
        ///    从一个指定的路径中寻找合法的KPP包
        ///     <para>* Dic.Key = PackName</para>
        /// </summary>
        /// <param name="path">寻找的路径</param>
        /// <returns>返回找到的KPP资源集合</returns>
        IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> Search(string path);
    }
}