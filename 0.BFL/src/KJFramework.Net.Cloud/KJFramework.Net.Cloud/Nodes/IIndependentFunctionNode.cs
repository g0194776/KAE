using KJFramework.Dynamic.Components;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     独立功能节点元接口，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public interface IIndependentFunctionNode<T> : IFunctionNode<T>, IDynamicDomainComponent
    {

    }
}