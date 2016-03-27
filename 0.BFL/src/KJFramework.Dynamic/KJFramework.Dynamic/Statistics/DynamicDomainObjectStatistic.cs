using System;
using KJFramework.Dynamic.Components;
using KJFramework.Statistics;

namespace KJFramework.Dynamic.Statistics
{
    /// <summary>
    ///     动态程序域对象统计器，提供了相关的基本操作。
    /// </summary>
    public class DynamicDomainObjectStatistic : Statistic
    {
        #region 成员

        private DynamicDomainObject _target;

        #endregion

        #region Overrides of Statistic

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="element">统计类型</param>
        /// <typeparam name="T">统计类型</typeparam>
        public override void Initialize<T>(T element)
        {
            _target = (DynamicDomainObject) ((Object) element);
            _target.WorkProcessing += WorkProcessing;
        }

        /// <summary>
        /// 关闭统计
        /// </summary>
        public override void Close()
        {
            if (_target != null)
            {
                _target.WorkProcessing -= WorkProcessing;
                _target = null;
            }
        }

        #endregion

        #region 事件

        void WorkProcessing(object sender, EventArgs.LightSingleArgEventArgs<string> e)
        {
            Console.WriteLine(e.Target);
        }

        #endregion
    }
}