using KJFramework.Messages.Contracts;

namespace KJFramework.Net.OneWay
{
    /// <summary>
    ///     接收通道元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public interface IInputChannel<T> : IOnewayChannel<T>
        where T : IntellectObject
    {
    }
}