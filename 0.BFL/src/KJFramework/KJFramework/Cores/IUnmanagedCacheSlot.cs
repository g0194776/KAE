using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     ���йܻ����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IUnmanagedCacheSlot
    {
        /// <summary>
        ///     ��ȡ�ڴ���
        /// </summary>
        IntPtr Handle { get; }
        /// <summary>
        ///     ������ǰ���й��ڴ�
        /// </summary>
        void Discard();
        /// <summary>
        ///     ��ȡ������Լ
        /// </summary>
        /// <returns>����������Լ</returns>
        ICacheLease GetLease();
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        /// <returns>���ػ�������</returns>
        byte[] GetValue();
        /// <summary>
        ///     ���û�������
        /// </summary>
        /// <param name="data">Ҫ���õ�����</param>
        void SetValue(byte[] data);
    }
}