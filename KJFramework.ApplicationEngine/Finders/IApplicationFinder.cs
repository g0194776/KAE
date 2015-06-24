using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Objects;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Exception;

namespace KJFramework.ApplicationEngine.Finders
{
    /// <summary>
    ///    应用查找器接口
    /// </summary>
    public interface IApplicationFinder
    {
        #region Methods.

        /// <summary>
        ///    从一个指定的路径中寻找合法的KPP包
        ///     <para>* Dic.Key = PackName</para>
        /// </summary>
        /// <param name="path">寻找的路径</param>
        /// <returns>返回找到的KPP资源集合</returns>
        IDictionary<string, IList<Tuple<ApplicationEntryInfo, KPPDataStructure>>> Search(string path);
        /// <summary>
        ///     读取通知KAE APP所在地址的文件，并将其进行解析
        /// </summary>
        /// <param name="fileFullPath">KAE APP文件的完整路径地址</param>
        /// <returns>返回解析后的KAE APP信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="FileNotFoundException">目标文件不存在</exception>
        Tuple<string, ApplicationEntryInfo, KPPDataStructure> ReadKPPFrom(string fileFullPath);

        #endregion
    }
}