namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ��Ϣβ�����춯�������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class EndBuildAction : BuildAction, IEndBuildAction
    {
        #region Implementation of IEndBuildAction

        /// <summary>
        ///     ����һ����Ϣ��β
        ///     <para>* ��Ϣ��β����涨�� ����ͷ�����ݵ������������ڵ���50000��</para>
        /// </summary>
        /// <param name="data">�����ֶ�Ԫ����</param>
        /// <returns>���ؽ�β</returns>
        public abstract byte[] Bind(byte[] data);

        /// <summary>
        ///     ��ȡ��Ϣ��β
        ///     <para>* ��Ϣ��β����涨�� ����ͷ�����ݵ������������ڵ���50000��</para>
        /// </summary>
        /// <typeparam name="T">��Ϣ��β����</typeparam>
        /// <param name="data">Ԫ����</param>
        /// <returns>������ȡ������Ϣ��β</returns>
        public abstract T Pickup<T>(byte[] data);

        #endregion
    }
}