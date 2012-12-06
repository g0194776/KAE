using System.Collections.Generic;
using KJFramework.ServiceModel.Identity;

namespace KJFramework.ServiceModel.Comparers
{
    public class MessageIdentityComparer : EqualityComparer<MessageIdentity>
    {
        #region Overrides of EqualityComparer<MessageIdentity>

        /// <summary>
        /// When overridden in a derived class, determines whether two objects of type <paramref name="T"/> are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public override bool Equals(MessageIdentity x, MessageIdentity y)
        {
            if (x.ProtocolId != y.ProtocolId) return false;
            if (x.ServiceId != y.ServiceId) return false;
            if (x.DetailsId != y.DetailsId) return false;
            return true;
        }

        /// <summary>
        /// When overridden in a derived class, serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The object for which to get a hash code.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public override int GetHashCode(MessageIdentity obj)
        {
            return obj.ToString().GetHashCode();
        }

        #endregion
    }
}