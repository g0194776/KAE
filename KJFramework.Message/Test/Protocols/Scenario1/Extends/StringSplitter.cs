using KJFramework.IO.Helper;
using KJFramework.Messages.Extends.Rules;

namespace Test.Protocols.Scenario1.Extends
{
    public class StringSplitRule : SplitRule
    {
        public StringSplitRule()
        {
            _supportType = typeof (string);
        }

        public override bool Check(int offset, byte[] data, ref int tagOffset, ref int targetContentLength)
        {
            int length = (int)ByteArrayHelper.GetNextData(data, offset, 1)[0];
            tagOffset = 0;
            targetContentLength = length + 1;
            return true;
        }
    }
}