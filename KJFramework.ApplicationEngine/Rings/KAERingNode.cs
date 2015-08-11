using System;

namespace KJFramework.ApplicationEngine.Rings
{
    /// <summary>
    ///     KAE系统内部实现所需要的圆环节点
    /// </summary>
    public sealed class KAERingNode : KAEHostNode
    {
        #region Constructor.

        /// <summary>
        ///     KAE系统内部实现所需要的圆环节点
        /// </summary>
        /// <param name="mixedAddress">格式为: 远程终结点地址;KPP的唯一ID</param>
        public KAERingNode(string mixedAddress)
            : base(mixedAddress.Split(new []{";"}, StringSplitOptions.RemoveEmptyEntries)[0])
        {
            KPPUniqueId = Guid.Parse(mixedAddress.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)[1]);
        }

        #endregion

        #region Members.

        /// <summary>
        ///     获取或设置KPP的UniqueId
        /// </summary>
        public Guid KPPUniqueId { get; set; }

        #endregion
    }
}