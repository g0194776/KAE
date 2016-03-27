namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     ���Ȳ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="TRemedy">���ȶ���</typeparam>
    public interface IRemedyPolicy<TRemedy> : IPolicy
    {
        /// <summary>
        ///     ��ȡ���������󲹾ȴ���
        /// </summary>
        int MaxRemedyCount { get; set; }
        /// <summary>
        ///     ����
        /// </summary>
        /// <returns>���ز��ȵĽ��</returns>
        bool Remedy(TRemedy remedyObject);
    }
}