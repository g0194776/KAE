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
        ///     ��ȡһ�����͵����з��񷽷�
        /// </summary>
        /// <returns>�������з��񷽷�</returns>
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
                        //����Ϊ�˵��������������Լ����������������кϷ�����ֵ�ġ���������
                        if (temp.Operation.IsOneWay && (temp.Method.ReturnParameter != null && temp.Method.ReturnType.FullName.ToLower() != "system.void"))
                        {
                            throw new System.Exception("��ǰ�ķ�����Լ�в�����һ�����кϷ�����ֵ�Ĳ���, ����IsOneWay=true�ı�� !");
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
        ///   ���ɵ�ǰ���������ΨһKEY��
        /// </summary>
        /// <param name="paramters">��������</param>
        /// <returns>�������ɵ�KEY</returns>
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