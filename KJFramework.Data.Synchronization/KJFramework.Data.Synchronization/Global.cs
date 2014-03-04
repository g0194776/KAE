using System;
using KJFramework.Data.Synchronization.Configurations;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.Data.Synchronization
{
    internal static class Global
    {
        public static readonly TimeSpan TranTimeout = TimeSpan.Parse(SyncDataConfigSection.Current.Settings.TranTimeout);
        public static readonly int TranChkInterval = int.Parse(SyncDataConfigSection.Current.Settings.TranChkInterval);
        public static readonly MetadataTransactionManager TransactionManager = new MetadataTransactionManager(new TransactionIdentityComparer()); 
        public static readonly MetadataProtocolStack ProtocolStack = new MetadataProtocolStack();
    }
}