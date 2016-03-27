using System;

namespace KJFramework.Messages.Attributes
{
    /// <summary>
    ///     �������Ա�ǩ���ṩ���Զ��������ԵĻ�������֧�֡�
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IntellectPropertyAttribute : System.Attribute, IIntellectProperty
    {
        #region ���캯��

        /// <summary>
        ///     �������Ա�ǩ���ṩ���Զ��������ԵĻ�������֧�֡�
        /// </summary>
        /// <param name="id">˳����</param>
        public IntellectPropertyAttribute(int id) : this(id, false)
        {
        }

        /// <summary>
        ///     �������Ա�ǩ���ṩ���Զ��������ԵĻ�������֧�֡�
        /// </summary>
        /// <param name="id">˳����</param>
        /// <param name="isRequire">��ʾ�˵�ǰ�����Ƿ����ӵ��ֵ</param>
        public IntellectPropertyAttribute(int id, bool isRequire)
        {
            _id = id;
            _isRequire = isRequire;
        }

        #endregion

        #region ��Ա

        private int _id;
        /// <summary>
        ///     ��ȡ����������˳����
        ///     <para>* �˱�Ų����ظ���</para>
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private bool _isRequire;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ����ӵ��ֵ��
        /// </summary>
        public bool IsRequire
        {
            get { return _isRequire; }
            set { _isRequire = value; }
        }

        private bool _allowDefaultNull;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�ֶ��Ƿ�֧��"Ĭ��ֵ�����봫��"����
        ///     <para>* �����Խ���ֵ�����ֶβ�������</para>
        ///     <para>* �����������������Ϊtrue, ���ұ�����ֶε�ǰ������ֵ�������ǿ���ڲ������õ�Ĭ��ֵ������ֶν��᲻�������л�����</para>
        /// </summary>
        public bool AllowDefaultNull
        {
            get { return _allowDefaultNull; }
            set { _allowDefaultNull = value; }
        }

        #region Customer Message Region

        private bool _needExtendAction;
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����Ƿ���Ҫ������չ���춯����
        ///     <para>* ����Ӱ�췶Χ����������Ϣ�ṹ��������</para>
        /// </summary>
        public bool NeedExtendAction
        {
            get { return _needExtendAction; }
            set { _needExtendAction = value; }
        }

        private string _tag;
        /// <summary>
        ///     ��ȡ�����ø�������
        ///     <para>* ����Ӱ�췶Χ����������Ϣ�ṹ��������</para>
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #endregion
    }
}