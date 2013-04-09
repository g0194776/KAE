using System;
using System.Collections.Generic;
using KJFramework.Data.ObjectDB.Exceptions;
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
        /// <param name="obj">要保存的对象</param>
        /// <param name="tokenId">类型编号</param>
        /// <param name="position">存储的位置信息</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="HookProcessException">数据钩子处理失败</exception>
        void Store(object obj, ulong tokenId, StorePosition position);
        /// <summary>
        ///     获取保存在数据库中所有指定类型的对象集合
        /// </summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="tokenId">类型令牌编号</param>
        /// <returns>返回相关对象集合, 如果无法查找到有效对象则返回null.</returns>
        IList<T> Get<T>(ulong tokenId) where T : new();
    }
}