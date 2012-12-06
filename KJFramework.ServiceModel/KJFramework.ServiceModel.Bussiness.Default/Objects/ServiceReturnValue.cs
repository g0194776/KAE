using System;
using System.Reflection;
using KJFramework.Cache;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.ServiceModel.Bussiness.Default.Metadata;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Bussiness.Default.Objects
{
    /// <summary>
    ///     ���񷽷�����ֵ����
    /// </summary>
    public class ServiceReturnValue : IntellectObject, IServiceReturnValue, IClearable
    {
        #region ��Ա

        [IntellectProperty(0)]
        public String ExceptionMessage { get; set; }
        [IntellectProperty(1)]
        public String ExceptionType { get; set; }
        [IntellectProperty(2)]
        public BinaryArgContext ReturnValue { get; set; }
        /// <summary>
        ///     ��ȡ�����ô�����
        /// </summary>
        [IntellectProperty(3)]
        public ServiceProcessResult ProcessResult { get; set; }
        /// <summary>
        ///     ��ȡ�������Ƿ���з���ֵ
        /// </summary>
        [IntellectProperty(4)]
        public bool HasReturnValue { get; set; }

        #endregion

        #region Implementation of IServiceReturnValue

        /// <summary>
        ///     ����һ���ɷ��������ص��쳣����
        /// </summary>
        /// <returns>����һ���쳣</returns>
        public System.Exception CreateException()
        {
            //�����ɴ����쳣������
            if (String.IsNullOrEmpty(ExceptionMessage) || String.IsNullOrEmpty(ExceptionType))
            {
                throw new System.Exception("[KJFramework.ServiceModel] �Ƿ����쳣��Ϣ��");
            }
                Type t = Type.GetType(ExceptionType);
                System.Exception exception = (System.Exception)t.Assembly.CreateInstance(t.FullName, true,
                                                                    BindingFlags.Public | BindingFlags.Instance,
                                                                    null, new object[] {ExceptionMessage}, null, null);
            return exception;
        }

        /// <summary>
        ///     ���÷���ֵ����
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <param name="obj">����ֵ����</param>
        public void SetReturnValue<T>(T obj)
        {
            ReturnValue = new BinaryArgContext(0, typeof (T), obj);
        }

        /// <summary>
        ///     ���÷���ֵ����
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <param name="obj">����ֵ����</param>
        public void SetReturnValue(Type type, object obj)
        {
            ReturnValue = new BinaryArgContext(0, type, obj);
        }

        /// <summary>
        ///     ��ȡ����ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        public T GetReturnValue<T>()
        {
            return (T)DataHelper.GetObject(typeof(T), ReturnValue.Data);
        }

        /// <summary>
        ///     ��ȡ����ֵ
        /// </summary>
        /// <param name="returnType">����ֵ����</param>
        public Object GetReturnValue(Type returnType)
        {
            return DataHelper.GetObject(returnType, ReturnValue.Data);
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// �����������
        /// </summary>
        public void Clear()
        {
            ExceptionMessage = null;
            ExceptionType = null;
            HasReturnValue = false;
            ProcessResult = ServiceProcessResult.UnDefinded;
        }

        #endregion
    }
}