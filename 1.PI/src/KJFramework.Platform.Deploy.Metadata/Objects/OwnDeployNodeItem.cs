using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    public class OwnDeployNodeItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置部署节点的机器名
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     获取或设置部署节点的地址
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string DeployAddress { get; set; }

        #endregion
    }
}