using System;
using KJFramework.Net.Cloud.Virtuals.Channels;

namespace KJFramework.Net.Cloud.Virtuals.Processors
{
    /// <summary>
    ///     具有获取传输通道功能的傀儡处理器，提供了相关的基本操作
    ///     <para>* 此傀儡处理器，总是返回根据一个随机数来决定返回的结果.</para>
    /// </summary>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public class RandomGetTransportChannelPuppetFunctionProcessor<TMessage> : PuppetFunctionProcessor
    {
        #region Members

        private PuppetNetworkNode<TMessage> _puppetNetworkNode;
        private static Random _random = new Random();

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
            _puppetNetworkNode.DGetTransportChannel = delegate(Guid id)
                                                          {
                                                              return _random.Next(10) % 2 == 0
                                                                         ? new PuppetTransportChannel()
                                                                         : null;
                                                          };
            return true;
        }

        /// <summary>
        ///     释放当前的傀儡功能处理
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