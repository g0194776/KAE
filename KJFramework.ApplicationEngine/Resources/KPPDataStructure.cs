using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.ApplicationEngine.Resources.Packs;

namespace KJFramework.ApplicationEngine.Resources
{
    /// <summary>
    ///    KPP文件数据格式
    /// </summary>
    internal class KPPDataStructure : MarshalByRefObject
    {
        #region Constructor.

        /// <summary>
        ///    KPP文件数据格式
        /// </summary>
        /// <param name="head">KPP资源文件头</param>
        /// <param name="sections">KPP数据节集合</param>
        /// <param name="files">KPP资源包内部所包含的文件集合</param>
        public KPPDataStructure(KPPDataHead head, IDictionary<byte, IKPPDataResource> sections, IDictionary<string, byte[]> files)
        {
            _head = head;
            _sections = sections;
            _files = files;
        }

        #endregion

        #region Members.

        private readonly KPPDataHead _head;
        private readonly IDictionary<string, byte[]> _files;
        private readonly IDictionary<byte, IKPPDataResource> _sections;

        #endregion

        #region Methods.

        /// <summary>
        ///    在数据头中获取具有指定名称字段数据信息
        /// </summary>
        /// <typeparam name="T">字段数据信息类型</typeparam>
        /// <param name="name">字段名</param>
        /// <returns>返回指定名称所代表的数据信息</returns>
        /// <exception cref="KeyNotFoundException">指定的KEY不存在</exception>
        public T GetHeadField<T>(string name)
        {
            return _head.GetField<T>(name);
        }

        /// <summary>
        ///    在指定编号的数据节中获取具有指定名称字段数据信息
        /// </summary>
        /// <typeparam name="T">字段数据信息类型</typeparam>
        /// <param name="sectionId">数据节编号</param>
        /// <param name="name">字段名</param>
        /// <returns>返回指定名称所代表的数据信息</returns>
        /// <exception cref="KeyNotFoundException">指定的KEY不存在</exception>
        public T GetSectionField<T>(byte sectionId, string name)
        {
            return _sections[sectionId].GetField<T>(name);
        }

        /// <summary>
        ///    将KPP资源包中的文件释放到指定的目录中
        /// </summary>
        /// <param name="path">释放到的目录</param>
        public void ReleaseFiles(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path);
            Directory.CreateDirectory(path);
            //...
        }


        #endregion
    }
}