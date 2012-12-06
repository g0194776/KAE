using System;
using System.Collections.Concurrent;
using KJFramework.IO.Helper;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;
using KJFramework.ServiceModel.Bussiness.Default.Metadata;
using KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions;
using KJFramework.ServiceModel.Bussiness.Default.Services;

namespace KJFramework.ServiceModel.Bussiness.Default.Centers
{
    /// <summary>
    ///     服务中心，用于记录并控制当前所有已经注册的服务。
    /// </summary>
    internal static class ServiceCenter
    {
        #region Members

        private static ConcurrentDictionary<String, ServiceHandle> _services = new ConcurrentDictionary<string, ServiceHandle>();
        private static IMetadataExchangeNode _metadataExchangeNode = new HttpMetadataExchangeNode();
        /// <summary>
        ///     获取元数据交换节点
        /// </summary>
        internal static IMetadataExchangeNode MetadataNode 
        {
            get { return _metadataExchangeNode; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///     注册一个服务句柄到服务中心
        /// </summary>
        /// <param name="handle">服务句柄</param>
        public static void Regist(ServiceHandle handle)
        {
            if (handle == null || handle.Uri == null) throw new System.Exception("无法注册一个服务到服务中心，非法的参数。");
            if (!_services.ContainsKey(handle.Uri.GetServiceUri()))
            {
                if (!_services.TryAdd(handle.Uri.GetServiceUri(), handle)) throw new System.Exception("无法加入到服务中心。");
                if (handle.IsSupportExchange)
                {
                    #if (DEBUG)
                    {
                        ConsoleHelper.PrintLine("Opening contract metadata exchange #Contract Name :  " + handle.CreateDescription().Infomation.ContractName, ConsoleColor.DarkCyan);
                        ConsoleHelper.PrintLine("=>Details...", ConsoleColor.DarkCyan);
                    }
                    #endif
                    IContractDescription contractDescription = handle.CreateDescription();
                    _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName, new HttpMetadataContractRootPageAction(contractDescription));
                    _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/", new HttpMetadataContractRootPageAction(contractDescription));
                    _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/Preview", new HttpMetadataContractPreviewPageAction(contractDescription));
                    _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/Preview/", new HttpMetadataContractPreviewPageAction(contractDescription));
                    _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/Metadata", new HttpMetadataContractGeneratePageAction(contractDescription));
                    _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/Metadata/", new HttpMetadataContractGeneratePageAction(contractDescription));
                    foreach (IDescriptionMethod method in contractDescription.GetMethods())
                    {
                        _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/Operations.aspx?Name=" + method.Name.Substring(method.Name.LastIndexOf('.') + 1) + "&Token=" + method.MethodToken, new HttpMetadataContractOperationPageAction(contractDescription, method.MethodToken));
                        _metadataExchangeNode.Regist("/" + contractDescription.Infomation.ContractName + "/Metadata/Operations.aspx?Name=" + method.Name.Substring(method.Name.LastIndexOf('.') + 1) + "&Token=" + method.MethodToken, new HttpContractOperationArgumentPageAction(contractDescription, method.MethodToken));
                    }
                }
            }
        }

        /// <summary>
        ///     从服务中心注销一个服务句柄
        /// </summary>
        /// <param name="uri">服务句柄的URI</param>
        public static void UnRegist(String uri)
        {
            if (String.IsNullOrEmpty(uri))
            {
                return;
            }
            if (_services.ContainsKey(uri))
            {
                ServiceHandle serviceHandle;
                if(_services.TryRemove(uri, out serviceHandle))
                {
                    if (serviceHandle.IsSupportExchange)
                    {
                        IContractDescription contractDescription = serviceHandle.CreateDescription();
                        _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName);
                        _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName + "/");
                        _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName + "/Preview");
                        _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName + "/Preview/");
                        _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName + "/Metadata");
                        _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName + "/Metadata/");
                        foreach (IDescriptionMethod method in contractDescription.GetMethods())
                        {
                            _metadataExchangeNode.UnRegist("/" + contractDescription.Infomation.ContractName + "/Operations.aspx?Name=" + method.Name.Substring(method.Name.LastIndexOf('.') + 1) + "&Token=" + method.MethodToken);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     通过一个资源地址，获取对应的服务句柄
        /// </summary>
        /// <param name="uri">资源地址</param>
        /// <returns>返回服务句柄</returns>
        public static ServiceHandle GetHandle(String uri)
        {
            if (String.IsNullOrEmpty(uri))
            {
                return null;
            }
            ServiceHandle handle;
            if (_services.TryGetValue(uri, out handle))
            {
                return handle;
            }
            return null;
        }

        #endregion
    }
}