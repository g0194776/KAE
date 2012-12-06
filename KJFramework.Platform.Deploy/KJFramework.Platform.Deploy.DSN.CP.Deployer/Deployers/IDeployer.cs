using KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDeployer
    {
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        string RequestToken { get; }
        /// <summary>
        ///     ��ȡ״̬�㱨��
        /// </summary>
        IDeployStatusReporter Reporter { get; }
        /// <summary>
        ///     ����һ��������
        /// </summary>
        /// <param name="deployStep">������</param>
        void Add(IDeployStep deployStep);
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="args">����</param>
        /// <returns>���ز���Ľ��</returns>
        bool Deploy(params object[] args);
        /// <summary>
        ///     ��ȡ�����г��ֵ����һ���쳣
        /// </summary>
        System.Exception LastException { get; }
    }
}