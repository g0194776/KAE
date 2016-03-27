using KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Generators
{
    /// <summary>
    ///     契约生成器元接口，提供了相关的基本操作
    /// </summary>
    public interface IContractGenerator
    {
        /// <summary>
        ///     为指定远程服务生成本地契约文件
        /// </summary>
        /// <param name="service">远程服务</param>
        void Generate(IRemotingService service);
    }
}