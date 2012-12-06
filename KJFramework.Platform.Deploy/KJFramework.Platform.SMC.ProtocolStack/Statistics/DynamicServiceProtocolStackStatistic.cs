using System;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;
using KJFramework.Statistics;

namespace KJFramework.Platform.Deploy.SMC.ProtocolStack.Statistics
{
    public class DynamicServiceProtocolStackStatistic : Statistic
    {
        #region Members

        private DynamicServiceProtocolStack _protocolStack;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param><typeparam name="T">ͳ������</typeparam>
        public override void Initialize<T>(T element)
        {
            _protocolStack = (DynamicServiceProtocolStack) ((Object)element);
            _protocolStack.MessageToByteFailed += MessageToByteFailed;
            _protocolStack.MessageToByteSuccessed += MessageToByteSuccessed;
            _protocolStack.ParseMessageFailed += ParseMessageFailed;
            _protocolStack.ParseMessageSuccessed += ParseMessageSuccessed;
        }

        /// <summary>
        /// �ر�ͳ��
        /// </summary>
        public override void Close()
        {
            if (_protocolStack != null)
            {
                _protocolStack.MessageToByteFailed -= MessageToByteFailed;
                _protocolStack.MessageToByteSuccessed -= MessageToByteSuccessed;
                _protocolStack.ParseMessageFailed -= ParseMessageFailed;
                _protocolStack.ParseMessageSuccessed -= ParseMessageSuccessed;
                _protocolStack = null;
            }
        }

        #endregion

        #region Events

        void ParseMessageSuccessed(object sender, EventArgs.LightSingleArgEventArgs<DynamicServiceMessage> e)
        {
        }

        void ParseMessageFailed(object sender, EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
        }

        void MessageToByteSuccessed(object sender, EventArgs.LightSingleArgEventArgs<DynamicServiceMessage> e)
        {
        }

        void MessageToByteFailed(object sender, EventArgs.LightSingleArgEventArgs<DynamicServiceMessage> e)
        {
        }

        #endregion
    }
}