using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Bussiness.Default.Objects
{
    /// <summary>
    ///     服务方法返回值元接口，提供了相关的基本属性结构
    /// </summary>
    internal interface IServiceReturnValue
    {
        /// <summary>
        ///     获取或设置处理结果
        /// </summary>
        ServiceProcessResult ProcessResult { get; set; }
        /// <summary>
        ///     获取或设置是否具有返回值
        /// </summary>
        bool HasReturnValue { get; set; }
        /// <summary>
        ///     创建一个由服务器返回的异常对象
        /// </summary>
        /// <returns>返回一个异常</returns>
        System.Exception CreateException();
        /// <summary>
        ///     设置返回值对象
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">返回值对象</param>
        void SetReturnValue<T>(T obj);
        /// <summary>
        ///     获取返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        T GetReturnValue<T>();
    }
}