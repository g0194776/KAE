using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程目标可访问资源接口
    /// </summary>
    internal interface IProtocolResource
    {
        #region Members.

        /// <summary>
        ///    获取所需要关注的远程ZooKeeper路径
        /// </summary>
        string Path { get; }

        /// <summary>
        ///    获取所关注的业务通信协议
        /// </summary>
        Protocols Protocol { get; }

        /// <summary>
        ///    获取应用等级
        /// </summary>
        ApplicationLevel Level { get; }

        /// <summary>
        ///    获取所使用的协议类型
        /// </summary>
        ProtocolTypes ProtocolTypes { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///     获取内部的结果
        /// </summary>
        /// <returns>返回内部包含的数据</returns>
        IList<string> GetResult();
        /// <summary>
        ///     增加一个对当前业务协议感兴趣的KAE APP信息订阅者
        /// </summary>
        /// <param name="appUniqueId">KAE APP唯一编号</param>
        void RegisterInterestedApp(Guid appUniqueId);
        /// <summary>
        ///     减少一个对当前业务协议感兴趣的KAE APP信息订阅者
        /// </summary>
        /// <param name="appUniqueId">KAE APP唯一编号</param>
        void UnRegisterInterestedApp(Guid appUniqueId);
        /// <summary>
        ///    获取一个对当前远程资源感兴趣的KAE APP唯一编号列表
        /// </summary>
        /// <returns>返回内部包含的数据</returns>
        IEnumerable<Guid> GetInterestedApps();

        #endregion

        #region Events.

        /// <summary>
        ///    内部资源列表变更事件
        /// </summary>
        event EventHandler ChildrenChanged;

        #endregion
    }
}
