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
        ///     从第三方客户数据转换为元数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="value">第三方客户数据</param>
        /// <returns>返回转换后的元数据</returns>
        /// <exception cref="Exception">转换失败</exception>
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
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            return Encoding.Default.GetString(ByteArrayHelper.GetNextData(data, 1, data.Length - 1));
        }

        /// <summary>
        ///     从元数据转换为第三方客户数据
        /// </summary>
        /// <param name="attribute">当前字段标注的属性</param>
        /// <param name="data">元数据</param>
        /// <param name="offset">元数据所在的偏移量</param>
        /// <param name="length">元数据长度</param>
        /// <returns>返回转换后的第三方客户数据</returns>
        /// <exception cref="Exception">转换失败</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}