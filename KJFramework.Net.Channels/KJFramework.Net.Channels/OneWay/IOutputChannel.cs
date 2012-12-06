using KJFramework.Messages.Contracts;

namespace KJFramework.Net.Channels.OneWay
{
    /// <summary>
    ///     输出通道元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public interface IOutputChannel<T> : IOnewayChannel<T>
        where T : IntellectObject
    {
        /// <summary>
        ///     请求一个消息到远程终结点
        /// </summary>
        /// <param name="message">请求的消息</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="Exception">发送失败</exception>
        int Send(T message);
        /// <summary>
        ///     异步请求一个消息到远程终结点
        /// </summary>
        /// <param name="message">请求的消息</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        /// <exception cref="Exception">发送失败</exception>
        /// <returns>返回异步结果</returns>
        void SendAsync(T message);
    }
}