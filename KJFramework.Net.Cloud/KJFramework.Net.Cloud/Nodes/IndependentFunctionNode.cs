using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Net.Exception;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     �����Ĺ��ܽڵ㣬��һ�ֶ������еĹ���ģ��
    ///     <para>* �����Ҫִ�л��������Ĺ��ܣ�����ѡ��ʹ�øù���ģ�顣</para>
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public abstract class IndependentFunctionNode<T> : DynamicDomainComponent, IIndependentFunctionNode<T>
    {
        #region Overrides of DynamicDomainComponent

        private object _tag;

        /// <summary>
        ///     ��ʼִ��ʱ������
        /// </summary>
        protected abstract override void InnerStart();
        /// <summary>
        ///     ִֹͣ��ʱ������
        /// </summary>
        protected abstract override void InnerStop();
        /// <summary>
        ///     ����ʱ������
        /// </summary>
        protected abstract override void InnerOnLoading();
        /// <summary>
        ///     �ڲ��������ʱ������
        /// </summary>
        /// <returns></returns>
        protected abstract override HealthStatus InnerCheckHealth();

        #endregion

        #region Implementation of IFunctionNode<T>

        /// <summary>
        ///    ��ȡ�����ø�������
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <returns>���س�ʼ�����״</returns>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        public abstract bool Initialize();

        #endregion
    }
}