using KJFramework.Net.Exception;
using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     ����վ�ڵ�Э��ջ���ṩ����صĻ���������
    /// </summary>
    public class CSNProtocolStack : BusinessProtocolStack
    {
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>���س�ʼ���Ľ��</returns>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        public override bool Initialize()
        {
            return true;
        }
    }
}