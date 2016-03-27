using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Core.Objects
{
    /// <summary>
    ///     服务方法提取对象，提供各类相关的基本属性结构
    /// </summary>
    public class ServiceMethodPickupObject 
    {

        #region 方法

        private ExecutableServiceMethod _method;
        /// <summary>
        ///     获取或设置服务方法
        /// </summary>
        public ExecutableServiceMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        private OperationAttribute _operation;
        /// <summary>
        ///     获取或设置开放操作属性
        /// </summary>
        public OperationAttribute Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }


        #endregion

        #region Indexer

        /// <summary>
        ///     获取方法的指定参数
        /// </summary>
        /// <param name="i">参数索引</param>
        /// <returns>返回参数类型</returns>
        public Type this[int i]
        {
            get { return _method.GetParameterType(i); }
        }

        #endregion
    }
}