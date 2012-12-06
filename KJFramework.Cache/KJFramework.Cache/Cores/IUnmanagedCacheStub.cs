using System;

namespace KJFramework.Cache.Cores
{
    /// <summary>
    ///     ���йܻ�����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    internal interface IUnmanagedCacheStub : ICacheStub<byte[]>
    {
        /// <summary>
        ///     ��ȡ�ڴ���
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        ///     ��ȡ��ǰ�ڲ��Ļ���ʹ����
        /// </summary>
        int CurrentSize { get; }
        /// <summary>
        ///     ��ȡ��ǰ������������
        /// </summary>
        int MaxSize { get; }
        /// <summary>
        ///     ������ǰ����
        /// </summary>
        void Discard();
    }
}