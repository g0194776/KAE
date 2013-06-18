using System;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     全局配置项
    /// </summary>
    public static class CSNGlobal
    {
        /// <summary>
        ///     全局的事务超时时间
        /// </summary>
        public static readonly TimeSpan TransactionTimeout = TimeSpan.Parse("00:00:30");
        /// <summary>
        ///     全局的事务超时检查时间间隔
        ///     <para>* 单位: 秒</para>
        /// </summary>
        public static readonly int TransactionCheckInterval = 30;
    }
}