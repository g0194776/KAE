﻿using System.Collections.Generic;
using KJFramework.ApplicationEngine.Resources;

namespace KJFramework.ApplicationEngine.ApplicationTest
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
