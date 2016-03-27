using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Services
{
    /// <summary>
    ///     ��̬������Ϣ
    ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
    /// </summary>
    public class DynamicServiceMessage : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ��̬������Ϣ
        ///     <para>* ������������������Ա�Ŵ�10��ʼʹ�á�</para>
        /// </summary>
        public DynamicServiceMessage()
        {
            Header = new DynamicServiceMessageHeader();
        }

        #endregion

        #region Members

        [IntellectProperty(0, IsRequire = true)]
        public DynamicServiceMessageHeader Header { get; set; }

        #endregion
    }
}