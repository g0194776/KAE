using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     �����ж����ṩ����صĻ������Խṹ
    /// </summary>
    public class DataRow : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ��������ֵ
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public DataColumn[] Columns { get; set; }

        #endregion
    }
}