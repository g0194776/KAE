namespace KJFramework.Game.Components
{
    /// <summary>
    ///     ���ṩ������ʽԪ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">�����Ա����</typeparam>
    public interface IListable<T>
    {
        /// <summary>
        ///     ��һ����Ա
        /// </summary>
        T Next { get; set; }
        /// <summary>
        ///     ��һ����Ա
        /// </summary>
        T Previous { get; set; }
        /// <summary>
        ///     ��ǰ��Ա
        /// </summary>
        T Current { get; set; }
    }
}