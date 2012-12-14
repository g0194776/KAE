using System;
using System.Threading;
using KJFramework.Tracing.Configuration;

namespace KJFramework.Tracing
{
    public enum TracingLevel { Info = 0, Warn = 1, Error = 2, Crtitical = 3 }

    public static class TracingSettings
    {
        private const string Section = "Tracing";

        private static int _version;
        private static Action _handler;

        internal static void WatchProviderChange(Action handler)
        {
            _handler = handler;
        }

        private static void OnTracingProviderChanged(int version, string section, string name)
        {
            if (Interlocked.Exchange(ref _version, version) != version)
                _handler();
        }

        public static TracingLevel Level
        {
            get { return (TracingLevel)Enum.Parse(typeof(TracingLevel), TracingDescriptionConfigSection.Current.Details.Level); }
        }

        public static string Provider
        {
            get { return TracingDescriptionConfigSection.Current.Details.Provider; }
        }

        public static string Datasource
        {
            get { return TracingDescriptionConfigSection.Current.Details.Datasource; }
        }
    }
}
