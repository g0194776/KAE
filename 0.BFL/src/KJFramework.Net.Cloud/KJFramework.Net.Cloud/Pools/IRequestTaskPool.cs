using KJFramework.Net.Cloud.Tasks;
using KJFramework.Net.Exception;

namespace KJFramework.Net.Cloud.Pools
{
    /// <summary>
    ///     ���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IRequestTaskPool<T>
    {
        /// <summary>
        ///     ��ȡ�����õ�ǰ��֧�ֵ������������
        /// </summary>
        int MaxCount { get; set; }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        void Initialzie();
        /// <summary>
        ///     ��һ����������
        /// </summary>
        /// <returns>
        ///     ������������
        ///     <para>* �����ǰ�Ŀ�������Ϊ0�����ͬ���ȴ���</para>
        /// </returns>
        IRequestTask<T> Rent();
        /// <summary>
        ///     �黹һ����������
        /// </summary>
        /// <param name="task">����</param>
        void Giveback(IRequestTask<T> task);
    }
}