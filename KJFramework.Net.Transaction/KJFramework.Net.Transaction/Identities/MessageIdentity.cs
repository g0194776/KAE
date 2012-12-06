namespace KJFramework.Net.Transaction.Identities
{
    /// <summary>
    ///     消息唯一标示，提供了相关的基本属性结构
    ///     <para>* 此标示仅用来与客户端通讯时使用</para>
    /// </summary>
    public class MessageIdentity
    {
        /// <summary>
        ///     获取或设置消息大分类编号
        /// </summary>
        public byte ProtocolId { get; set; }
        /// <summary>
        ///     获取或设置消息小分类编号
        /// </summary>
        public byte ServiceId { get; set; }
        /// <summary>
        ///     获取或设置消息详细分类编号
        /// </summary>
        public byte DetailsId { get; set; }
        /// <summary>
        ///     获取或设置当前消息事务唯一编号
        /// </summary>
        public short Tid { get; set; }

        #region Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("(P: {0}, S: {1}, D: {2}, T: {3})", ProtocolId, ServiceId, DetailsId, Tid);
        }

        #endregion
    }
}