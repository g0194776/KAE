using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     描述方法元接口，提供了相关的基本操作。
    /// </summary>
    public interface IDescriptionMethod : IServiceMethod
    {
        /// <summary>
        ///     获取所有该操作的参数
        /// </summary>
        /// <returns>返回参数集合</returns>
        IDescriptionArgument[] GetArguments();
        /// <summary>
        ///     获取操作的标示
        /// </summary>
        int MethodToken { get; }
    }
}