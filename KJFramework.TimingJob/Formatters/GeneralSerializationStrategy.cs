using System;
using System.Text;
using EasyNetQ;
using KJFramework.TimingJob.Decoders;
using KJFramework.TimingJob.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KJFramework.TimingJob.Formatters
{
    /// <summary>
    ///    ADSS消息的序列化/反序列化策略
    /// </summary>
    internal class GeneralSerializationStrategy : IMessageSerializationStrategy
    {
        #region Methods.

        private static readonly IDataDecoder _decoder = new DefaultBinaryDataDecoder();

        #endregion

        public SerializedMessage SerializeMessage(IMessage message)
        {
            throw new NotImplementedException();
        }

        public IMessage<T> DeserializeMessage<T>(MessageProperties properties, byte[] body) where T : class
        {
            byte[] decodedData = _decoder.Decode(body);
            if (decodedData == null) throw new ArgumentException("Failed to deserialize message");
            return new Message<T>((T)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(decodedData)));
        }

        public IMessage DeserializeMessage(MessageProperties properties, byte[] body)
        {
            byte[] decodedData = _decoder.Decode(body);
            if (decodedData == null) throw new ArgumentException("Failed to deserialize message");
            EasyNetQMessage message =  new EasyNetQMessage(properties, (JObject)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(decodedData)));
            return message;
        }
    }
}