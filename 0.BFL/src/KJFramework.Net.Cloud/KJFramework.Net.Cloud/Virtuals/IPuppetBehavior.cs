using System;
using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Schedulers;
using KJFramework.Net.Cloud.Virtuals.Processors;

namespace KJFramework.Net.Cloud.Virtuals
{
    /// <summary>
    ///     傀儡行为元接口，提供了相关的基本操作
    /// </summary>
    public interface IPuppetBehavior<T>
    {
        /// <summary>
        ///     附加一个傀儡功能处理器
        /// </summary>
        /// <param name="processor">傀儡功能处理器</param>
        /// <returns>返回自身</returns>
        PuppetNetworkNode<T> Attach(IPuppetFunctionProcessor processor);
        /// <summary>
        ///     创建一个傀儡任务调度器
        /// </summary>
        /// <param name="taskCount">初始化的任务数量</param>
        /// <returns>返回创建后的傀儡任务调度器</returns>
        IRequestScheduler<T> CreateScheduler(int taskCount = 100);
        /// <summary>
        ///     附加成功事件
        /// </summary>
        event EventHandler AttachSuccessed;
        /// <summary>
        ///     附加失败事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<string>> AttachFailed;
    }
}