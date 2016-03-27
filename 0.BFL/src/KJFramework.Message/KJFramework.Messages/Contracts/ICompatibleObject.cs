namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     �ɼ��ݶ���Ԫ�ӿڣ� �ṩ����صĻ���������
    /// </summary>
    public interface ICompatibleObject
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ����δ֪������
        /// </summary>
        bool HasParameter { get; }
        /// <summary>
        ///     ��ȡ����ָ����ŵ�δ֪����
        /// </summary>
        /// <param name="id">���</param>
        /// <returns>����δ֪����</returns>
        IUnknownParameter GetParameter(int id);
        /// <summary>
        ///     ��ȡ����δ֪����
        /// </summary>
        /// <returns>����δ֪��������</returns>
        IUnknownParameter[] GetParameters();
    }
}