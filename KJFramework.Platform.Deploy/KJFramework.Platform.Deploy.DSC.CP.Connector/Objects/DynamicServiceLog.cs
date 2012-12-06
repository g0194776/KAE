using System;
using System.Collections.Generic;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Objects
{
    public class DynamicServiceLog
    {
        #region Constructor

        public DynamicServiceLog()
        {
            Componnets = new Dictionary<string, OwnComponentItem>();
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置组件个数
        /// </summary>
        public int ComponentCount { get; set; }
        /// <summary>
        ///     获取或设置组件详情
        /// </summary>
        public Dictionary<string, OwnComponentItem> Componnets { get; set; }
        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     获取或设置最后心跳时间
        /// </summary>
        public DateTime LastHeartbeatTime { get; set; }
        /// <summary>
        ///     获取或设置控制服务地址
        /// </summary>
        public string ControlServiceAddress { get; set; }
        /// <summary>
        ///     获取或设置外壳版本号
        /// </summary>
        public string ShellVersion { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     更新组件的健康状态
        /// </summary>
        /// <param name="responseMessage">获取组件健康状态回馈信息包</param>
        internal void Update(DSCGetComponentHealthResponseMessage responseMessage)
        {
            if (responseMessage.Items != null && Componnets != null)
            {
                foreach (ComponentHealthItem healthItem in responseMessage.Items)
                {
                    OwnComponentItem item;
                    if (Componnets.TryGetValue(healthItem.ComponentName, out item))
                    {
                        item.Status = healthItem.Status;
                    }
                }
            }
        }

        #endregion
    }
}