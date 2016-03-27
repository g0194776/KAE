using System;
using System.Collections.Generic;
using System.Reflection;
using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     �����������࣬�ṩ����صĻ���������
    /// </summary>
    internal class DescriptionMethod : ServiceMethod, IDescriptionMethod
    {
        #region ���캯��

        /// <summary>
        ///     ���񷽷����࣬�ṩ����صĻ���������
        ///     <para>ʹ��һ��MethodInfo����ʼ����ǰ���񷽷���</para>
        /// </summary>
        /// <param name="method">��������</param>
        internal DescriptionMethod(MethodInfo method)
            : base(method)
        {
            ParameterInfo[] parameters;
            if ((parameters = method.GetParameters()) != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    //����Ĭ�϶��ǿ���Ϊ�յ�
                    _arguments.Add(new DescriptionArgument(i, true, parameters[i]));
                }
            }
            if (method.ReturnType.FullName != "System.Void")
            {
                _returnType = method.ReturnType;
                _hasReturnValue = true;
            }
            else
            {
                _hasReturnValue = false;
            }
        }

        #endregion

        #region Implementation of IDescriptionMethod

        protected String _returnTypeFullName;
        protected List<IDescriptionArgument> _arguments = new List<IDescriptionArgument>();

        /// <summary>
        ///     ��ȡ�����÷�������ȫ����
        /// </summary>
        public string ReturnTypeFullName
        {
            get { return _returnTypeFullName; }
            set { _returnTypeFullName = value; }
        }

        #endregion

        #region Implementation of IDescriptionMethod

        /// <summary>
        ///     ��ȡ���иò����Ĳ���
        /// </summary>
        /// <returns>���ز�������</returns>
        public IDescriptionArgument[] GetArguments()
        {
            return _arguments == null ? null : _arguments.ToArray();
        }

        /// <summary>
        ///     ��ȡ�����ı�ʾ
        /// </summary>
        public int MethodToken
        {
            get
            {
                if (_attribute.MethodToken != 0)
                {
                    return _attribute.MethodToken;
                }
                return _method.MetadataToken;
            }
        }

        #endregion
    }
}