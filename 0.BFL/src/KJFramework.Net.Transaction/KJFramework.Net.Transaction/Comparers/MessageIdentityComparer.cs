using System.Collections.Generic;
using KJFramework.Net.Identities;

namespace KJFramework.Net.Transaction.Comparers
{
    /// <summary>
    ///     消息协议比较器
    /// </summary>
    public class MessageIdentityComparer : EqualityComparer<MessageIdentity>
    {
        #region Overrides of EqualityComparer<MessageIdentityComparer>

        /// <summary>
        /// 当在派生类中被重写时，确定两个类型为 <paramref name="T"/> 的对象是否相等。
        /// </summary>
        /// <returns>
        /// 如果指定的对象相等，则为 true；否则为 false。
        /// </returns>
        /// <param name="x">要比较的第一个对象。</param><param name="y">要比较的第二个对象。</param>
        public override bool Equals(MessageIdentity x, MessageIdentity y)
        {
            if (x.ProtocolId != y.ProtocolId) return false;
            if (x.ServiceId != y.ServiceId) return false;
            if (x.DetailsId != y.DetailsId) return false;
            return true;
        }

        /// <summary>
        /// 当在派生类中被重写时，用作指定对象的哈希算法和数据结构（如哈希表）的哈希函数。
        /// </summary>
        /// <returns>
        /// 指定对象的哈希代码。
        /// </returns>
        /// <param name="obj">要为其获取哈希代码的对象。</param><exception cref="T:System.ArgumentNullException"><paramref name="obj"/> 的类型为引用类型，<paramref name="obj"/> 为 null。</exception>
        public override int GetHashCode(MessageIdentity obj)
        {
            return obj.ToString().GetHashCode();
        }

        #endregion
    }
}