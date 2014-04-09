using System.Collections.Generic;
using System.IO;

namespace KJFramework.ApplicationEngine.Resources.Packs
{
    /// <summary>
    ///     KPP数据资源接口
    /// </summary>
    internal interface IKPPDataResource
    {
        #region Methods.

        /// <summary>
        ///    获取具有指定名称字段数据信息
        /// </summary>
        /// <typeparam name="T">字段数据信息类型</typeparam>
        /// <param name="name">字段名</param>
        /// <returns>返回指定名称所代表的数据信息</returns>
        /// <exception cref="KeyNotFoundException">指定的KEY不存在</exception>
        T GetField<T>(string name);
        /// <summary>
        ///    设置一个具有指定名称字段的数据信息
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">数据值</param>
        void SetField(string name, object value);
        /// <summary>
        ///    将一个目录下的所有文件打包为一个KPP资源包
        /// </summary>
        /// <param name="stream">资源流</param>
        void Pack(MemoryStream stream);
        /// <summary>
        ///    将一个目录下的PP资源包进行解包
        /// </summary>
        /// <param name="stream">资源流</param>
        void UnPack(MemoryStream stream);

        #endregion
    }
}