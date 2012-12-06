using System;
using KJFramework.Plugin;

namespace KJFramework
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ�˽�һ�������ļ�������һ�������ϵĻ���������
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        ///     ��ȡ�����ļ�
        /// </summary>
        /// <param name="path">�����ļ�ȫ·��</param>
        /// <returns>����һ��������</returns>
        IConfiger Config(String path);
        /// <summary>
        ///     ��ȡ�����ļ�
        /// </summary>
        /// <param name="path">�����ļ�ȫ·��</param>
        /// <param name="args">����չ�����ò���</param>
        /// <returns>����һ��������</returns>
        IConfiger Config(String path, Object args);
        /// <summary>
        ///     ��ȡ�����ļ�
        /// </summary>
        /// <param name="path">�����ļ�ȫ·��</param>
        /// <param name="args">����չ�����ò�������</param>
        /// <returns>����һ��������</returns>
        IConfiger Config(String path, Object[] args);
        /// <summary>
        ///     ��ȡ�Ѿ����ڵ�������
        /// </summary>
        /// <returns>�����Ѿ����ڵ������������δ�������ã��򷵻�null��</returns>
        IConfiger GetConfiger();
    }
}