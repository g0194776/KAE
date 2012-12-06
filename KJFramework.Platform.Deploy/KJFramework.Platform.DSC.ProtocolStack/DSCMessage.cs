using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Platform.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     ����������Ϣ
    ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
    /// </summary>
    public class DSCMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ����������Ϣ
        ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
        /// </summary>
        public DSCMessage()
        {
            Header = new DSCMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public DSCMessageHeader Header { get; set; }

        #endregion
    }
}