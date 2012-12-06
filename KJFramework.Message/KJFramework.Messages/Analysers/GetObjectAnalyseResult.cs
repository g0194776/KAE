using System;
using System.Reflection;
using System.Text;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Stubs;

namespace KJFramework.Messages.Analysers
{    /// <summary>
    ///     �ַ���ת��Լ��ί��
    /// </summary>
    /// <param name="type">ת��������</param>
    /// <param name="stringBuilder">�ַ���������</param>
    /// <param name="property">�ֶ���Ϣ</param>
    /// <param name="instance">����ʵ��</param>
    /// <param name="space">�����ռ�</param>
    /// <param name="isArrayLoop">�Ƿ���������ѭ���ı�ʾ</param>
    public delegate void ToStringHandler(Type type, StringBuilder stringBuilder, PropertyInfo property, Object instance, string space, bool isArrayLoop);
    /// <summary>
    ///     ��ת��Ϊ����ķ���������ṩ����صĻ���������
    /// </summary>
    public class GetObjectAnalyseResult : AnalyseResult
    {
        #region Members

        private FastInvokeHandler _delegate;
        private FastInvokeHandler _getDelegate;
        private IPropertySetStub _setStub;

        /// <summary>
        ///     �ȴ�����
        /// </summary>
        internal Action<object, GetObjectAnalyseResult, byte[], int, int> CacheProcess;
        /// <summary>
        ///     �ַ���ת����
        /// </summary>
        public ToStringHandler StringConverter;
        /// <summary>
        ///      ��ȡ������Ŀ���������
        /// </summary>
        public Type TargetType;
        #endregion

        #region Methods

        /// <summary>
        ///     ��ȡ��ǰ�ֶ�ֵ
        /// </summary>
        /// <param name="instance">����ʵ��</param>
        /// <returns>ֵ</returns>
        public Object GetValue(Object instance)
        {
            if (_getDelegate == null)
            {
                MethodInfo methodInfo = Property.GetGetMethod();
                _getDelegate = DynamicHelper.GetMethodInvoker(methodInfo);
            }
            return _getDelegate(instance, null);
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public GetObjectAnalyseResult Initialize()
        {
            if (_setStub == null)
            {
                MethodInfo methodInfo = Property.GetSetMethod();
                _setStub = PropertySetStubHelper.Create(TargetType, Property.PropertyType);
                _setStub.Initialize(methodInfo);
            }
            return this;
        }

        /// <summary>
        ///  ���õ�ǰ����ֵ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="instance">Ŀ�����</param>
        /// <param name="value">������ֵ</param>
        public void SetValue<T>(object instance, T value)
        {
            _setStub.Set(instance, value);
        }

        /// <summary>
        ///     Ϊ��ǰ�ֶ�����ֵ
        /// </summary>
        /// <param name="instance">����ʵ��</param>
        /// <param name="args">ֵ</param>
        public void SetValue(object instance, params object[] args)
        {
            if (_delegate == null)
            {
                MethodInfo methodInfo = Property.GetSetMethod();
                _delegate = DynamicHelper.GetMethodInvoker(methodInfo);
            }
            _delegate(instance, args);
        }

        #endregion
    }
}