using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.ServiceModel.Bussiness.Default.Metadata;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Objects
{
    /// <summary>
    ///     ���󷽷������ṩ����صĻ���������
    /// </summary>
    public class RequestMethodObject : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     ���󷽷������ṩ����صĻ���������
        /// </summary>
        public RequestMethodObject()
        {
            
        }

        /// <summary>
        ///     ���󷽷������ṩ����صĻ���������
        /// </summary>
        /// <param name="argsCount">��������</param>
        public RequestMethodObject(int argsCount)
        {
            if (argsCount > 0) Context = new BinaryArgContext[argsCount];
        }

        /// <summary>
        ///     ���󷽷������ṩ����صĻ���������
        /// </summary>
        /// <param name="data">Ԫ����</param>
        public RequestMethodObject(byte[] data)
        {
            _body = data;
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        [IntellectProperty(0)]
        public BinaryArgContext[] Context { get; set; }
        /// <summary>
        ///     ��ȡ�����÷������б��
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public int MethodToken { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     ���һ�������Ʋ��������ĵ�������
        /// </summary>
        /// <param name="context">�����Ʋ���������</param>
        ///<exception cref="System.Exception">���ʧ��</exception>
        public virtual void AddArg(BinaryArgContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            Context[context.Id] = context;
        }

        /// <summary>
        ///     ����ָ����Ż�ȡ�����Ʋ���������
        /// </summary>
        /// <param name="id">���</param>
        /// <returns>���ض����Ʋ���������</returns>
        public virtual IBinaryArgContext GetArg(int id)
        {
            return Context[id];
        }

        /// <summary>
        ///     ��ȡ���в���
        /// </summary>
        /// <returns>���ز�������</returns>
        public virtual IBinaryArgContext[] GetArgs()
        {
            return Context;
        }

        #endregion
    }
}