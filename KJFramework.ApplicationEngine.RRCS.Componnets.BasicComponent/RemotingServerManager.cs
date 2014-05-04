using System.Collections.Generic;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent
{
    /// <summary>
    ///     远程服务地址管理器
    /// </summary>
    internal  static class RemotingServerManager
    {
        #region Members.

        private static readonly object _lockObj = new object();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RemotingServerManager));
        //key = MessageIdentity + Application's Level + Application's Version
        private static readonly IDictionary<string, IList<string>> _addresses = new Dictionary<string, IList<string>>();

        #endregion

        #region Methods.



        #endregion
    }
}