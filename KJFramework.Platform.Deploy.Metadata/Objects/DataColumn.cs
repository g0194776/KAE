using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     �����ж����ṩ����صĻ������Խṹ
    /// </summary>
    public class DataColumn : IntellectObject
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����������е�ֵ
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Value { get; set; }
        ///// <summary>
        /////     ��ȡ�������б��
        /////     <para>* �˱�Ž��ᰴ�����ݿ��е��ֶ�˳�����ж�Ӧ��</para>
        ///// </summary>
        //[IntellectProperty(1, IsRequire = true)]
        //public short Id { get; set; }
       
        #endregion
    }
}