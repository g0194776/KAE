using System;
using System.Text;
using KJFramework.Attribute;

namespace KJFramework.ServiceModel.Configurations
{
    internal class EncoderItem
    {
        #region 成员

        private Encoding _encoder;

        #endregion

        [CustomerField("Id")]
        public String Id;
        [CustomerField("Num")]
        public int Num;

        /// <summary>
        ///     获取编码器
        /// </summary>
        /// <returns>返回编码器</returns>
        internal Encoding GetEncoder()
        {
            if (_encoder == null)
            {
                if (Num <= 0)
                {
                    throw new System.Exception("Get encoder faild, becuse invalid encoding number.");
                }
                _encoder = Encoding.GetEncoding(Num);
            }
            return _encoder;
        }
    }
}