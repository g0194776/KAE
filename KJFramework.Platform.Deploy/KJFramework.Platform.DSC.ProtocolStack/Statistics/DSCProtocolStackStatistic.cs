using System;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Statistics;

namespace KJFramework.Platform.DSC.ProtocolStack.Statistics
{
    public class DSCProtocolStackStatistic : Statistic
    {
        #region Members

        private DSCProtocolStack _protocolStack;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="element">统计类型</param><typeparam name="T">统计类型</typeparam>
        public override void Initialize<T>(T element)
        {
            _protocolStack = (DSCProtocolStack)((Object)element);
            _protocolStack.MessageToByteFailed += MessageToByteFailed;
            _protocolStack.MessageToByteSuccessed += MessageToByteSuccessed;
            _protocolStack.ParseMessageFailed += ParseMessageFailed;
            _protocolStack.ParseMessageSuccessed += ParseMessageSuccessed;
        }

        /// <summary>
        /// 关闭统计
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

        void ParseMessageSuccessed(object sender, EventArgs.LightSingleArgEventArgs<DSCMessage> e)
        {
        }

        void ParseMessageFailed(object sender, EventArgs.LightSingleArgEventArgs<byte[]> e)
        {
        }

        void MessageToByteSuccessed(object sender, EventArgs.LightSingleArgEventArgs<DSCMessage> e)
        {
        }

        void MessageToByteFailed(object sender, EventArgs.LightSingleArgEventArgs<DSCMessage> e)
        {
        }

        #endregion
    }
}