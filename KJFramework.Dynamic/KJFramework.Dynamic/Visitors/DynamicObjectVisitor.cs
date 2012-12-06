using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Logger;
using KJFramework.Statistics;

namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     动态程序域对象访问器，提供了相关的基本操作。
    /// </summary>
    public class DynamicObjectVisitor : IDynamicObjectVisitor
    {
        #region 构造函数

        /// <summary>
        ///     动态程序域对象访问器，提供了相关的基本操作。
        /// </summary>
        /// <param name="component">程序域组件</param>
        internal DynamicObjectVisitor(DynamicDomainComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            _component = component;
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        protected DynamicDomainComponent _component;

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IDynamicObjectVisitor

        /// <summary>
        ///     获取一个具有指定名称的动态程序域组件
        ///     <para>* 如果当前要获取的组件正在升级，则获取到的组件为升级前版本。</para>
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="flag">标记名称</param>
        /// <param name="args">可选参数</param>
        /// <returns>返回指定对象</returns>
        public T GetObject<T>(String flag, params Object[] args)
        {
            //空值检测
            if (String.IsNullOrEmpty(flag))
            {
                return default(T);
            }
            try
            {
                if (_component == null || !_component.RuleTable.Exists(flag))
                {
                    return default(T);
                }
                return _component.RuleTable.Get<T>(flag, args);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        #endregion
    }
}