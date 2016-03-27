using System;
using System.Configuration;
using System.Threading;

namespace KJFramework.Tracing
{
    public enum TracingLevel { Info = 0, Warn = 1, Error = 2, Crtitical = 3, Debug = 4 }

    public static class TracingSettings
    {
        private const string Section = "Tracing";

        private static int _version;
        private static Action _handler;
        private static string _provider;
        private static string _dataSource;
        private static TracingLevel? _level;

        internal static void WatchProviderChange(Action handler)
        {
            _handler = handler;
        }

        private static void OnTracingProviderChanged(int version, string section, string name)
        {
            if (Interlocked.Exchange(ref _version, version) != version)
                _handler();
        }

        /// <summary>
        ///     获取或设置日志记录等级
        /// </summary>
        public static TracingLevel Level
        {
            get
            {
                if(_level == null) return (TracingLevel)Enum.Parse(typeof(TracingLevel), ConfigurationManager.AppSettings["Tracing-Level"]);
                return (TracingLevel) _level;
            }
            set { _level = value; }
        }

        /// <summary>
        ///     获取或设置日志记录器
        /// </summary>
        public static string Provider
        {
            get
            {
                if (string.IsNullOrEmpty(_provider)) return ConfigurationManager.AppSettings["Tracing-Provider"];
                return _provider;
            }
            set { _provider = value; }
        }

        /// <summary>
        ///     获取或设置日志记录源
        /// </summary>
        public static string Datasource
        {
            get
            {
                if (string.IsNullOrEmpty(_dataSource)) return ConfigurationManager.AppSettings["Tracing-Datasource"];
                return _dataSource;
            }
            set { _dataSource = value; }
        }
    }
}
