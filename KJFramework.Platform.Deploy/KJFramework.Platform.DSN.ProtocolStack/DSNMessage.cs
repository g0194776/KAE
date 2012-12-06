using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.DSN.ProtocolStack
{
    /// <summary>
    ///     ����ڵ���Ϣ
    ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
    /// </summary>
    public class DSNMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ����ڵ���Ϣ
        ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
        /// </summary>
        public DSNMessage()
        {
            Header = new DSNMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public DSNMessageHeader Header { get; set; }

        #endregion
    }
}