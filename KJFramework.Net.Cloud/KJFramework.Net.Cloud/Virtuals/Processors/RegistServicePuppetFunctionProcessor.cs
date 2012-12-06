using System;
using KJFramework.ServiceModel.Elements;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     ����ע������ܵĿ��ܴ��������ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="TMessage">��Ϣ����</typeparam>
    public class RegistServicePuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
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
            _puppetNetworkNode.DRegistService = delegate(Binding binding, Type type) { };
            return true;
        }

        /// <summary>
        ///     �ͷŵ�ǰ�Ŀ��ܹ��ܴ���
        /// </summary>
        public override void Release()
        {
            if (_puppetNetworkNode != null)
            {
                _puppetNetworkNode.DRegistService = null;
            }
            _puppetNetworkNode = null;
        }

        #endregion
    }
}