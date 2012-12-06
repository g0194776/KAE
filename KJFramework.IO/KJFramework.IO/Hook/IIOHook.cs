using KJFramework.IO.EventArgs;

namespace KJFramework.IO.Hook
{
    /// <summary>
    ///        输入输出钩子元接口，提供了相关的基本操作。
    /// </summary>
    public interface IIOHook
    {    
        /// <summary>
        ///        条件满足触发事件
        /// </summary>
        event DelegateFulfil Fulfilled;
    }
}
