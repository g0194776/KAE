using KJFramework.Messages.Attributes;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Net.Transaction.UnitTest.ProtocolStack
{
    public class TestRequestMessage : BaseMessage
    {
        public TestRequestMessage()
        {
            MessageIdentity = new MessageIdentity {ProtocolId = 12, ServiceId = 0, DetailsId = 0};
        }

        [IntellectProperty(10)]
        public string Value1 { get; set; }
    }
}