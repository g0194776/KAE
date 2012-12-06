using System;
using KJFramework.Statistics;

namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     动态对象访问器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDynamicObjectVisitor : IStatisticable<IStatistic>, IDisposable
    {
        /// <summary>
        ///     获取一个具有指定名称的动态程序域组件
        ///     <para>* 如果当前要获取的组件正在升级，则获取到的组件为升级前版本。</para>
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="flag">标记名称</param>
        /// <param name="args">可选参数</param>
        /// <returns>返回指定对象</returns>
        T GetObject<T>(String flag, params Object[] args);
    }
}