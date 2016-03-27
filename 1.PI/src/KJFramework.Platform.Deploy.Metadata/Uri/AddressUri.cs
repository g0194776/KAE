using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Uri
{
    /// <summary>
    ///     ��ַ·������
    /// </summary>
    public class AddressUri : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����õ�ַ�ؼ���
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string Key { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ֵַ
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string Value { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ַ����
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ�����õ�ַ���������ֶ�
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Tag { get; set; }

        #endregion
    }
}