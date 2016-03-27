using System;
using System.Reflection;
using KJFramework.Helpers;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     ���񷽷����࣬�ṩ����صĻ���������
    /// </summary>
    public class ServiceMethod : IServiceMethod
    {
        #region ���캯��

        /// <summary>
        ///     ���񷽷����࣬�ṩ����صĻ���������
        ///     <para>ʹ��һ��MethodInfo����ʼ����ǰ���񷽷���</para>
        /// </summary>
        /// <param name="method">��������</param>
        public ServiceMethod(MethodInfo method)
        {
            _id = Guid.NewGuid();
            _method = method;
            if (_method != null)
            {
                Initialize();
            }
        }

        #endregion

        #region ��Ա

        protected ParameterInfo[] _parameterInfos;
        protected MethodInfo _method;
        /// <summary>
        ///     ��ȡ�ڲ����ķ�����Ϣ
        /// </summary>
        public MethodInfo CoreMethodInfo
        {
            get { return _method; }
        }

        #endregion

        #region ����

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        protected virtual void Initialize()
        {
            _parameterInfos = _method.GetParameters();
            _argsCount = _parameterInfos == null ? 0 : _parameterInfos.Length;
            _name = _method.DeclaringType.FullName + "." +  _method.Name;
            if (_method.ReturnParameter != null && _method.ReturnType.FullName.ToLower() != "system.void")
            {
                _hasReturnValue = true;
                _returnType = _method.ReturnType;
            }
            _attribute = AttributeHelper.GetCustomerAttribute<OperationAttribute>(_method);
        }

        /// <summary>
        ///     ��ȡ���ķ�������
        /// </summary>
        /// <returns>���غ��ķ�������</returns>
        public MethodInfo GetCoreMethod()
        {
            return _method;
        }

        #endregion

        #region Implementation of IDisposable

        protected Guid _id;
        protected int _argsCount;
        protected string _name;
        protected bool _hasReturnValue;
        protected Type _returnType;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServiceMethod

        protected OperationAttribute _attribute;

        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        public OperationAttribute Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        ///     ��ȡ������Ψһ���
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        public int ArgsCount
        {
            get { return _argsCount; }
            set { _argsCount = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ���з���ֵ
        ///     <para>* void ����ӵ�з���ֵ��</para>
        /// </summary>
        public bool HasReturnValue
        {
            get { return _hasReturnValue; }
            set { _hasReturnValue = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷���ֵ����
        /// </summary>
        public Type ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        /// <param name="paraIndex">��������</param>
        /// <returns>���ز�������</returns>
        public Type GetParameterType(int paraIndex)
        {
            if (_parameterInfos == null || _parameterInfos.Length == 0) return null;
            return _parameterInfos[paraIndex].ParameterType;
        }

        #endregion
    }
}