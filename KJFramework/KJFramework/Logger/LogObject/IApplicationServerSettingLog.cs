using System;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     应用服务器设置记录元接口, 提供了相关的基本操作。
    /// </summary>
    public interface IApplicationServerSettingLog : ILog
    {
        /// <summary>
        ///     获取或设置服务开放端口 (TCP/UDP)
        /// </summary>
        /// <remarks>
        ///     非远程端口, 远程服务端口应该由远程服务配置文件进行设定
        /// </remarks>
        int ServicePort { get; set; }
        /// <summary>
        ///     获取或设置远程服务配置文件路径
        /// </summary>
        String RemotingConfigurationFilePath { get; set; }
    }
}
