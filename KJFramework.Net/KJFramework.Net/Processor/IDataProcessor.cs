using System.Collections.Generic;
using KJFramework.Net.Buffers;

namespace KJFramework.Net.Processor
{
    /// <summary>
    ///     数据处理器元接口, 提供了对于网络数据的相关处理功能。
    /// </summary>
    /// <typeparam name="TMessage">返回的消息类型</typeparam>
    public interface IDataProcessor<TMessage> : IProcessor
    {
        /// <summary>
        ///     处理
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="bufferPool">缓冲池</param>
        /// <returns>返回处理后的消息</returns>
        List<TMessage> Process(byte[] data, IBufferPool bufferPool);

        /// <summary>
        ///     处理部分数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="bufferPool">缓冲池</param>
        /// <returns>返回处理后的数据</returns>
        byte[] ProcessPartData(byte[] data, IBufferPool bufferPool);
    }
}
