using System;
using KJFramework.Data.Synchronization.Configurations;
using KJFramework.Data.Synchronization.Messages;

namespace KJFramework.Data.Synchronization
{
    internal static class Global
    {
        public static readonly TimeSpan TranTimeout = TimeSpan.Parse(SyncDataConfigSection.Current.Settings.TranTimeout);
        public static readonly int TranChkInterval = int.Parse(SyncDataConfigSection.Current.Settings.TranChkInterval);
        public static readonly SyncDataProtocolStack ProtocolStack = new SyncDataProtocolStack();
    }
}