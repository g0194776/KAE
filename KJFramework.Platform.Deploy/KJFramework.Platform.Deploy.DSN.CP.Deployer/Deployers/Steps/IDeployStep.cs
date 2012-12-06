using System;
using KJFramework.Platform.Deploy.DSN.CP.Deployer.Reporters;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Deployers.Steps
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDeployStep
    {
        /// <summary>
        ///     ִ�в���
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="args">��ز���</param>
        /// <returns>����ִ�еĽ��</returns>
        bool Execute(out Object[] context, params Object[] args);
        /// <summary>
        ///     ��ȡִ�в��赱�г��ֵ��쳣
        /// </summary>
        System.Exception Exception { get; }
        /// <summary>
        ///     ��ȡ�����ò���״̬�㱨��
        ///     <para>* �����Ի��ɲ�����ͳһע��</para>
        /// </summary>
        IDeployStatusReporter Reporter { get; set; }
    }
}