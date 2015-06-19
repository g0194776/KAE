using System.Collections.Generic;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程目标可访问资源接口
    /// </summary>
    internal interface IProtocolResource
    {
        #region Methods.

        /// <summary>
        ///     获取内部的结果
        /// </summary>
        /// <returns></returns>
        IList<string> GetResult();
        /// <summary>
        ///     增加一个对当前业务协议感兴趣的KAE APP信息订阅者
        /// </summary>
        /// <param name="appUniqueId">KAE APP唯一编号</param>
        void RegisterInterestedApp(System.Guid appUniqueId);

        #endregion
    }
}
