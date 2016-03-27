using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Core.Objects
{
    /// <summary>
    ///     ���񷽷���ȡ�����ṩ������صĻ������Խṹ
    /// </summary>
    public class ServiceMethodPickupObject 
    {

        #region ����

        private ExecutableServiceMethod _method;
        /// <summary>
        ///     ��ȡ�����÷��񷽷�
        /// </summary>
        public ExecutableServiceMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        private OperationAttribute _operation;
        /// <summary>
        ///     ��ȡ�����ÿ��Ų�������
        /// </summary>
        public OperationAttribute Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }


        #endregion

        #region Indexer

        /// <summary>
        ///     ��ȡ������ָ������
        /// </summary>
        /// <param name="i">��������</param>
        /// <returns>���ز�������</returns>
        public Type this[int i]
        {
            get { return _method.GetParameterType(i); }
        }

        #endregion
    }
}