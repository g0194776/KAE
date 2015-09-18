using KJFramework.TimingJob.EventArgs;
using Newtonsoft.Json.Linq;

namespace KJFramework.TimingJob.Formatters
{
    /// <summary>
    ///    Media Dissector JSON数据序列化/反序列化器接口
    /// </summary>
    public interface ITaskJsonDataSerializer<in T>
    {
        #region Methods.

        /// <summary>
        ///    尝试将一个业务对象序列化为字节数组用于传输
        /// </summary>
        /// <param name="obj">序列化后追加数据的宿主JOBJECT</param>
        /// <param name="task">即将被序列化的Media Dissector任务对象</param>
        /// <returns>返回操作是否执行成功的标示</returns>
        SerializerResultTypes TrySerialize(JObject obj, T task);
        /// <summary>
        ///    尝试将指定的字节数组反序列化为业务对象
        /// </summary>
        /// <param name="data">即将被反序列化的字节数组</param>
        /// <param name="task">需要被赋值的Media Dissector任务对象</param>
        /// <returns>返回操作是否执行成功的标示</returns>
        SerializerResultTypes TryDeserialize(JObject data, T task);


        #endregion
    }
}