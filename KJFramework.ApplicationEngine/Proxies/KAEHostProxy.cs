using System;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    KAE宿主于内部所有已上架APP的代理器
    /// </summary>
    internal class KAEHostProxy : MarshalByRefObject, IKAEHostProxy
    {
        /// <summary>
        ///     根据一个角色名和一个配置项的KEY名称来获取一个配置信息
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">配置信息的KEY</param>
        /// <returns>返回相应的配置信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field)
        {
            return SystemWorker.Instance.ConfigurationProxy.GetField(role, field);
        }
    }
}