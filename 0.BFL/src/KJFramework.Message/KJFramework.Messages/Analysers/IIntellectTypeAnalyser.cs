namespace KJFramework.Messages.Analysers
{
    /// <summary>
    ///     �������ͷ�����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface IIntellectTypeAnalyser<out T, in K>
    {
        /// <summary>
        ///     ����һ�������е�������������
        /// </summary>
        /// <param name="type">Ҫ����������</param>
        /// <returns>���ط����Ľ��</returns>
        T Analyse(K type);
        /// <summary>
        ///     ��յ�ǰ���еķ������
        /// </summary>
        void Clear();
    }
}