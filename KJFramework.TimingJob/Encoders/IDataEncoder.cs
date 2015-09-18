namespace KJFramework.TimingJob.Encoders
{
    /// <summary>
    ///    数据编码器接口
    /// </summary>
    public interface IDataEncoder
    {
        #region Methods.

        /// <summary>
        ///    将一段二进制数据进行编码
        /// </summary>
        /// <param name="data">需要编码的二进制数据</param>
        /// <returns>返回编码后的二进制数据</returns>
        byte[] Encode(byte[] data);

        #endregion
    }
}