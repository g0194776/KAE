using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     文件控制器接口
    /// </summary>
    internal interface IFileController
    {
        /// <summary>
        ///     获取一个值，该值标示了当前的文件是否为数据库主文件
        /// </summary>
        bool IsMainFile { get; }
        /// <summary>
        ///     确保当前
        /// </summary>
        /// <param name="id">类型编号</param>
        /// <param name="size">数据大小</param>
        /// <param name="remaining">出去本次需要计算的数据大小后，文件内部的剩余大小</param>
        /// <returns>如果返回true, 则证明当前文件内部可以包含本次大小的数据</returns>
        bool EnsureSize(ulong id, uint size, out StorePosition remaining);
        /// <summary>
        ///     存储一个对象数据
        /// </summary>
        /// <param name="tokenId">类型编号</param>
        /// <param name="position">存储的位置信息</param>
        /// <param name="data">要存储的数据</param>
        void Store(ulong tokenId, StorePosition position, byte[] data);
    }
}