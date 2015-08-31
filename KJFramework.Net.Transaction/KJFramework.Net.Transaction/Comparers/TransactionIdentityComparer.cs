﻿using System.Collections.Generic;
using System.Net;
using KJFramework.Net.EndPoints;
using KJFramework.Net.Enums;
using KJFramework.Net.Identities;

namespace KJFramework.Net.Transaction.Comparers
{
    /// <summary>
    ///   TCP网络唯一事务标示比较器
    /// </summary>
    public class TransactionIdentityComparer : EqualityComparer<TransactionIdentity>
    {
        #region Overrides of EqualityComparer<TransactionIdentity>

        /// <summary>
        /// When overridden in a derived class, determines whether two objects of type <paramref name="T"/> are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
        public override bool Equals(TransactionIdentity x, TransactionIdentity y)
        {
            if (x.IdentityType == TransactionIdentityTypes.TCP)
            {
                if (x.IsOneway != y.IsOneway) return false;
                if (x.MessageId != y.MessageId) return false;
                if (((IPEndPoint)x.EndPoint).Port != ((IPEndPoint)y.EndPoint).Port) return false;
                if (!((IPEndPoint)x.EndPoint).Address.Equals(((IPEndPoint)y.EndPoint).Address)) return false;
            }
            else if (x.IdentityType == TransactionIdentityTypes.NamedPipe)
            {
                if (x.IsOneway != y.IsOneway) return false;
                if (x.MessageId != y.MessageId) return false;
                if (((NamedPipeEndPoint)x.EndPoint).GetPipeCodeId() != ((NamedPipeEndPoint)y.EndPoint).GetPipeCodeId()) return false;
            }
            return true;
        }

        /// <summary>
        /// When overridden in a derived class, serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        /// </exception>
        public override int GetHashCode(TransactionIdentity obj)
        {
            //return obj.GetHashCode();
            return obj.ToString().GetHashCode();
        }

        #endregion
    }
}