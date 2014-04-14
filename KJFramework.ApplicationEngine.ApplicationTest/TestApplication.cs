using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.ApplicationEngine.ApplicationTest
{
    public class TestApplication : Application
    {
        public override IList<KAENetworkResource> AcquireCommunicationSupport()
        {
            return null;
        }

        public override IDictionary<ProtocolTypes, IList<MessageIdentity>> AcquireSupportedProtocols()
        {
            return null;
        }
    }
}
