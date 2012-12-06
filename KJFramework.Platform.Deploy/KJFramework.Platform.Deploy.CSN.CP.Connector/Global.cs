using KJFramework.Net.Cloud.Nodes;
using KJFramework.Platform.Deploy.CSN.CP.Connector.Caches;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;

namespace KJFramework.Platform.Deploy.CSN.CP.Connector
{
    internal class Global
    {
        public static NetworkNode<CSNMessage> ClientNode;
        public static ConnectorComponent ComponentInstance;
        public static DBMatcherComponent DBComponentInstance;
        public static DataCacheManager<DataTable> DBCacheManager = new DataCacheManager<DataTable>();
        public static DBDataCacheFactory DBCacheFactory = new DBDataCacheFactory();
    }
}