using System;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters
{
    /// <summary>
    ///     ����״̬�㱨��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDeployStatusReporter
    {
        /// <summary>
        ///     ��ȡ��˻㱨���������ͨ�����
        /// </summary>
        Guid ChannelId { get; }
        /// <summary>
        ///     ��ȡ��˻㱨�����������������
        /// </summary>
        string RequestToken { get; }
        /// <summary>
        ///     ��һ��״̬֪ͨ��Զ���ս��
        /// </summary>
        /// <param name="content">״̬��Ϣ</param>
        /// <exception cref="System.Exception">֪ͨʧ��</exception>
        void Notify(string content);
        /// <summary>
        ///     ��һ��������Ϣ֪ͨ��Զ���ս��
        /// </summary>
        /// <param name="exception">�쳣</param>
        /// <exception cref="System.Exception">֪ͨʧ��</exception>
        void Notify(System.Exception exception);
    }
}