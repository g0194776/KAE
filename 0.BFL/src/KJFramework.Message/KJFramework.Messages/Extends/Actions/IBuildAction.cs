using System;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     ���춯��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IBuildAction : IDisposable
    {
        /// <summary>
        ///     ��ȡ���춯�����
        /// </summary>
        int BuildId { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ���춯���Ƿ�����ִ��ָ��������
        /// </summary>
        bool IsBuilding { get; }
    }
}