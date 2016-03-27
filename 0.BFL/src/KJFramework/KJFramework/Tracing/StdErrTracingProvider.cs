using System;

namespace KJFramework.Tracing
{
    internal class StdErrTracingProvider : TxTracingProvider
    {
        #region Methods

        protected override void Write(string text)
        {
            Console.Error.Write(text);
            Console.Error.Flush();
        }

        #endregion
    }
}