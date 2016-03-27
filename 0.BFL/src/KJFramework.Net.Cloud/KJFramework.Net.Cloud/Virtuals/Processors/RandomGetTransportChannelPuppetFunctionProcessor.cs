using System;
using KJFramework.Net.Cloud.Virtuals.Channels;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     ���л�ȡ����ͨ�����ܵĿ��ܴ��������ṩ����صĻ�������
    ///     <para>* �˿��ܴ����������Ƿ��ظ���һ����������������صĽ��.</para>
    /// </summary>
    /// <typeparam name="TMessage">��Ϣ����</typeparam>
    public class RandomGetTransportChannelPuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
    {
        #region Members

        private PuppetNetworkNode<TMessage> _puppetNetworkNode;
        private static Random _random = new Random();

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
            _puppetNetworkNode.DGetTransportChannel = delegate(Guid id)
                                                          {
                                                              return _random.Next(10) % 2 == 0
                                                                         ? new PuppetTransportChannel()
                                                                         : null;
                                                          };
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