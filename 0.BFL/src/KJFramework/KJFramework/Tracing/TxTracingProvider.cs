using System.Text;

namespace KJFramework.Tracing 
{
    internal abstract class TxTracingProvider : ITracingProvider
    {
        #region Methods

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

        #endregion
    }
}
