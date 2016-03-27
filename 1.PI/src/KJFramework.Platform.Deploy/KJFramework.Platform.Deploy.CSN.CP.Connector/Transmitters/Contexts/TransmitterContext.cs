using System.Collections.Generic;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Objects;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Subscribers;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.CSN.ProtocolStack.Enums;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Transmitters.Contexts
{
    /// <summary>
    ///     传输器上下文
    /// </summary>
    public class TransmitterContext : ITransmitterContext
    {
        #region Members

        protected Dictionary<string, object> _objs = new Dictionary<string, object>();
        protected CSNMessage _responseMessage;
        protected int _previousSessionId;
        protected int _taskId;
        protected ConfigTypes _configType;
        protected IConfigSubscriber _subscriber;
        protected int _totalDataLength;
        protected int _totalPackageCount;
        protected DataPart[] _datas;

        #endregion

        #region Implementation of ITransmitterContext

        /// <summary>
        ///     获取或设置任务编号
        /// </summary>
        public int TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        /// <summary>
        ///     获取或设置总共的数据长度
        /// </summary>
        public int TotalDataLength
        {
            get { return _totalDataLength; }
            set { _totalDataLength = value; }
        }

        /// <summary>
        ///     获取或设置总共的分包数量
        /// </summary>
        public int TotalPackageCount
        {
            get { return _totalPackageCount; }
            set { _totalPackageCount = value; }
        }

        /// <summary>
        ///     获取或设置上一个请求消息的会话编号
        /// </summary>
        public int PreviousSessionId
        {
            get { return _previousSessionId; }
            set { _previousSessionId = value; }
        }

        /// <summary>
        ///     获取或设置分包数据集合
        /// </summary>
        public DataPart[] Datas
        {
            get { return _datas; }
            set { _datas = value; }
        }

        /// <summary>
        ///     获取或设置配置订阅者
        /// </summary>
        public IConfigSubscriber Subscriber
        {
            get { return _subscriber; }
            set { _subscriber = value; }
        }

        /// <summary>
        ///     获取或设置回馈消息
        /// </summary>
        public CSNMessage ResponseMessage
        {
            get { return _responseMessage; }
            set { _responseMessage = value; }
        }

        /// <summary>
        ///     获取或设置配置类型
        /// </summary>
        public ConfigTypes ConfigType
        {
            get { return _configType; }
            set { _configType = value; }
        }

        /// <summary>
        ///     获取具有指定关键值的值
        /// </summary>
        /// <typeparam name="T">返回值的类型</typeparam>
        /// <param name="key">关键字</param>
        /// <returns>返回值</returns>
        public T Get<T>(string key)
        {
            object obj;
            if (_objs.TryGetValue(key, out obj))
            {
                return (T) obj;
            }
            return default(T);
        }

        /// <summary>
        ///     添加一个值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">要添加的对象</param>
        public void Add(string key, object value)
        {
            if (!_objs.ContainsKey(key))
            {
                _objs.Add(key, value);
            }
        }

        /// <summary>
        ///     移除一个具有指定关键字的值
        /// </summary>
        /// <param name="key">关键字</param>
        public void Remove(string key)
        {
            _objs.Remove(key);
        }

        #endregion
    }
}