using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    public class OwnDeployNodeItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����ò���ڵ�Ļ�����
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     ��ȡ�����ò���ڵ�ĵ�ַ
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string DeployAddress { get; set; }

        #endregion
    }
}