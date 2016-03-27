using System;
using System.Collections.Generic;

namespace KJFramework.ApplicationEngine.Loggers
{
    /// <summary>
    ///     KAE宿主状态记录器接口
    /// </summary>
    internal interface IKAEStateLogger
    {
        #region Members.

        /// <summary>
        ///     获取或设置内部所能够包含的日志条数最大值，超过这个数目的历史数据将会自动消失
        /// </summary>
        int MaximumLogCount { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///     记录一条状态信息
        /// </summary>
        /// <param name="content">需要被记录的状态信息内容</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        void Log(string content);
        /// <summary>
        ///    返回内部所包含的所有状态信息
        /// </summary>
        /// <returns>返回包含的数据</returns>
        List<string> GetAllLogs();

        #endregion
    }
}