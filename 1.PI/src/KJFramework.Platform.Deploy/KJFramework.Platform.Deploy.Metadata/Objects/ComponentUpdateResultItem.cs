using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    public class ComponentUpdateResultItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     ��ȡ�����ø��½��
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public bool Result { get; set; }
        /// <summary>
        ///     ��ȡ�����ô�����Ϣ
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string ErrorTrace { get; set; }
        /// <summary>
        ///     ��ȡ���������������ʱ��
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }

        #endregion
    }
}