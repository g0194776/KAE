using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///      ��Լ���ݷ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IMetadataExchange
    {
        /// <summary>
        ///     ����һ�����ڵ�ǰ����ڵ����Լ����
        /// </summary>
        /// <returns>������Լ����</returns>
        IContractDescription CreateDescription();
        /// <summary>
        ///      ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ����ڵ��Ƿ�֧����ԼԪ���ݽ���
        /// </summary>
        bool IsSupportExchange { get; set; }
    }
}