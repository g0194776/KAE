using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.ApplicationEngine.ApplicationTest
{
    public class TestApplication : Application
    {
        public override IList<KAENetworkResource> AcquireCommunicationSupport()
        {
             IList<KAENetworkResource> networks = new List<KAENetworkResource>();
            networks.Add(new KAENetworkResource{NetworkUri = new TcpUri("tcp://localhost:6666"), Protocol = ProtocolTypes.Intellegence});
            return networks;
        }

        protected override void InnerInitialize()
        {
        }
    }
}
