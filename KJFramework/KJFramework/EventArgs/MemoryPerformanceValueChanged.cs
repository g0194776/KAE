using System;
namespace KJFramework.EventArgs
{
    public delegate void DelegateMemoryPerformanceValueChanged(Object sender, MemoryPerformanceValueChangedEventArgs e);
    /// <summary>
    ///     内存性能值更改事件
    /// </summary>
    public class MemoryPerformanceValueChangedEventArgs : System.EventArgs
    {
        #region 成员

        private double _memoryUsage;
        /// <summary>
        ///     获取CPU使用率
        /// </summary>
        public double MemoryUsage
        {
            get { return _memoryUsage; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     内存性能值更改事件
        /// </summary>
        /// <param name="memoryUsage">内存性能值</param>
        public MemoryPerformanceValueChangedEventArgs(double memoryUsage)
        {
            _memoryUsage = memoryUsage;
        }

        #endregion

    }
}