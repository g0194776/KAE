using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;

namespace KJFramework.Tracing
{
    internal class FileTracingProvider : TxTracingProvider
    {
        #region Constructor

        public FileTracingProvider(string dir)
        {
            if (!Path.IsPathRooted(dir))
            {
                string root;
                if (HttpRuntime.AppDomainAppId == null)
                {
                    Process p = Process.GetCurrentProcess();
                    ProcessModule m = p.MainModule;
                    root = Path.GetDirectoryName(m.FileName);
                }
                else
                {
                    root = HttpRuntime.BinDirectory;
                }
                dir = Path.Combine(root, dir);
            }
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            _dir = dir;
        }

        #endregion

        #region Members

        private string _dir;

        #endregion

        #region Methods

        protected override void Write(string text)
        {
            if (!Directory.Exists(_dir))
                Directory.CreateDirectory(_dir);

            string name = Path.Combine(_dir, DateTime.UtcNow.ToString("yyyyMMddHH") + ".log");
            using (FileStream file = File.Open(name, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter writer = new StreamWriter(file, Encoding.UTF8))
            {
                writer.AutoFlush = true;
                writer.Write(text);
            }
        }

        #endregion
    }
}