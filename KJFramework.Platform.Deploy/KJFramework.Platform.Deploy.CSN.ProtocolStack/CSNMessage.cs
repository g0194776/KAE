using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Platform.Deploy.CSN.Common;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ����վ�ڵ���Ϣ
    ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
    /// </summary>
    public class CSNMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ����ڵ���Ϣ
        ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
        /// </summary>
        public CSNMessage()
        {
            Header = new CSNMessageHeader();
            Header.SessionId = SessionGenerator.Create();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public CSNMessageHeader Header { get; set; }

        #endregion
    }
}