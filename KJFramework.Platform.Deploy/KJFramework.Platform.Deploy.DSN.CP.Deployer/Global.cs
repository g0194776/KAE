using System;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Platform.Deploy.DSN.ProtocolStack;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer
{
    internal class Global
    {
        public static NetworkNode<DSNMessage> CommunicationNode;
        public static string MachineName = Environment.MachineName + "-DEPOYER";
    }
}