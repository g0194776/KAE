using KJFramework.Net.Exception;
using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.Platform.Deploy.CSN.ProtocolStack
{
    /// <summary>
    ///     配置站节点协议栈，提供了相关的基本操作。
    /// </summary>
    public class CSNProtocolStack : BusinessProtocolStack
    {
        /// <summary>
        ///     初始化
        /// </summary>
        /// <returns>返回初始化的结果</returns>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        public override bool Initialize()
        {
            return true;
        }
    }
}