using System;
using KJFramework.Net.Cloud.Virtuals.Channels;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     ���л�ȡ����ͨ�����ܵĿ��ܴ��������ṩ����صĻ�������
    ///     <para>* �˿��ܴ����������Ƿ���һ�����ܴ���ͨ��.</para>
    /// </summary>
    /// <typeparam name="TMessage">��Ϣ����</typeparam>
    public class GetTransportChannelPuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
    {
        #region Members

        private PuppetNetworkNode<TMessage> _puppetNetworkNode;

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
            _puppetNetworkNode.DGetTransportChannel = delegate(Guid id) { return new PuppetTransportChannel(); };
            return true;
        }

        /// <summary>
        ///     �ͷŵ�ǰ�Ŀ��ܹ��ܴ���
        /// </summary>
        public override void Release()
        {
            if (_puppetNetworkNode != null)
            {
                _puppetNetworkNode.DGetTransportChannel = null;
            }
            _puppetNetworkNode = null;
        }

        #endregion
    }
}