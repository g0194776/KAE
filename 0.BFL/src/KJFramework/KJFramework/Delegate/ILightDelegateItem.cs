using System;

namespace KJFramework.Delegate
{
    /// <summary>
    ///     ����ί����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ILightDelegateItem : IDisposable
    {
        /// <summary>
        ///     ��ȡ�����ø�������
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     ����
        /// </summary>
        /// <param name="objs">���в���</param>
        void Execute(params Object[] objs);
    }
}