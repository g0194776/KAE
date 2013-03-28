namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     文件控制器接口
    /// </summary>
    internal interface IFileController
    {
        /// <summary>
        ///     确保当前
        /// </summary>
        /// <param name="id">类型编号</param>
        /// <param name="size">数据大小</param>
        /// <param name="remaining">出去本次需要计算的数据大小后，文件内部的剩余大小</param>
        /// <returns>如果返回true, 则证明当前文件内部可以包含本次大小的数据</returns>
        bool EnsureSize(ulong id, uint size, out int remaining);
    }
}