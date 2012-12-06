using System;
using KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata
{
    /// <summary>
    ///     元数据交换网络节点元接口，提供了相关的基本操作。
    /// </summary>
    public interface IMetadataExchangeNode
    {
        /// <summary>
        ///     注册一个将要开放元数据的服务契约
        /// </summary>
        /// <param name="path">服务路径</param>
        /// <param name="pageAction">元数据页面动作</param>
        void Regist(String path, IHttpMetadataPageAction pageAction);
        /// <summary>
        ///     注册一个参数描述信息
        /// </summary>
        /// <param name="argumentId">参数编号</param>
        /// <param name="argMetadata">参数描述信息</param>
        void Regist(string argumentId, string argMetadata);
        /// <summary>
        ///     根据指定参数编号获取参数描述信息
        /// </summary>
        /// <param name="argumentId">参数编号</param>
        /// <returns>参数描述信息</returns>
        string GetArgumentMetadata(string argumentId);
        /// <summary>
        ///     注销一个开放的服务契约
        /// </summary>
        /// <param name="name">契约名称</param>
        void UnRegist(String name);
        /// <summary>
        ///     开启元数据交换
        /// </summary>
        void Start();
        /// <summary>
        ///     停止元数据交换
        /// </summary>
        void Stop();
    }
}