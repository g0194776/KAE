using KJFramework.Messages.Attributes;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Net.Transaction.UnitTest.ProtocolStack
{
    public class TestResponseMessage : BaseMessage
    {
        public TestResponseMessage()
        {
            MessageIdentity = new MessageIdentity {ProtocolId = 12, ServiceId = 0, DetailsId = 1};
        }

        [IntellectProperty(10, AllowDefaultNull = true)]
        public byte ErrorId { get; set; }
    }
}