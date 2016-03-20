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
            if (properties == null) throw new ArgumentNullException("properties");
            if (body == null) throw new ArgumentNullException("body");
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
        public MessageProperties Properties { get; internal set; }

        /// <summary>
        /// The message <see cref="T:System.Type"/>. This is a shortcut to GetBody().GetType().
        /// </summary>
        public Type MessageType
        {
            get { return typeof (JObject); }
        }

        /// <summary>
        ///    获取消息对象
        /// </summary>
        public JObject Body { get; internal set; }

        #endregion
    }
}