using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Objects;

namespace KJFramework.ApplicationEngine.Managers
{
    /// <summary>
    ///     KAE下宿主实例所持有的KPP管理器
    /// </summary>
    internal class KAEHostAppManager : IKAEHostAppManager
    {
        #region Members.

        private readonly object _activedAppsLockObj = new object();
        private readonly IDictionary<Guid, ApplicationDynamicObject> _activedApps = new Dictionary<Guid, ApplicationDynamicObject>();

        #endregion

        #region Methods.

        /// <summary>
        ///     注册一个APP实例到当前的管理器中
        /// </summary>
        /// <param name="app">APP实例</param>
        public void RegisterApp(ApplicationDynamicObject app)
        {
            lock (_activedAppsLockObj) _activedApps[app.GlobalUniqueId] = app;
        }

        /// <summary>
        ///     根据APP的UniqueId来获取一个内部所持有的APP实例
        /// </summary>
        /// <param name="guid">APP唯一编号</param>
        /// <returns>返回操作的结果</returns>
        public ApplicationDynamicObject GetApp(Guid guid)
        {
            ApplicationDynamicObject app;
            lock (_activedAppsLockObj)
                return (_activedApps.TryGetValue(guid, out app) ? app : null);
        }

        /// <summary>
        ///     在内部移除一个APP实例
        /// </summary>
        /// <param name="guid">APP唯一编号</param>
        public void Remove(Guid guid)
        {
            lock (_activedAppsLockObj) _activedApps.Remove(guid);
        }

        /// <summary>
        ///    更新灰度升级策略的源代码
        /// </summary>
        /// <param name="code">灰度升级策略的源代码</param>
        public void UpdateGreyPolicy(string code)
        {
            lock (_activedAppsLockObj)
                foreach (KeyValuePair<Guid, ApplicationDynamicObject> pair in _activedApps) pair.Value.UpdateGreyPolicy(code);
        }

        /// <summary>
        ///    反向更新从CSN推送过来的KEY和VALUE配置信息
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void UpdateConfiguration(string key, string value)
        {
            lock (_activedAppsLockObj)
                foreach (KeyValuePair<Guid, ApplicationDynamicObject> app in _activedApps)
                    app.Value.UpdateConfiguration(key, value);
        }

        #endregion
    }
}