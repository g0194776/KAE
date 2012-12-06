using System;
using KJFramework.Messages.Helpers;

namespace KJFramework.ServiceModel.Core.Helpers
{
    internal class IntellectObjectHelper
    {
        /// <summary>
        ///     将一个元数据搭上编号
        /// </summary>
        /// <param name="value">元数据</param>
        /// <param name="id">编号</param>
        /// <param name="vt">值类型标示</param>
        /// <returns>返回搭上编号后的元数据</returns>
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
            short arrLength = (short) (value == null ? 0 : value.Length);
            byte[] data = new byte[arrLength + 2];
            //Buffer.BlockCopy(BitConverter.GetBytes(arrLength), 0, data, 0, 2);
            BitConvertHelper.GetBytes(arrLength, data, 0);
            if (value != null) Buffer.BlockCopy(value, 0, data, 2, value.Length);
            return data;
        }
    }
}