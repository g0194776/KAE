using KJFramework.Messages.Contracts;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��Ϣ��������</typeparam>
    public interface IInputChannel<T> : IOnewayChannel<T>
        where T : IntellectObject
    {
    }
}