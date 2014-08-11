using System;
using KJFramework.EventArgs;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程配置信息代理器元接口，提供了相关的基本操作
    /// </summary>
    public interface IRemoteConfigurationProxy
    {
        /// <summary>
        ///     获取某个字段的值
        ///     <para>* 使用此方法将会自动从核心配置表中读取数据</para>
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">字段名</param>
        /// <param name="callback">配置信息更新后的回调函数</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        string GetField(string role, string field, Action<string> callback = null);
        /// <summary>
        ///     获取部分配置信息
        /// </summary>
        /// <param name="configKey">配置文件KEY名称</param>
        /// <returns>如果不存在指定条件的配置文件，则返回null</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        string GetPartialConfig(string configKey);
        /// <summary>
        ///     获取指定表的数据
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        DataTable GetTable(string database, string table);        
        /// <summary>
        ///     获取指定表的数据
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="hasCache">缓存标识</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        DataTable GetTable(string database, string table, bool hasCache);
        /// <summary>
        ///     获取指定表的所有记录，并将每一个记录包装为指定类型的形式
        ///     <para>* 使用此方法只能从CSNDB中过去指定表的内容</para>
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        T[] GetTable<T>(string table) where T : class, new();
        /// <summary>
        ///     获取指定表的所有记录，并将每一个记录包装为指定类型的形式
        ///     <para>* 使用此方法只能从CSNDB中过去指定表的内容</para>
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="hasCache">缓存标识</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        T[] GetTable<T>(string table, bool hasCache) where T : class, new();
        /// <summary>
        ///    接收到了来自远程CSN的配置信息更新推送
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        void UpdateConfiguration(string key, string value);
        /// <summary>
        ///    如果收到了来自CSN的配置信息变更通知，则触发此事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> ConfigurationUpdated;
    }
}