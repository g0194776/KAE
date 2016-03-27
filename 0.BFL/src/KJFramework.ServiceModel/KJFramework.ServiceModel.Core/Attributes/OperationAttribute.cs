using System;

namespace KJFramework.ServiceModel.Core.Attributes
{
    /// <summary>
    ///     �������ԣ���ʾָ������Ϊ�����еĿ��Ų�����
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OperationAttribute : System.Attribute
    {
        #region ��Ա

        private bool _isOneWay;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ĳ����Ƿ��ǵ���ġ�
        /// </summary>
        public bool IsOneWay
        {
            get { return _isOneWay; }
            set { _isOneWay = value; }
        }

        private int _methodToken;
        /// <summary>
        ///     ��ȡ�����÷������б��
        ///     <para>* ���ͻ��˳��е���ԼΪԶ�̴����ĸ���(�����ó���)ʱ�������ע�����ԡ�</para>
        /// </summary>
        public int MethodToken
        {
            get { return _methodToken; }
            set { _methodToken = value; }
        }

        private bool _isAsync;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ���첽ִ��
        ///     <para>* ���һ��������IsAsync = true, ��ô�������ø÷�����Name���ԣ����Ҷ������ط������벻Ҫ����һ����Name�����������ڻص�����������</para>
        /// </summary>
        public bool IsAsync
        {
            get { return _isAsync; }
            set { _isAsync = value; }
        }

        #endregion
    }
}