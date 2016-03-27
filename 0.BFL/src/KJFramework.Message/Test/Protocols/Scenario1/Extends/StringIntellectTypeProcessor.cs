using System;
using System.Text;
using KJFramework.IO.Helper;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.TypeProcessors;

namespace Test.Protocols.Scenario1.Extends
{
    public class CustomerStringIntellectTypeProcessor : IntellectTypeProcessor
    {

        public CustomerStringIntellectTypeProcessor()
        {
            _supportedType = typeof (string);
        }

        #region Overrides of IntellectTypeProcessor

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="value">�������ͻ�����</param>
        /// <returns>����ת�����Ԫ����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            string content = (string) value;
            byte[] data = Encoding.Default.GetBytes(content);
            byte[] body = new byte[data.Length + 1];
            body[0] = (byte) data.Length;
            Array.ConstrainedCopy(data, 0, body, 1, data.Length);
            return body;
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            return Encoding.Default.GetString(ByteArrayHelper.GetNextData(data, 1, data.Length - 1));
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ�������ڵ�ƫ����</param>
        /// <param name="length">Ԫ���ݳ���</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}