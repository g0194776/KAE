using System;
using KJFramework.Net.Transaction.Configurations;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     全局配置项
    /// </summary>
    internal static class Global
    {
        /// <summary>
        ///     全局的事务超时时间
        /// </summary>
        public static readonly TimeSpan TransactionTimeout = TransactionSettingConfigSection.Current.Settings == null ? TimeSpan.Parse("00:00:30") : TimeSpan.Parse(TransactionSettingConfigSection.Current.Settings.TransactionTimeout);
        /// <summary>
        ///     全局的事务超时检查时间间隔
        ///     <para>* 单位: 秒</para>
        /// </summary>
        public static readonly int TransactionCheckInterval = TransactionSettingConfigSection.Current.Settings == null ? 30 : TransactionSettingConfigSection.Current.Settings.TransactionCheckInterval;
    }
}