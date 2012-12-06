using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata
{
    /// <summary>
    ///     二进制参数上下文
    /// </summary>
    public class BinaryArgContext : IntellectObject, IBinaryArgContext
    {
        #region Constructor

        /// <summary>
        ///     二进制参数上下文
        /// </summary>
        public BinaryArgContext()
        {
            
        }

        /// <summary>
        ///     二进制参数上下文
        /// </summary>
        /// <param name="id">参数编号</param>
        /// <param name="type">参数类型 </param>
        /// <param name="value">参数值</param>
        public BinaryArgContext(int id, Type type, object value)
        {
            if (type == null) throw new ArgumentNullException("type");
            Id = (byte) id;
            Data = DataHelper.ToBytes(type, value);
        }

        /// <summary>
        ///     二进制参数上下文
        /// </summary>
        public BinaryArgContext(byte[] data)
        {
            Data = data;
        }

        #endregion

        #region Members

        /// <summary>
        ///     创建一个空的参数上下文
        /// </summary>
        /// <param name="id">参数编号</param>
        /// <returns>返回空的参数上下文</returns>
        public static BinaryArgContext CreateNullContext(int id)
        {
            return new BinaryArgContext {Id = (byte) id};
        }

        #endregion

        #region Implementation of IBinaryArgContext

        /// <summary>
        ///     获取或设置一个值，该值标示了当前的参数是否存在未抛出的异常
        /// </summary>
        public bool HasException { get; set; }

        /// <summary>
        ///     获取或设置异常信息
        /// </summary>
        public System.Exception Exception { get; set; }

        /// <summary>
        ///     获取或设置参数唯一编号
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public byte Id { get; set; }
        /// <summary>
        ///     获取或设置二进制参数上下文元数据
        /// </summary>
        [IntellectProperty(1)]
        public byte[] Data { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     创建一个含有异常的参数上下文
        /// </summary>
        /// <returns></returns>
        public static IBinaryArgContext CreateExceptionContext(System.Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");
            return new BinaryArgContext {Exception = exception, HasException = true};
        }

        
        #endregion
    }
}