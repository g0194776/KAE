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
    ///     服务方法返回值父类
    /// </summary>
    public class ServiceReturnValue : IntellectObject, IServiceReturnValue, IClearable
    {
        #region 成员

        [IntellectProperty(0)]
        public String ExceptionMessage { get; set; }
        [IntellectProperty(1)]
        public String ExceptionType { get; set; }
        [IntellectProperty(2)]
        public BinaryArgContext ReturnValue { get; set; }
        /// <summary>
        ///     获取或设置处理结果
        /// </summary>
        [IntellectProperty(3)]
        public ServiceProcessResult ProcessResult { get; set; }
        /// <summary>
        ///     获取或设置是否具有返回值
        /// </summary>
        [IntellectProperty(4)]
        public bool HasReturnValue { get; set; }

        #endregion

        #region Implementation of IServiceReturnValue

        /// <summary>
        ///     创建一个由服务器返回的异常对象
        /// </summary>
        /// <returns>返回一个异常</returns>
        public System.Exception CreateException()
        {
            //构不成创造异常的条件
            if (String.IsNullOrEmpty(ExceptionMessage) || String.IsNullOrEmpty(ExceptionType))
            {
                throw new System.Exception("[KJFramework.ServiceModel] 非法的异常信息。");
            }
                Type t = Type.GetType(ExceptionType);
                System.Exception exception = (System.Exception)t.Assembly.CreateInstance(t.FullName, true,
                                                                    BindingFlags.Public | BindingFlags.Instance,
                                                                    null, new object[] {ExceptionMessage}, null, null);
            return exception;
        }

        /// <summary>
        ///     设置返回值对象
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">返回值对象</param>
        public void SetReturnValue<T>(T obj)
        {
            ReturnValue = new BinaryArgContext(0, typeof (T), obj);
        }

        /// <summary>
        ///     设置返回值对象
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">返回值对象</param>
        public void SetReturnValue(Type type, object obj)
        {
            ReturnValue = new BinaryArgContext(0, type, obj);
        }

        /// <summary>
        ///     获取返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        public T GetReturnValue<T>()
        {
            return (T)DataHelper.GetObject(typeof(T), ReturnValue.Data);
        }

        /// <summary>
        ///     获取返回值
        /// </summary>
        /// <param name="returnType">返回值类型</param>
        public Object GetReturnValue(Type returnType)
        {
            return DataHelper.GetObject(returnType, ReturnValue.Data);
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// 清除对象自身
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