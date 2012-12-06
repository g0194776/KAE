using System.Net;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     Ԫ����ҳ�涯��Ԫ�ӿڣ��ṩ�˵���صĻ���������
    /// </summary>
    public interface IHttpMetadataPageAction
    {
        /// <summary>
        ///     ��ȡ��Լ����
        /// </summary>
        IContractDescription ContractDescription { get; }
        /// <summary>
        ///     ִ�ж���
        /// </summary>
        /// <param name="httpListenerRequest">HTTP��������</param>
        string Execute(HttpListenerRequest httpListenerRequest);
    }
}