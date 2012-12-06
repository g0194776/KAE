using System;
using System.Collections.Generic;

namespace KJFramework.Plugin
{
    /// <summary>
    ///     插件管理器元接口, 提供了相关的基本操作
    /// </summary>
    public interface IPluginManagement<TStoreType>
    {
        /// <summary>
        ///     获取指定目录中的，并且按照指定类型进行加载
        /// </summary>
        /// <param name="path" type="string">
        ///     <para>
        ///         目录
        ///     </para>
        /// </param>
        /// <param name="pluginType" type="System.Type">
        ///     <para>
        ///         类型
        ///     </para>
        /// </param>
        void Load(String path, Type pluginType);
        /// <summary>
        ///     按照全地址加载插件
        /// </summary>
        /// <param name="path" type="string">
        ///     <para>
        ///         插件的全地址
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回false,表示加载失败
        /// </returns>
        bool Add(String path);
        /// <summary>
        ///     按照类型加载插件
        /// </summary>
        /// <param name="pluginType" type="System.Type">
        ///     <para>
        ///         类型
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回false,表示加载失败
        /// </returns>
        bool Add(TStoreType pluginType);
        /// <summary>
        ///     卸载具有指定名称的插件
        /// </summary>
        /// <param name="name" type="string">
        ///     <para>
        ///         名称
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回false,表示卸载失败
        /// </returns>
        bool Remove(String name);
        /// <summary>
        ///     卸载具有指定ID的插件
        /// </summary>
        /// <param name="id" type="int">
        ///     <para>
        ///         id
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回false,表示卸载失败
        /// </returns>
        bool Remove(int id);
        /// <summary>
        ///     清除所有已加载的插件
        /// </summary>
        void Clear();
        /// <summary>
        ///     获取具有指定插件类别的插件链表
        /// </summary>
        /// <param name="catalogName" type="string">
        ///     <para>
        ///         插件类别
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null, 表示不存在当前插件类别
        /// </returns>
        List<TStoreType> GetPluginList(String catalogName);
        /// <summary>
        ///     获取当前插件数目
        /// </summary>
        int Count { get; }
        /// <summary>
        ///     返回当前插件集合的迭代器
        /// </summary>
        /// <returns>返回插件集合的迭代器</returns>
        IEnumerable<TStoreType> GetEnumerable();
    }
}