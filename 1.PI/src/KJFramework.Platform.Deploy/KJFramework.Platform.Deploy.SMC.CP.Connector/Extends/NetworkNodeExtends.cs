using System;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Cloud.Nodes;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Extends
{
    public static class NetworkNodeExtends
    {
        public static void Send<T>(this NetworkNode<T> networkNode, T message, Guid channelId)
            where T : IntellectObject
        {
            message.Bind();
            networkNode.Send(channelId, message.Body);
        }
    }
}