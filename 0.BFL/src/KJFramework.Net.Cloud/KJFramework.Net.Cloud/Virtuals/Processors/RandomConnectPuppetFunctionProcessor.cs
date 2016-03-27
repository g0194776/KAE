using System;
using KJFramework.Net.Channels;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     具有连接功能的傀儡处理器，提供了相关的基本操作
    ///     <para>* 此傀儡处理器，总是根据一个随机数来决定返回的结果.</para>
    /// </summary>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public class RandomConnectPuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
    {
        #region Members

        private PuppetNetworkNode<TMessage> _puppetNetworkNode;
        private static Random _randown = new Random();

        #endregion

        #region Overrides of PuppetFunctionProcessor

        /// <summary>
        ///     初始化
        /// </summary>
        /// <typeparam name="T">宿主类型</typeparam>
        /// <param name="target">宿主对象</param>
        /// <returns>返回初始化的状态</returns>
        public override bool Initialize<T>(T target)
        {
            _puppetNetworkNode = (PuppetNetworkNode<TMessage>) ((object)target);
            _puppetNetworkNode.DConnect = delegate(ITransportChannel channel) { return _randown.Next(10) % 2 == 0; };
            return true;
        }

        /// <summary>
        ///     释放当前的傀儡功能处理
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