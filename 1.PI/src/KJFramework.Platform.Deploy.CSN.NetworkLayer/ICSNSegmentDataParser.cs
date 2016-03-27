using System;
using System.Collections.Generic;
using KJFramework.EventArgs;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     数据段解析器元接口
    /// </summary>
    public interface ICSNSegmentDataParser<T> : IDisposable
    {
        /// <summary>
        ///     追加一个新的数据段
        /// </summary>
        /// <param name="args">数据段接受参数</param>
        void Append(CSNSegmentReceiveEventArgs args);
        /// <summary>
        ///     解析成功事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<List<T>>> ParseSucceed;
    }
}