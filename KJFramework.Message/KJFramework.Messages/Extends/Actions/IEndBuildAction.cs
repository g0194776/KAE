namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ��Ϣ��β���춯�����ṩ����صĻ���������
    /// </summary>
    public interface IEndBuildAction : IBuildAction
    {
        /// <summary>
        ///     ����һ����Ϣ��β
        ///     <para>* ��Ϣ��β����涨�� ����ͷ�����ݵ������������ڵ���50000��</para>
        /// </summary>
        /// <param name="data">�����ֶ�Ԫ����</param>
        /// <returns>���ؽ�β</returns>
        byte[] Bind(byte[] data);
        /// <summary>
        ///     ��ȡ��Ϣ��β
        ///     <para>* ��Ϣ��β����涨�� ����ͷ�����ݵ������������ڵ���50000��</para>
        /// </summary>
        /// <typeparam name="T">��Ϣ��β����</typeparam>
        /// <param name="data">Ԫ����</param>
        /// <returns>������ȡ������Ϣ��β</returns>
        T Pickup<T>(byte[] data);
    }
}