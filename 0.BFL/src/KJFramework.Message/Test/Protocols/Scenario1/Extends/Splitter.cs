using System.Collections.Generic;
using KJFramework.IO.Helper;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Extends.Splitters;

namespace Test.Protocols.Scenario1.Extends
{
    public class Splitter : IMetadataFieldSplitter
    {
        #region Implementation of IMetadataFieldSplitter

        /// <summary>
        ///     �ָ��ֶ�
        /// </summary>
        /// <param name="target">Ŀ���������</param>
        /// <param name="data">��ϢԪ����</param>
        /// <param name="head">��Ϣͷ��ʾ����</param>
        /// <param name="end">��Ϣβ��ʾ����</param>
        /// <returns>���طָ����ֶμ���</returns>
        /// <exception cref="System.Exception">�ָ�ʧ��</exception>
        public Dictionary<int, byte[]> Split(IntellectObject target, byte[] data, out Dictionary<int, byte[]> head, out Dictionary<int, byte[]> end)
        {
            Dictionary<int, byte[]> result = target.Split(ByteArrayHelper.GetNextData(data, 4, data.Length - 4));
            head = new Dictionary<int, byte[]>();
            end = null;
            return result;
        }

        #endregion
    }
}