namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ��Ϣͷ�����춯��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IHeadBuildAction : IBuildAction
    {
        /// <summary>
        ///     ����һ����Ϣͷ��
        ///     <para>* ��Ϣͷ������涨�� ����ͷ�����ݵ���������С��0��</para>
        /// </summary>
        /// <param name="data">�����ֶ�Ԫ����</param>
        /// <returns>����ͷ��</returns>
        byte[] Bind(byte[] data);
        /// <summary>
        ///     ��ȡ��Ϣͷ��
        ///     <para>* ��Ϣͷ������涨�� ����ͷ�����ݵ���������С��0��</para>
        /// </summary>
        /// <typeparam name="T">��Ϣͷ������</typeparam>
        /// <param name="data">Ԫ����</param>
        /// <returns>������ȡ������Ϣͷ��</returns>
        T Pickup<T>(byte[] data);
    }
}