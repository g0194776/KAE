using System;

namespace KJFramework.Cores
{
    /// <summary>
    ///     ���йܻ���۾�̬�࣬�ṩ����صĻ���������
    /// </summary>
    public static class UnmanagedCacheSlot
    {
        #region Methods

        /// <summary>
        ///     ����һ���µķ��йܻ����
        /// </summary>
        /// <param name="size">�����������</param>
        /// <returns>���ط��й��ڴ��</returns>
        /// <exception cref="System.OutOfMemoryException">�ڴ����</exception>
        /// <exception cref="System.ArgumentException">��������</exception>
        public static IUnmanagedCacheSlot New(int size)
        {
            return New(size, DateTime.MaxValue);
        }

        /// <summary>
        ///     ����һ���µķ��йܻ����
        /// </summary>
        /// <param name="size">�����������</param>
        /// <param name="expireTime">����ʱ��</param>
        /// <returns>���ط��й��ڴ��</returns>
        /// <exception cref="System.OutOfMemoryException">�ڴ����</exception>
        /// <exception cref="System.ArgumentException">��������</exception>
        public static IUnmanagedCacheSlot New(int size, DateTime expireTime)
        {
            return new UnmanagedCacheStub(size, expireTime);
        }

        /// <summary>
        ///     ����һ���µķ��йܻ����
        /// </summary>
        /// <param name="ptr">�ڴ���</param>
        /// <param name="size">�����������</param>
        /// <param name="useageSize">��ʹ�ô�С</param>
        /// <returns>���ط��й��ڴ��</returns>
        /// <exception cref="System.OutOfMemoryException">�ڴ����</exception>
        /// <exception cref="System.ArgumentException">��������</exception>
        public static IUnmanagedCacheSlot New(IntPtr ptr, int size, int useageSize)
        {
            return new UnmanagedCacheStub(ptr, size, useageSize, DateTime.MaxValue);
        }

        /// <summary>
        ///     ����һ���µķ��йܻ����
        /// </summary>
        /// <param name="ptr">�ڴ���</param>
        /// <param name="size">�����������</param>
        /// <param name="useageSize">��ʹ�ô�С</param>
        /// <param name="expireTime">����ʱ��</param>
        /// <returns>���ط��й��ڴ��</returns>
        /// <exception cref="System.OutOfMemoryException">�ڴ����</exception>
        /// <exception cref="System.ArgumentException">��������</exception>
        public static IUnmanagedCacheSlot New(IntPtr ptr, int size, int useageSize, DateTime expireTime)
        {
            return new UnmanagedCacheStub(ptr, size, useageSize, expireTime);
        }

        #endregion
    }
}