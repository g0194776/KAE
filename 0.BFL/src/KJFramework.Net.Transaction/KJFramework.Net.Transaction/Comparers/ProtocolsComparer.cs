using System.Collections.Generic;

namespace KJFramework.Net.Transaction.Comparers
{
    public class ProtocolsComparer : EqualityComparer<Objects.Protocols>
    {
        #region Overrides of EqualityComparer<Protocols>

        /// <summary>
        /// When overridden in a derived class, determines whether two objects of type 
        /// <paramref name="T"/> are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public override bool Equals(Objects.Protocols x, Objects.Protocols y)
        {
            return (x.ProtocolId == y.ProtocolId && x.ServiceId == y.ServiceId && x.DetailsId == y.DetailsId);
        }

        /// <summary>
        /// When overridden in a derived class, serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The object for which to get a hash code.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public override int GetHashCode(Objects.Protocols obj)
        {
            return obj.ToString().GetHashCode();
        }

        #endregion
    }
}