using System;
using System.Text;
using KJFramework.Attribute;

namespace KJFramework.ServiceModel.Configurations
{
    internal class EncoderItem
    {
        #region ��Ա

        private Encoding _encoder;

        #endregion

        [CustomerField("Id")]
        public String Id;
        [CustomerField("Num")]
        public int Num;

        /// <summary>
        ///     ��ȡ������
        /// </summary>
        /// <returns>���ر�����</returns>
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