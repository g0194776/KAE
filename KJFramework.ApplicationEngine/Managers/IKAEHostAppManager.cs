using System;
using KJFramework.ApplicationEngine.Objects;

namespace KJFramework.ApplicationEngine.Managers
{
    /// <summary>
    ///     KAE下宿主实例所持有的KPP管理器接口
    ///  </summary>
    internal interface IKAEHostAppManager
    {
        #region Methods.

        /// <summary>
        ///     注册一个APP实例到当前的管理器中
        /// </summary>
        /// <param name="app">APP实例</param>
        void RegisterApp(ApplicationDynamicObject app);
        /// <summary>
        ///     根据APP的UniqueId来获取一个内部所持有的APP实例
        /// </summary>
        /// <param name="guid">APP唯一编号</param>
        /// <returns>返回操作的结果</returns>
        ApplicationDynamicObject GetApp(Guid guid);
        /// <summary>
        ///     在内部移除一个APP实例
        /// </summary>
        /// <param name="guid">APP唯一编号</param>
        void Remove(Guid guid);
        /// <summary>
        ///    更新灰度升级策略的源代码
        /// </summary>
        /// <param name="code">灰度升级策略的源代码</param>
        void UpdateGreyPolicy(string code);
        /// <summary>
        ///    反向更新从CSN推送过来的KEY和VALUE配置信息
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        void UpdateConfiguration(string key, string value);

        #endregion
    }
}