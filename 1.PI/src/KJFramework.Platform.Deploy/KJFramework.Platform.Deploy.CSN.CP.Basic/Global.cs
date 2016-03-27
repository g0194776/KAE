using System;
using KJFramework.Net.Cloud.Nodes;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.CP.Basic
{
    internal class Global
    {
        public static NetworkNode<DSCMessage> CenterNode;
        public static Guid CenterId;
        public static BasicComponent ConnectorInstance;
    }
}