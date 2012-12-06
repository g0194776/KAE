using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Helpers;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata
{
    /// <summary>
    ///     �����Ʋ���������
    /// </summary>
    public class BinaryArgContext : IntellectObject, IBinaryArgContext
    {
        #region Constructor

        /// <summary>
        ///     �����Ʋ���������
        /// </summary>
        public BinaryArgContext()
        {
            
        }

        /// <summary>
        ///     �����Ʋ���������
        /// </summary>
        /// <param name="id">�������</param>
        /// <param name="type">�������� </param>
        /// <param name="value">����ֵ</param>
        public BinaryArgContext(int id, Type type, object value)
        {
            if (type == null) throw new ArgumentNullException("type");
            Id = (byte) id;
            Data = DataHelper.ToBytes(type, value);
        }

        /// <summary>
        ///     �����Ʋ���������
        /// </summary>
        public BinaryArgContext(byte[] data)
        {
            Data = data;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ����һ���յĲ���������
        /// </summary>
        /// <param name="id">�������</param>
        /// <returns>���ؿյĲ���������</returns>
        public static BinaryArgContext CreateNullContext(int id)
        {
            return new BinaryArgContext {Id = (byte) id};
        }

        #endregion

        #region Implementation of IBinaryArgContext

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�Ĳ����Ƿ����δ�׳����쳣
        /// </summary>
        public bool HasException { get; set; }

        /// <summary>
        ///     ��ȡ�������쳣��Ϣ
        /// </summary>
        public System.Exception Exception { get; set; }

        /// <summary>
        ///     ��ȡ�����ò���Ψһ���
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public byte Id { get; set; }
        /// <summary>
        ///     ��ȡ�����ö����Ʋ���������Ԫ����
        /// </summary>
        [IntellectProperty(1)]
        public byte[] Data { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     ����һ�������쳣�Ĳ���������
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