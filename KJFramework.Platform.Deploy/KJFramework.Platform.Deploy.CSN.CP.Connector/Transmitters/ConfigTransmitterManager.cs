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
        ///     注册一个配置传输器
        /// </summary>
        /// <param name="configTransmitter">配置传输器</param>
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
        ///     获取一个配置传输器
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <returns>返回配置传输器</returns>
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
        ///     移除一个配置传输器
        /// </summary>
        /// <param name="taskId">任务编号</param>
        public void UnRegist(int taskId)
        {
            _transmitters.Remove(taskId);
        }

        #endregion
    }
}