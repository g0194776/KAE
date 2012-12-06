using KJFramework.Net.EventArgs;

namespace KJFramework.Net
{
    /// <summary>
    ///     ֧�ָ��µ�Ԫ�ӿڣ��ṩ��һ��������µ���ػ���������
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        ///     ִ�и��²���
        /// </summary>
        bool Update();
        /// <summary>
        ///     ִ�и��²���
        /// </summary>
        /// <param name="url">
        ///     �����˰汾�������ļ���ַ
        ///            * ���ļ�һ��ΪXML��ʽ��
        /// </param>
        bool Update(string url);
        /// <summary>
        ///     ����״̬�¼�
        /// </summary>
        event DelegateUpdateProcessing UpdateProcessing;
    }
}