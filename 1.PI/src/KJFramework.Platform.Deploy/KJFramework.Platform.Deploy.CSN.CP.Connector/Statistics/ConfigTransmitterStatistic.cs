using System;
using System.Diagnostics;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters;
using KJFramework.Statistics;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Statistics
{
    /// <summary>
    ///     ���ô�����ͳ����
    /// </summary>
    public class ConfigTransmitterStatistic : Statistic
    {
        #region Members

        private IConfigTransmitter _configTransmitter;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <param name="element">ͳ������</param>
        /// <typeparam name="T">ͳ������</typeparam>
        public override void Initialize<T>(T element)
        {
            _configTransmitter = (IConfigTransmitter) element;
            _configTransmitter.Processing += ConfigTransmitterProcessing;
            throw new NotImplementedException();
        }

        /// <summary>
        ///     �ر�ͳ��
        /// </summary>
        public override void Close()
        {
            if (_configTransmitter != null)
            {
                _configTransmitter.Processing -= ConfigTransmitterProcessing;
                _configTransmitter = null;
            }
        }

        #endregion

        #region Events

        //processing event
        void ConfigTransmitterProcessing(object sender, EventArgs.LightSingleArgEventArgs<string> e)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine(e.Target);
            }
        }

        #endregion
    }
}