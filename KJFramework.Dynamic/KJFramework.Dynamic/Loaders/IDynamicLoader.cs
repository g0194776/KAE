using System;

namespace KJFramework.Dynamic.Loaders
{
    /// <summary>
    ///     ��̬������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDynamicLoader
    {
        /// <summary>
        ///     ��̬����
        /// </summary>
        /// <param name="args">���ز���</param>
        void Load(params String[] args);
        /// <summary>
        ///     ���سɹ��¼�
        /// </summary>
        event EventHandler LoadSuccessfully;
        /// <summary>
        ///     ����ʧ���¼�
        /// </summary>
        event EventHandler LoadFailed;
    }
}