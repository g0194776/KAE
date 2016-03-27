using System;
using System.Collections.Generic;
using System.Text;
using KJFramework.Net.ProtocolStacks;

namespace ConsoleApplication1
{
    public class StringP : ProtocolStack<string>
    {
        #region Overrides of ProtocolStack<string>

        public override bool Initialize()
        {
            return true;
        }

        public override List<string> Parse(byte[] data, int offset, int count)
        {
            byte[] dd = new byte[count];
            Buffer.BlockCopy(data, offset, dd, 0, count);
            return new List<string> {Encoding.Default.GetString(dd)};
        }

        public override List<string> Parse(byte[] data)
        {
            return new List<string> { Encoding.Default.GetString(data) };
        }

        public override byte[] ConvertToBytes(string message)
        {
            return Encoding.Default.GetBytes(message);
        }

        #endregion
    }
}