using System;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector
{
    public class Global
    {
        public static NetworkNode<DynamicServiceMessage> ServiceNetworkNode;
        public static NetworkNode<DSCMessage> CenterNetworkNode;
        public static Guid CenterId;
        public static ConnectorComponent ConnectorInstance;
    }
}