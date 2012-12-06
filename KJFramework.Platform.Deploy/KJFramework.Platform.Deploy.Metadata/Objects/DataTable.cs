using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     ���ݱ�����ṩ����صĻ������Խṹ
    /// </summary>
    public class DataTable : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ������������
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public DataRow[] Rows { get; set; }
        /// <summary>
        ///     ��ȡ��������������
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string[] Columns { get; set; }

        #endregion
    }
}