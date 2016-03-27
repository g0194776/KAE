using System;
using KJFramework.Net.Transaction.Configurations;
using KJFramework.Net.Transaction.Enums;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     全局配置项
    /// </summary>
    public static class Global
    {
        /// <summary>
        ///     全局的事务超时时间
        /// </summary>
        public static readonly TimeSpan TransactionTimeout = TransactionSettingConfigSection.Current.Settings == null
            ? TimeSpan.Parse("00:00:30")
            : TimeSpan.Parse(TransactionSettingConfigSection.Current.Settings.TransactionTimeout);

        /// <summary>
        ///     全局的事务超时检查时间间隔
        ///     <para>* 单位: 秒</para>
        /// </summary>
        public static readonly int TransactionCheckInterval = TransactionSettingConfigSection.Current.Settings == null
            ? 30
            : TransactionSettingConfigSection.Current.Settings.TransactionCheckInterval;

        /// <summary>
        ///    最大连接数(在相同的远程终结点地址情况下)
        /// </summary>
        public static readonly int MaximumConnectionCount = TransactionSettingConfigSection.Current.Settings == null
            ? 3
            : TransactionSettingConfigSection.Current.Settings.MaximumConnectionCount;

        /// <summary>
        ///    最小连接数(在相同的远程终结点地址情况下)
        /// </summary>
        public static readonly int MinimumConnectionCount = TransactionSettingConfigSection.Current.Settings == null
            ? 1
            : TransactionSettingConfigSection.Current.Settings.MinimumConnectionCount;

        /// <summary>
        ///    并行网络连接的分发策略
        /// </summary>
        public static readonly ConnectionLoadBalanceStrategies ConnectionLoadBalanceStrategy =
            (ConnectionLoadBalanceStrategies)
                (TransactionSettingConfigSection.Current.Settings == null
                    ? ConnectionLoadBalanceStrategies.Sequential
                    : Enum.Parse(typeof (ConnectionLoadBalanceStrategies),
                        TransactionSettingConfigSection.Current.Settings.ConnectionLoadBalanceStrategy, true));
    }
}