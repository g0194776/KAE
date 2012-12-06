using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Web;

namespace KJFramework.Tracing 
{
    public abstract class TxTracingProvider : ITracingProvider
    {
        public void Write(string pid, string pname, string machine, TraceItem[] items)
        {
            StringBuilder text = new StringBuilder(500);
            for (int i = 0; i < items.Length; ++i)
            {
                text.AppendLine("***");
                text.AppendLine("--------------------------------------------------");
                text.Append('[').Append(items[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append("] ");
                text.Append(pname).Append(" <").Append(pid).Append("> @ ").AppendLine(machine);
                text.Append('[').Append(items[i].Level.ToString()).Append("]: ").AppendLine(items[i].Logger).AppendLine();
                text.AppendLine(items[i].Message);
                if (items[i].Error != null)
                    text.AppendLine().AppendLine(items[i].Error.ToString());
                text.AppendLine("--------------------------------------------------");
            }
            Write(text.ToString());
        }

        protected abstract void Write(string text);
    }

    public class FileTracingProvider : TxTracingProvider 
    {
        private string _dir;

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
    }

    public class StdErrTracingProvider : TxTracingProvider
    {
        protected override void Write(string text)
        {
            Console.Error.Write(text);
            Console.Error.Flush();
        }
    }
}
