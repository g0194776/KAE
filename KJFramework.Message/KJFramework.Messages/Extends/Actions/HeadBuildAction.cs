namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ��Ϣͷ�����춯�������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class HeadBuildAction : BuildAction, IHeadBuildAction
    {
        #region Implementation of IHeadBuildAction

        /// <summary>
        ///     ����һ����Ϣͷ��
        ///     <para>* ��Ϣͷ������涨�� ����ͷ�����ݵ���������С��0��</para>
        /// </summary>
        /// <param name="data">�����ֶ�Ԫ����</param>
        /// <returns>����ͷ��</returns>
        public abstract byte[] Bind(byte[] data);

        /// <summary>
        ///     ��ȡ��Ϣͷ��
        ///     <para>* ��Ϣͷ������涨�� ����ͷ�����ݵ���������С��0��</para>
        /// </summary>
        /// <typeparam name="T">��Ϣͷ������</typeparam>
        /// <param name="data">Ԫ����</param>
        /// <returns>������ȡ������Ϣͷ��</returns>
        public abstract T Pickup<T>(byte[] data);

        #endregion
    }
}