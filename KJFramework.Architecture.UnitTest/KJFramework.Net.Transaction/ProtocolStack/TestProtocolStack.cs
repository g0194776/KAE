using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.Net.Transaction.UnitTest.ProtocolStack
{
    public class TestProtocolStack : BusinessProtocolStack
    {
        public override bool Initialize()
        {
            return true;
        }
    }
}