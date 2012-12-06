using System.Collections.Generic;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     远程服务代理接口，提供了相关的基本操作
    /// </summary>
    public interface IRemotingService
    {
        /// <summary>
        ///     获取服务相关信息
        /// </summary>
        IServiceInfomation Infomation { get; }
        /// <summary>
        ///     获取服务的描述方法集合
        /// </summary>
        /// <returns>返回描述方法集合</returns>
        IList<IMethod> GetPreviewMethods();
    }
}