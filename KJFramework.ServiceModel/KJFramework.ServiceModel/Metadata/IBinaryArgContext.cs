namespace KJFramework.ServiceModel.Metadata
{
    /// <summary>
    ///     �����Ʋ���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IBinaryArgContext
    {
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�Ĳ����Ƿ����δ�׳����쳣
        /// </summary>
        bool HasException { get; set; }
        /// <summary>
        ///     ��ȡ�������쳣��Ϣ
        /// </summary>
        System.Exception Exception { get; set; }
        /// <summary>
        ///     ��ȡ�����ò���Ψһ���
        /// </summary>
        byte Id { get; set; }
        /// <summary>
        ///     ��ȡ�����ö����Ʋ���������Ԫ����
        /// </summary>
        byte[] Data { get; set; }
    }
}