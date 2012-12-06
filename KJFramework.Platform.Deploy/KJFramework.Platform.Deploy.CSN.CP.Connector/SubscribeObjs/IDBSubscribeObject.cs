namespace KJFramework.Platform.Deploy.CSN.CP.Connector.SubscribeObjs
{
    /// <summary>
    ///     ���ݿⶩ�Ķ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDBSubscribeObject : ISubscribeObject
    {
        /// <summary>
        ///     �������ݿ�
        /// </summary>
        /// <param name="db">���ݿ����Ƽ���</param>
        void AddSubscribe(params string[] db);
        /// <summary>
        ///     ����ָ��DB�е����ݱ�
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="tables">���ݱ����Ƽ���</param>
        void AddSubscribe(string db, params string[] tables);
        /// <summary>
        ///     ����ָ��DB�У�ָ�����ݱ�������
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">���ݱ�����</param>
        /// <param name="column">�����Ƽ���</param>
        void AddSubscribe(string db, string table, params string[] column);
        /// <summary>
        ///     �Ƴ�����ָ��DB�Ķ���
        /// </summary>
        /// <param name="db">���ݿ����Ƽ���</param>
        void RemoveSubscribe(params string[] db);
        /// <summary>
        ///     �Ƴ������ָ��DB�б�Ķ���
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="tables">���ݱ����Ƽ���</param>
        void RemoveSubscribe(string db, params string[] tables);
        /// <summary>
        ///     �Ƴ������ָ��DBָ������ֶζ���
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">���ݱ����Ƽ���</param>
        /// <param name="column">�����Ƽ���</param>
        void RemoveSubscribe(string db, string table, params string[] column);
        /// <summary>
        ///     ��⵱ǰ�����Ƿ�����ָ�����ݿ�
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <returns>�����Ƿ��ĵ�״̬</returns>
        bool CheckIntersting(string db);
        /// <summary>
        ///     ��⵱ǰ�����Ƿ�����ָ�����ݿ��������ݿ��еı�
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">����</param>
        /// <returns>�����Ƿ��ĵ�״̬</returns>
        bool CheckIntersting(string db, string table);
        /// <summary>
        ///     ��⵱ǰ�����Ƿ�����ָ�����ݿ��������ݿ��еı��Լ������
        /// </summary>
        /// <param name="db">���ݿ�����</param>
        /// <param name="table">����</param>
        /// <param name="column">����</param>
        /// <returns>�����Ƿ��ĵ�״̬</returns>
        bool CheckIntersting(string db, string table, string column);
    }
}