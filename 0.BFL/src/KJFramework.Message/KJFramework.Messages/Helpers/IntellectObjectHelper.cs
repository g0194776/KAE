using System;

namespace KJFramework.Messages.Helpers
{
    internal class IntellectObjectHelper
    {
        /// <summary>
        ///     ��һ��Ԫ���ݴ��ϱ��
        /// </summary>
        /// <param name="value">Ԫ����</param>
        /// <param name="id">���</param>
        /// <param name="vt">ֵ���ͱ�ʾ</param>
        /// <returns>���ش��ϱ�ź��Ԫ����</returns>
        public static byte[] SetDataId(byte[] value, int id, bool vt)
        {
            int offset = vt ? 1 : 5;
            int len = value == null ? 0 : value.Length;
            byte[] temp = new byte[(value == null ? 0 : value.Length) + offset];
            temp[0] = (byte)id;
            if (!vt) BitConvertHelper.GetBytes(len, temp, 1);
            //Buffer.BlockCopy(BitConverter.GetBytes(value == null ? 0 : value.Length), 0, temp, 1, 4);
            if (value != null) Buffer.BlockCopy(value, 0, temp, offset, value.Length);
            return temp;
        }

        public static byte[] SetLength(byte[] value)
        {
            ushort arrLength = (ushort)(value == null ? 0 : value.Length);
            byte[] data = new byte[arrLength + 2];
            BitConvertHelper.GetBytes(arrLength, data, 0);
            if (value != null) Buffer.BlockCopy(value, 0, data, 2, value.Length);
            return data;
        }
    }
}