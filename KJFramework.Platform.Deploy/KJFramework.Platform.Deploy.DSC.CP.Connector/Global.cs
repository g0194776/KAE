using KJFramework.Net.Cloud.Nodes;
using KJFramework.Platform.Client.ProtocolStack;
using KJFramework.Platform.Deploy.DSC.CP.Client;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector
{
    public class Global
    {
        public static NetworkNode<ClientMessage> ClientCommnicationNode;
        public static ClientProxyComponent ClientComponentInstance;
        public static NetworkNode<DSCMessage> CommnicationNode;
        public static ConnectorComponent ComponentInstance;
    }
}