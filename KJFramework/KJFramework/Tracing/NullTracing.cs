namespace KJFramework.Tracing
{
    internal class NullTracing : ITracing
    {
        public void Info(string format, params object[] args) { Trace(TracingLevel.Info, null, format, args); }
        public void Info(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Info, ex, format, args); }
        public void Warn(string format, params object[] args) { Trace(TracingLevel.Warn, null, format, args); }
        public void Warn(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Warn, ex, format, args); }
        public void Error(System.Exception ex) {  Error(ex, null); }
        public void Error(string format, params object[] args) { Trace(TracingLevel.Error, null, format, args); }
        public void Error(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Error, ex, format, args); }
        public void Critical(string format, params object[] args) { Trace(TracingLevel.Crtitical, null, format, args); }
        public void Critical(System.Exception ex, string format, params object[] args) { Trace(TracingLevel.Crtitical, ex, format, args); }
        public void LogFileOnly(TracingLevel level, string format, params object[] args) { TraceFileOnly(level, format, args); }
        public void LogFileOnly(TracingLevel level, System.Exception ex, string format, params object[] args) { TraceFileOnly(level, ex, format, args); }

        protected virtual void Trace(TracingLevel level, System.Exception error, string format, params object[] args) { /* no-op */ }
        protected virtual void TraceFileOnly(TracingLevel level, string format, params object[] args) { /* no-op */ }
        protected virtual void TraceFileOnly(TracingLevel level, System.Exception error, string format, params object[] args) { /* no-op */ }
    }
}