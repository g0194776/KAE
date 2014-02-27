using KJFramework.Net.Channels.Identities;

namespace KJFramework.ServiceModel.Core.EventArgs
{
    /// <summary>
    ///     ��̬�ͻ��˴�����ײ������¼�����
    /// </summary>
    public class ClientLowProxyRequestEventArgs : System.EventArgs
    {
        #region Members

        /// <summary>
        ///     ��ȡ�����ûỰ���
        /// </summary>
        public TransactionIdentity Identity;
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        public int MethodToken;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ�Ϊ�첽����
        /// </summary>
        public bool IsAsync;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ���Ҫ����
        /// </summary>
        public bool NeedAck;
        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        public object[] Arguments;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�ĵ����Ƿ���Ҫ�ص�����
        /// </summary>
        public bool HasCallback;

        #endregion
    }
}