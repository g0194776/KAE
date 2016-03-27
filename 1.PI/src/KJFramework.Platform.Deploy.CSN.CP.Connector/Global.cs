using KJFramework.Platform.Deploy.CSN.Common.Caches;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector
{
    internal class Global
    {
        public static CSNProtocolStack ProtocolStack;
        public static DBDataCacheFactory DBCacheFactory = new DBDataCacheFactory();
    }
}