using KJFramework.Helpers;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Methods;
using KJFramework.ServiceModel.Core.Objects;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KJFramework.ServiceModel.Core.Helpers
{
    public class ServiceHelper
    {
        #region Members

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ServiceHelper));

        #endregion

        #region Methods

        /// <summary>
        ///     获取一个类型的所有服务方法
        /// </summary>
        /// <returns>返回所有服务方法</returns>
        public static ServiceMethodPickupObject[] GetServiceMethods(Type instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (AttributeHelper.GetCustomerAttribute<ServiceContractAttribute>(instance) == null)
            {
                throw new System.Exception("invalid service contract !");
            }
            try
            {
                var methodInfos = instance.GetMethods().Where(method => AttributeHelper.GetCustomerAttribute<OperationAttribute>(method) != null).Select(method => new { Method = method, Operation = AttributeHelper.GetCustomerAttribute<OperationAttribute>(method) });
                if (methodInfos.Count() > 0)
                {
                    List<ServiceMethodPickupObject> serviceMethods = new List<ServiceMethodPickupObject>();
                    foreach (var temp in methodInfos)
                    {
                        //声明为了单向操作，但是契约的这个操作方法是有合法返回值的。（不允许）
                        if (temp.Operation.IsOneWay && (temp.Method.ReturnParameter != null && temp.Method.ReturnType.FullName.ToLower() != "system.void"))
                        {
                            throw new System.Exception("当前的服务契约中不允许一个带有合法返回值的操作, 标有IsOneWay=true的标记 !");
                        }
                        serviceMethods.Add(new ServiceMethodPickupObject
                        {
                            Method =
                                new ExecutableServiceMethod(temp.Method) { Handler = DynamicHelper.GetMethodInvoker(temp.Method) },
                            Operation = temp.Operation
                        });
                    }
                    return serviceMethods.ToArray();
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
            return null;
        }

        /// <summary>
        ///   生成当前类型数组的唯一KEY。
        /// </summary>
        /// <param name="paramters">类型数组</param>
        /// <returns>返回生成的KEY</returns>
        internal static String GenParameterKey(Type[] paramters)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Type paramter in paramters)
            {
                builder.Append(String.Format("{0},{1}", paramter.FullName, paramter.Assembly.FullName));
                builder.Append("_");
            }
            return builder.ToString();
        }

        #endregion
    }
}