using System;
using KJFramework.Basic.Enum;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     基础的应用服务器设置记录, 提供了相关的基本操作。
    /// </summary>
    [Serializable]
    public class BasicApplicationServerSettingLog : IApplicationServerSettingLog
    {
        #region IApplicationServerSettingLog 成员

        private int _serviceport;

        /// <summary>
        ///     获取或设置服务开放端口 (TCP/UDP)
        /// </summary>
        /// <remarks>
        ///     非远程端口, 远程服务端口应该由远程服务配置文件进行设定
        /// </remarks>
        public int ServicePort
        {
            get
            {
                return _serviceport;
            }
            set
            {
                _serviceport = value;
            }
        }

        private String _remotingconfigurationfilepath;

        /// <summary>
        ///     获取或设置远程服务配置文件路径
        /// </summary>
        public string RemotingConfigurationFilePath
        {
            get
            {
                return _remotingconfigurationfilepath;
            }
            set
            {
                _remotingconfigurationfilepath = value;
            }
        }

        #endregion

        #region ILog 成员

        private DateTime _time;

        /// <summary>
        ///     获取或设置记录具体日期事件
        /// </summary>
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        private String _name;

        /// <summary>
        ///     获取或设置记录名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private String _user;

        /// <summary>
        ///     获取或设置记录人
        /// </summary>
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        /// <summary>
        ///     获取该记录对象的类型
        /// </summary>
        public LogTypes LogType
        {
            get { return LogTypes.ApplicationDebug; }
        }

        private String _tag;

        /// <summary>
        ///     获取或设置记录辅助数据
        /// </summary>
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        #endregion
    }
}
