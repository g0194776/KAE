using KJFramework.Net.Exception;
using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.Data.Synchronization.Messages
{
    /// <summary>
    ///     同步数据框架协议栈
    /// </summary>
    public class SyncDataProtocolStack : BusinessProtocolStack
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