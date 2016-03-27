using KJFramework.Net.Transaction;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程日志代理器
    /// </summary>
    public class RemoteLogProxy : ITracingNotificationHandler
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (RemoteLogProxy));

        #endregion

        #region Methods

        /// <summary>
        ///     处理当前日志的通知
        /// <para>* 只有在满足当前日志等级后，才会调用此方法</para>
        /// </summary>
        /// <param name="level">日志级别</param>
        /// <param name="ex">异常对象</param>
        /// <param name="message">错误消息</param>
        /// <param name="loggerType">记录器类型名称</param>
        public void Handle(TracingLevel level, System.Exception ex, string message, string loggerType)
        {
            if (!SystemWorker.IsInitialized) return;
            if (level == TracingLevel.Info) return;
            //BusinessMessageTransaction lgsTransaction = SystemWorker.CreateOnewayTransaction("LGS");
            //if (lgsTransaction == null || lgsTransaction is FailMessageTransaction)
            //{
            //    _tracing.LogFileOnly(TracingLevel.Error, "#Cannot connect to remote {0} service currently.", "LGS");
            //    return;
            //}
            //string detail = string.Empty;
            //while (ex != null && ex.InnerException != null) ex = ex.InnerException;
            //if (ex != null) detail = "#" + ex.Message;
            //CreateLogRequestMessage requestMessage = new CreateLogRequestMessage
            //{
            //    ProcessLevel = Convert.ToByte((int)level),
            //    ClassName = loggerType,
            //    ProcessInfo = message,
            //    ServiceName = SystemWorker.Role,
            //    ProcessDetail = detail,
            //    CreateTime = DateTime.Now,
            //    ProcessTrace = ex.StackTrace
            //};
            //lgsTransaction.SendRequest(requestMessage);
        }

        #endregion
    }
}