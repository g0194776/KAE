namespace KJFramework.Tracing
{
    public interface ITracing
    {
        void Info(string format, params object[] args);
        void Info(System.Exception ex, string format, params object[] args);
        void Warn(string format, params object[] args);
        void Warn(System.Exception ex, string format, params object[] args);
        void Error(string format, params object[] args);
        void Error(System.Exception ex, string format, params object[] args);
        void Critical(string format, params object[] args);
        void Critical(System.Exception ex, string format, params object[] args);
    }

    public class NullTracing : ITracing
    {
        public void Info(string format, params object[] args) { Trace(TracingLevel.Info, null, format, args); }
        public void Info(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Info, ex, format, args); }
        public void Warn(string format, params object[] args) { Trace(TracingLevel.Warn, null, format, args); }
        public void Warn(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Warn, ex, format, args); }
        public void Error(string format, params object[] args) { Trace(TracingLevel.Error, null, format, args); }
        public void Error(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Error, ex, format, args); }
        public void Critical(string format, params object[] args) { Trace(TracingLevel.Crtitical, null, format, args); }
        public void Critical(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Crtitical, ex, format, args); }

        protected virtual void Trace(TracingLevel level, System.Exception error, string format, params object[] args) { /* no-op */ }
    }

    public class FormatTracing : NullTracing
    {
        private string _logger;

        public FormatTracing(string logger)
        {
            _logger = logger;
        }

        protected override void Trace(TracingLevel level, System.Exception error, string format, params object[] args)
        {
            try
            {
                if (level >= TracingSettings.Level)
                {
                    for (int i = 0; i < args.Length; ++i)
                    {
                        //#region
                        //if (args[i] != null)
                        //{
                        //    if (args[i] is Guid)
                        //    {
                        //        args[i] = CamelUtility.ToString((Guid)args[i]);
                        //    }
                        //    else if (args[i] is byte[])
                        //    {
                        //        byte[] bin = ((byte[])args[i]);
                        //        int len;
                        //        StringBuilder str;
                        //        if (bin.Length > 20)
                        //        {
                        //            len = 20;
                        //            str = new StringBuilder(43);
                        //        }
                        //        else
                        //        {
                        //            len = bin.Length;
                        //            str = new StringBuilder(len * 2);
                        //        }
                        //        for (int n = 0; n < len; ++n)
                        //            CamelUtility.FormatAsHex(bin[n], str);
                        //        if (bin.Length > 20)
                        //            str.Append("...");
                        //        args[i] = str.ToString();
                        //    }
                        //    else if (args[i] is IFormattableMessage)
                        //    {
                        //        args[i] = ServiceSettings.Debug
                        //                ? (args[i] as IFormattableMessage).FormatAsString(0)
                        //                : (args[i] as IFormattableMessage).FormatAsStringLite();
                        //    }
                        //}
                        //#endregion
                    }

                    string message = string.Empty;
                    try
                    {
                        message = args.Length == 0 ? format : string.Format(format ?? string.Empty, args);
                    }
                    catch (System.Exception ex)
                    {
                        if (level < TracingLevel.Warn)
                            level = TracingLevel.Warn;
                        if (error == null)
                            error = ex;
                        message = string.Concat("tracing formatting error: [", args.Length, "] ", format ?? string.Empty);
                    }

                    TracingManager.AddTraceItem(new TraceItem(_logger, level, error, message));
                }
            }
            catch 
            {
                // mute everything...
            }
        }
    }
}
