namespace KJFramework.Game.Components
{
    /// <summary>
    ///     ��֧�ְ�װԪ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IInstallable
    {
        /// <summary>
        ///     ��װ
        /// </summary>
        /// <returns>���ذ�װ��״̬</returns>
        bool Install();
        /// <summary>
        ///     ж��
        /// </summary>
        /// <returns>����ж�ص�״̬</returns>
        bool UnInstall();
    }
}