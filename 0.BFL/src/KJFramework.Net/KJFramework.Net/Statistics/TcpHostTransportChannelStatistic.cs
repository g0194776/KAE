using KJFramework.Statistics;

namespace KJFramework.Net.Statistics
{
    internal class TcpHostTransportChannelStatistic : Statistic
    {
        #region Overrides of Statistic

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="element">统计类型</param><typeparam name="T">统计类型</typeparam>
        public override void Initialize<T>(T element)
        {
        }

        /// <summary>
        /// 关闭统计
        /// </summary>
        public override void Close()
        {
        }

        #endregion
    }
}