namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     ��Լ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IContractDescription
    {
        /// <summary>
        ///      ��ȡ���з�����Լ�б�����Ĳ�������
        /// </summary>
        /// <returns>���ز�����������</returns>
        IDescriptionMethod[] GetMethods();
        /// <summary>
        ///     ��ȡ������Լ��Ϣ
        /// </summary>
        IContractInfomation Infomation { get; }
    }
}