using System;

namespace KJFramework.Delegate
{
    /// <summary>
    ///     轻型委托项元接口，提供了相关的基本操作。
    /// </summary>
    public interface ILightDelegateItem : IDisposable
    {
        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     运行
        /// </summary>
        /// <param name="objs">运行参数</param>
        void Execute(params Object[] objs);
    }
}