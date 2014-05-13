using KJFramework.Enums;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Performances
{
    /// <summary>
    ///     ������������
    /// </summary>
    public class ComponentHealthItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ�����ý���״̬
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public HealthStatus Status { get; set; }

        #endregion
    }
}