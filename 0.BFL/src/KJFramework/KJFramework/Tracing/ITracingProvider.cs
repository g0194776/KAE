using System;

namespace KJFramework.Tracing 
{
    public sealed class TraceItem
    {
        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
        }

        private string _logger;
        public string Logger
        {
            get { return _logger; }
        }

        private TracingLevel _level;
        public TracingLevel Level
        {
            get { return _level; }
        }

        private System.Exception _error;
        public System.Exception Error
        {
            get { return _error; }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
        }

        public TraceItem(string logger, TracingLevel level, System.Exception error, string message)
        {
            _timestamp = DateTime.UtcNow;
            _logger = logger;
            _level = level;
            _error = error;
            _message = message;
        }
    }

    public interface ITracingProvider
    {
        void Write(string pid, string pname, string machine, TraceItem[] items);
    }
}
