using System;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Connector
{
    public class Global
    {
        public static Guid CenterId;
        public static NetworkNode<DSCMessage> CenterNode;
        public static string MachineName = Environment.MachineName + "-DEPOYER";
        public static ConnectorComponent ConnectorInstance;
    }
}