using System;
using KJFramework.Net.Cloud.Processors;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.ProtocolStack.Services;

namespace KJFramework.Platform.Deploy.SMC.CP.Connector.Processors.Services
{
    /// <summary>
    ///     动态服务注册请求消息处理器
    /// </summary>
    public class DSRegistRequestMessageProcessor : FunctionProcessor<DynamicServiceMessage>
    {
        #region Overrides of FunctionProcessor<DynamicServiceMessage>

        public override DynamicServiceMessage Process(Guid id, DynamicServiceMessage message)
        {
            DynamicServiceRegistRequestMessage msg = (DynamicServiceRegistRequestMessage)message;
            bool result = ServicePerformancer.Instance.Regist(id, msg);
            DynamicServiceRegistResponseMessage responseMessage = new DynamicServiceRegistResponseMessage();
            responseMessage.Result = result;
            responseMessage.Header.ClientTag = msg.Header.ClientTag;
            Console.WriteLine("=>#New Regist request  : " + msg.ServiceName + ", Result : " + result);
            Console.WriteLine("Details below :");
            Console.WriteLine("Process name: " + msg.ProcessName);
            Console.WriteLine("Name: " + msg.Name);
            Console.WriteLine("Version: " + msg.Version);
            Console.WriteLine("Shell version: " + msg.ShellVersion);
            Console.WriteLine("Description: " + msg.Description);
            Console.WriteLine("Support Domain Performance: " + msg.SupportDomainPerformance);
            Console.WriteLine("Component details below......");
            Console.WriteLine("Component count: " + msg.ComponentCount);
            if (msg.ComponentCount > 0)
            {
                foreach (ComponentDetailItem item in msg.Items)
                {
                    Console.WriteLine();
                    Console.WriteLine("#Name: " + item.Name);
                    Console.WriteLine("#Service name: " + item.ServiceName);
                    Console.WriteLine("#Health status: " + item.ServiceName);
                    Console.WriteLine("#Version: " + item.Version);
                    Console.WriteLine("#Description: " + item.Description);
                    Console.WriteLine("#Catalog name: " + item.CatalogName);
                }
            }
            return responseMessage;
        }

        #endregion
    }
}