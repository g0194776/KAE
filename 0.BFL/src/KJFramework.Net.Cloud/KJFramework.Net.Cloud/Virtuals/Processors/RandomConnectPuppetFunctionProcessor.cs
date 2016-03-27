using System;
using KJFramework.Net.Channels;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     �������ӹ��ܵĿ��ܴ��������ṩ����صĻ�������
    ///     <para>* �˿��ܴ����������Ǹ���һ����������������صĽ��.</para>
    /// </summary>
    /// <typeparam name="TMessage">��Ϣ����</typeparam>
    public class RandomConnectPuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
    {
        #region Members

        private PuppetNetworkNode<TMessage> _puppetNetworkNode;
        private static Random _randown = new Random();

        #endregion

        #region Overrides of PuppetFunctionProcessor

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="target">��������</param>
        /// <returns>���س�ʼ����״̬</returns>
        public override bool Initialize<T>(T target)
        {
            _puppetNetworkNode = (PuppetNetworkNode<TMessage>) ((object)target);
            _puppetNetworkNode.DConnect = delegate(ITransportChannel channel) { return _randown.Next(10) % 2 == 0; };
            return true;
        }

        /// <summary>
        ///     �ͷŵ�ǰ�Ŀ��ܹ��ܴ���
        /// </summary>
        public override void Release()
        {
            if (_puppetNetworkNode != null)
            {
                _puppetNetworkNode.DConnect = null;
            }
            _puppetNetworkNode = null;
        }

        #endregion
    }
}