namespace KJFramework.Platform.Deploy.Maintenance
{
    /// <summary>
    ///     ��ά���Ķ�̬����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IMaintenanceDynamicService
    {
        /// <summary>
        ///     ��ʼ����
        /// </summary>
        void StartHeartBeat();
        /// <summary>
        ///     ֹͣ����
        /// </summary>
        void StopHeartBeat();
    }
}