namespace KJFramework.Engin
{
    /// <summary>
    ///     ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IEngin : IControlable
    {
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        void Initialize();
        /// <summary>
        ///     ��ȡһ��ֵ����ֵָʾ�˵�ǰ�����Ƿ��Ѿ���������еĵ��ȹ�����
        /// </summary>
        bool IsFinish { get; }
    }
}