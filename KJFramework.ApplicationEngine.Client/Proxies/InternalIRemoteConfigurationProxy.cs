using System;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.EventArgs;

namespace KJFramework.ApplicationEngine.Client.Proxies
{
    public class InternalIRemoteConfigurationProxy : IRemoteConfigurationProxy
    {
        #region Methods.

        /// <summary>
        ///     获取某个字段的值
        ///     <para>* 使用此方法将会自动从核心配置表中读取数据</para>
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">字段名</param>
        /// <param name="callback">配置信息更新后的回调函数</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field, Action<string> callback = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取部分配置信息
        /// </summary>
        /// <param name="configKey">配置文件KEY名称</param>
        /// <returns>如果不存在指定条件的配置文件，则返回null</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetPartialConfig(string configKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///    接收到了来自远程CSN的配置信息更新推送
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void UpdateConfiguration(string key, string value)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> ConfigurationUpdated;

        #endregion
    }
}