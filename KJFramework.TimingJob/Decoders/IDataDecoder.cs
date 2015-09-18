namespace KJFramework.TimingJob.Decoders
{
    /// <summary>
    ///    数据解码器接口
    /// </summary>
    public interface IDataDecoder
    {
        #region Methods.
        
        /// <summary>
        ///    将一段二进制数据进行解码
        /// </summary>
        /// <param name="data">需要解码的二进制数据</param>
        /// <returns>返回解码后的二进制数据</returns>
        byte[] Decode(byte[] data);

        #endregion
    }
}