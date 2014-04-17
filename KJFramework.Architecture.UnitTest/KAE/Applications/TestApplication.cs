using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Resources;

namespace KJFramework.Architecture.UnitTest.KAE.Applications
{
    public class TestApplication : Application
    {
        public override IList<KAENetworkResource> AcquireCommunicationSupport()
        {
            return null;
        }

        protected override void InnerInitialize()
        {
            
        }
    }
}
