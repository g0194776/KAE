using System;
using Newtonsoft.Json.Linq;

namespace KJFramework.TimingJob.Formatters
{
    /// <summary>
    ///    任务数据格式化器接口
    /// </summary>
    public interface ITaskDataFormatter<T>
    {
        #region Methods.

        /// <summary>
        ///    尝试序列化一个Media Dissector任务为远程任务消息
        /// </summary>
        /// <param name="result">远程任务消息</param>
        /// <param name="task">序列化所需要的Media Dissector任务</param>
        /// <returns>返回是否序列化成功的标示</returns>
        bool TrySerialize(JObject result, T task);
        /// <summary>
        ///    尝试解析一个远程任务消息为Media Dissector任务
        /// </summary>
        /// <param name="result">远程任务消息</param>
        /// <param name="task">解析成功后的Media Dissector任务</param>
        /// <returns>返回是否解析成功的标示</returns>
        bool TryParse(JObject result, out T task);
        /// <summary>
        ///    注册一个新的数据序列化器
        /// </summary>
        /// <param name="serializer">被注入的数据序列化器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        ITaskDataFormatter<T> Register(ITaskJsonDataSerializer<T> serializer);

        #endregion
    }
}