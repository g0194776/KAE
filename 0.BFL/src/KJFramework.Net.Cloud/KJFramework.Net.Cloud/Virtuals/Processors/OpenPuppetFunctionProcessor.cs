namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     ���п�������ڵ㹦�ܵĿ��ܴ��������ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="TMessage">��Ϣ����</typeparam>
    public class OpenPuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
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
            _puppetNetworkNode.DOpen = delegate() { };
            return true;
        }

        /// <summary>
        ///     �ͷŵ�ǰ�Ŀ��ܹ��ܴ���
        /// </summary>
        public override void Release()
        {
            if (_puppetNetworkNode != null)
            {
                _puppetNetworkNode.DOpen = null;
            }
            _puppetNetworkNode = null;
        }

        #endregion
    }
}