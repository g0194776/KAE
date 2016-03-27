using System;
using System.Diagnostics;
using System.Reflection;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Stubs;

namespace KJFramework.Messages.Analysers
{
    /// <summary>
    ///     ��ת��ΪԪ���ݵķ���������ṩ����صĻ���������
    /// </summary>
    [DebuggerDisplay("Type: {Property.PropertyType}")]
    public class ToBytesAnalyseResult : AnalyseResult
    {
        #region Members

        private IPropertyStub _stub;
        /// <summary>
        ///     ��ȡ������Ŀ���������
        /// </summary>
        public Type TargetType { get; set; }
   

        /// <summary>
        ///     �ȴ�����
        /// </summary>
        public Action<IMemorySegmentProxy, IntellectPropertyAttribute, ToBytesAnalyseResult, object, bool, bool> CacheProcess;

        #endregion

        #region Methods

        /// <summary>
        ///     ��ȡ��ǰ�ֶ�ֵ
        /// </summary>
        /// <param name="instance">����ʵ��</param>
        /// <returns>ֵ</returns>
        public T GetValue<T>(Object instance)
        {
            return _stub.Get<T>(instance);
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public ToBytesAnalyseResult Initialize()
        {
            if (_stub == null)
            {
                MethodInfo methodInfo = Property.GetGetMethod(true);
                _stub = PropertyStubHelper.Create(TargetType, Property.PropertyType, methodInfo);
                _stub.Initialize(methodInfo);
            }
            return this;
        }

        #endregion
    }
}