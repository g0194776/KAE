using System;
using System.Collections;
using System.Collections.Generic;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Processors;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     ���ܽڵ㣬�ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public abstract class FunctionNode<T> : IFunctionNode<T>
    {
        #region ���캯��

        /// <summary>
        ///     ���ܽڵ㣬�ṩ����صĻ���������
        /// </summary>
        public FunctionNode()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region ��������

        ~FunctionNode()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IFunctionNode<T>

        protected bool _enable;
        protected Guid _id;
        protected Object _tag;

        /// <summary>
        ///     ��ȡ���ñ�ʾ
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
        }

        /// <summary>
        ///     ��ȡΨһ��ֵ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///   ��ȡ�����ø�������
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>���س�ʼ��״̬</returns>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        public abstract bool Initialize();

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}