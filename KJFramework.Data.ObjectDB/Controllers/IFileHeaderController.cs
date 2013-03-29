using System;
using System.Collections.Generic;
using KJFramework.Data.ObjectDB.Structures;

namespace KJFramework.Data.ObjectDB.Controllers
{
    /// <summary>
    ///     文件头控制器接口
    /// </summary>
    internal interface IFileHeaderController : IDisposable
    {
        /// <summary>
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="fullname">要添加的类型全名称</param>
        /// <returns>返回类型令牌</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        TypeToken AddToken(string fullname);
        /// <summary>
        ///     添加一个类型令牌
        /// </summary>
        /// <param name="id">要添加的类型编号</param>
        /// <returns>返回类型令牌</returns>
        TypeToken AddToken(ulong id);
        /// <summary>
        ///     获取具有指定编号的类型令牌信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回类型令牌</returns>
        TypeToken? GetToken(ulong id);
        /// <summary>
        ///     获取或者添加一个类型令牌
        ///     <para>* 如果指定的类型全名未被加入到当前索引表中，则此方法会自动添加这个类型令牌</para>
        /// </summary>
        /// <param name="fullname">要添加的类型全名称</param>
        /// <returns>返回类型令牌</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        TypeToken GetOrAddToken(string fullname);
        /// <summary>
        ///     获取类型令牌信息集合
        /// </summary>
        IEnumerable<TypeToken> GetTokens();
        /// <summary>
        ///     提交变更请求
        /// </summary>
        void Commit();
    }
}