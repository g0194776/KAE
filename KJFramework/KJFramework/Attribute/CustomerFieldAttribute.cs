using System;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     �Զ������ý������ֶα�ǩ����
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CustomerFieldAttribute : System.Attribute
    {
        #region ���캯��

        /// <summary>
        ///     �Զ������ýڱ�ǩ����
        /// </summary>
        /// <param name="name">���ý�����</param>
        public CustomerFieldAttribute(string name)
        {
            _name = name;
        }

        /// <summary>
        ///     �Զ������ýڱ�ǩ����
        /// </summary>
        /// <param name="name">���ý�����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        public CustomerFieldAttribute(string name, object defaultValue)
        {
            _name = name;
            _defaultValue = defaultValue;
        }

        #endregion

        #region ��Ա

        private String _name;

        /// <summary>
        ///     ��ȡ�Զ������ý�����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        private Object _defaultValue;
        /// <summary>
        ///     ��ȡ������Ĭ��ֵ
        /// </summary>
        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        private bool _isList;
        /// <summary>
        ///     ��ȡ�����õ�ǰ�ֶ��Ƿ�Ϊһ�����ϵı�ʾ
        /// </summary>
        public bool IsList
        {
            get { return _isList; }
            set { _isList = value; }
        }

        private Type _elementType;
        /// <summary>
        ///     ��ȡ�����õ�ǰ�ڲ�Ԫ�ص�����
        ///            *������IsList == true��ʱ����Ч��
        /// </summary>
        public Type ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        private String _elementName;
        /// <summary>
        ///     ��ȡ�����õ�ǰ�ڲ�Ԫ�ص�����������
        ///            *������IsList == true��ʱ����Ч��
        /// </summary>
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }



        #endregion
    }
}