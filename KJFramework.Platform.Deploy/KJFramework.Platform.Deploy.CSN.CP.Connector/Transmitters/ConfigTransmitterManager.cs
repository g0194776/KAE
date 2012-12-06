using System;
using System.Collections.Generic;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters
{
    public class ConfigTransmitterManager
    {
        #region Constructor

        private ConfigTransmitterManager()
        {
            _transmitters = new Dictionary<int, IConfigTransmitter>();
        }

        #endregion

        #region Members

        private Dictionary<int, IConfigTransmitter> _transmitters;
        public static readonly ConfigTransmitterManager Instance = new ConfigTransmitterManager();

        #endregion

        #region Methods

        /// <summary>
        ///     ע��һ�����ô�����
        /// </summary>
        /// <param name="configTransmitter">���ô�����</param>
        public void Regist(IConfigTransmitter configTransmitter)
        {
            if (configTransmitter == null)
            {
                throw new ArgumentNullException("configTransmitter");
            }
            IConfigTransmitter t;
            if (!_transmitters.TryGetValue(configTransmitter.TaskId, out t))
            {
                _transmitters.Add(configTransmitter.TaskId, configTransmitter);
            }
        }

        /// <summary>
        ///     ��ȡһ�����ô�����
        /// </summary>
        /// <param name="taskId">������</param>
        /// <returns>�������ô�����</returns>
        public IConfigTransmitter GetTransmitter(int taskId)
        {
            IConfigTransmitter t;
            if (_transmitters.TryGetValue(taskId, out t))
            {
                return t;
            }
            return null;
        }

        /// <summary>
        ///     �Ƴ�һ�����ô�����
        /// </summary>
        /// <param name="taskId">������</param>
        public void UnRegist(int taskId)
        {
            _transmitters.Remove(taskId);
        }

        #endregion
    }
}