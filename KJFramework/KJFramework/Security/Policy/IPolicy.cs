namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     ���Խӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ��Ѿ�������
        /// </summary>
        bool Deployed { get; }
        /// <summary>
        ///     ��ȡ������Ϣ
        /// </summary>
        IPolicyInfomation Infomation { get; }
    }
}