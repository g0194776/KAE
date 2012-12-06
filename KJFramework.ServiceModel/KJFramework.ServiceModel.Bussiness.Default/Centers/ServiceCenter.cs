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
    ///     �������ģ����ڼ�¼�����Ƶ�ǰ�����Ѿ�ע��ķ���
    /// </summary>
    internal static class ServiceCenter
    {
        #region Members

        private static ConcurrentDictionary<String, ServiceHandle> _services = new ConcurrentDictionary<string, ServiceHandle>();
        private static IMetadataExchangeNode _metadataExchangeNode = new HttpMetadataExchangeNode();
        /// <summary>
        ///     ��ȡԪ���ݽ����ڵ�
        /// </summary>
        internal static IMetadataExchangeNode MetadataNode 
        {
            get { return _metadataExchangeNode; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///     ע��һ������������������
        /// </summary>
        /// <param name="handle">������</param>
        public static void Regist(ServiceHandle handle)
        {
            if (handle == null || handle.Uri == null) throw new System.Exception("�޷�ע��һ�����񵽷������ģ��Ƿ��Ĳ�����");
            if (!_services.ContainsKey(handle.Uri.GetServiceUri()))
            {
                if (!_services.TryAdd(handle.Uri.GetServiceUri(), handle)) throw new System.Exception("�޷����뵽�������ġ�");
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
        ///     �ӷ�������ע��һ��������
        /// </summary>
        /// <param name="uri">��������URI</param>
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
        ///     ͨ��һ����Դ��ַ����ȡ��Ӧ�ķ�����
        /// </summary>
        /// <param name="uri">��Դ��ַ</param>
        /// <returns>���ط�����</returns>
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