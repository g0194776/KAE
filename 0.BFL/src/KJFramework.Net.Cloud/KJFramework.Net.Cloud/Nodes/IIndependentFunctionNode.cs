using KJFramework.Dynamic.Components;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     �������ܽڵ�Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public interface IIndependentFunctionNode<T> : IFunctionNode<T>, IDynamicDomainComponent
    {

    }
}