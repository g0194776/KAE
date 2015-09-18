using System;
using EasyNetQ;
using KJFramework.Tracing;

namespace KJFramework.TimingJob.Pools
{
    /// <summary>
    ///    支持EasyNetQ内部的Logger转发
    /// </summary>
    internal sealed class RabbitMQLogger : IEasyNetQLogger
    {
        #region Members.

        public static readonly IEasyNetQLogger Instance = new RabbitMQLogger();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RabbitMQLogger));

        #endregion

        #region Methods.

        public void DebugWrite(string format, params object[] args)
        {
            _tracing.DebugInfo(string.Format(format, args));
        }

        public void InfoWrite(string format, params object[] args)
        {
            _tracing.Info(format, args);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            _tracing.Error(format, args);
        }

        public void ErrorWrite(Exception exception)
        {
            _tracing.Error("ERROR", exception);
        }

        #endregion
    }
}