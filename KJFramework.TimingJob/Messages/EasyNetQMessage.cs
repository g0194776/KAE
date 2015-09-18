using System;
using EasyNetQ;
using Newtonsoft.Json.Linq;

namespace KJFramework.TimingJob.Messages
{
    /// <summary>
    ///    EasyNetQ对于接收到消息的外层包装类
    /// </summary>
    internal class EasyNetQMessage : IMessage<JObject>
    {
        #region Constructor.

        /// <summary>
        ///    EasyNetQ对于接收到消息的外层包装类
        /// </summary>
        /// <param name="properties">消息属性</param>
        /// <param name="body">消息对象</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public EasyNetQMessage(MessageProperties properties, JObject body)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (body == null) throw new ArgumentNullException(nameof(body));
            Properties = properties;
            Body = body;
        }

        #endregion

        #region Members.

        public object GetBody()
        {
            return Body;
        }

        /// <summary>
        ///    获取消息属性
        /// </summary>
        public MessageProperties Properties { get; }

        public Type MessageType => typeof(JObject);

        /// <summary>
        ///    获取消息对象
        /// </summary>
        public JObject Body { get; }

        #endregion
    }
}