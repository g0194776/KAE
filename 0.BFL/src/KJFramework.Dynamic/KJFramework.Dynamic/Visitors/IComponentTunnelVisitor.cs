namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     ���ͨ��������Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IComponentTunnelVisitor
    {
        /// <summary>
        ///     ��ȡָ����������
        /// </summary>
        /// <param name="componentName">�������</param>
        /// <returns>������������</returns>
        T GetTunnel<T>(string componentName) where T : class;
    }
}