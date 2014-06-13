using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KJFramework.ApplicationEngine.Rings
{
    /// <summary>
    ///     一致性HASH圆环
    /// </summary>
    internal sealed class KetamaRing
    {
        #region Members.

        private int numReps = 160;
        private readonly long[] _keys;
        private readonly Dictionary<long, KAEHostNode> _ketamaNodes;

        #endregion

        #region Constructor.

        public KetamaRing(List<KAEHostNode> nodes)
        {
            _ketamaNodes = new Dictionary<long, KAEHostNode>();
            //对所有节点，生成nCopies个虚拟结点
            for (int j = 0; j < nodes.Count; j++)
            {
                KAEHostNode node = nodes[j];
                //每四个虚拟结点为一组
                for (int i = 0; i < numReps / 4; i++)
                {
                    byte[] digest = ComputeMd5(String.Format("{0}_{1}", node, i));

                    /** Md5是一个16字节长度的数组，将16字节的数组每四个字节一组，
                         * 分别对应一个虚拟结点，这就是为什么上面把虚拟结点四个划分一组的原因*/
                    for (int h = 0; h < 4; h++)
                    {
                        long rv = ((long)(digest[3 + h * 4] & 0xFF) << 24)
                                  | ((long)(digest[2 + h * 4] & 0xFF) << 16)
                                  | ((long)(digest[1 + h * 4] & 0xFF) << 8)
                                  | ((long)digest[0 + h * 4] & 0xFF);

                        rv = rv & 0xffffffffL; /* Truncate to 32-bits */
                        _ketamaNodes[rv] = node;
                    }
                }
            }

            _keys = _ketamaNodes.Keys.OrderBy(p => p).ToArray();
        }

        #endregion

        #region Methods.

        public KAEHostNode GetWorkerNode(string k)
        {
            byte[] digest = ComputeMd5(k);
            return GetNodeInner(Hash(digest, 0));
        }

        private KAEHostNode GetNodeInner(long hash)
        {
            if (_ketamaNodes.Count == 0) return null;
            int near;
            int index = Array.BinarySearch(_keys, hash);
            if (index < 0)
            {
                near = (~index);
                if (near == _keys.Length)
                    near = 0;
            }
            else
            {
                near = index;
            }

            return _ketamaNodes[_keys[near]];
        }

        public static long Hash(byte[] digest, int nTime)
        {
            long rv = ((long)(digest[3 + nTime * 4] & 0xFF) << 24)
                      | ((long)(digest[2 + nTime * 4] & 0xFF) << 16)
                      | ((long)(digest[1 + nTime * 4] & 0xFF) << 8)
                      | ((long)digest[0 + nTime * 4] & 0xFF);

            return rv & 0xffffffffL; /* Truncate to 32-bits */
        }

        public static byte[] ComputeMd5(string k)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] keyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(k));
            md5.Clear();
            return keyBytes;
        }

        #endregion
    }
}