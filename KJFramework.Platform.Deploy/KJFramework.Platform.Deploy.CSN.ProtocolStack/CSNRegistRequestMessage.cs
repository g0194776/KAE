using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     CSNע��������Ϣ
    /// </summary>
    public class CSNRegistRequestMessage : CSNMessage
    {
        #region Constructor

        /// <summary>
        ///     CSNע��������Ϣ
        /// </summary>
        public CSNRegistRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ���Ҫ����������õ�����
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public bool NeedUpdate { get; set; }

        #endregion
    }
}