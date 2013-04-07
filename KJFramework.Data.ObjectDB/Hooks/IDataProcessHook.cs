namespace KJFramework.Data.ObjectDB.Hooks
{
    /// <summary>
    ///     数据处理钩子接口
    /// </summary>
    public interface IDataProcessHook
    {
        /// <summary>
        ///     处理二进制数据
        /// </summary>
        /// <param name="data">要被处理的二进制数据</param>
        /// <returns>返回处理后的数据</returns>
        byte[] Process(byte[] data);
    }
}