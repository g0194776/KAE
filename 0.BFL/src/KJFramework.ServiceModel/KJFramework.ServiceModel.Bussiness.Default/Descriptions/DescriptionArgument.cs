using System;
using System.Reflection;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     �����������࣬�ṩ����صĻ���������
    /// </summary>
    internal class DescriptionArgument : IDescriptionArgument
    {
        #region ���캯��

        /// <summary>
        ///     �����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="id">˳����</param>
        /// <param name="canBeNull">�ɿձ�ʾ</param>
        /// <param name="parameterInfo">������Ϣ</param>
        internal DescriptionArgument(int id, bool canBeNull, ParameterInfo parameterInfo)
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");
            _id = id;
            _canNull = canBeNull;
            _fullName = parameterInfo.ParameterType.Name;
            _name = parameterInfo.Name;
            _parameterType = parameterInfo.ParameterType;
        }

        /// <summary>
        ///     �����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="id">˳����</param>
        /// <param name="canBeNull">�ɿձ�ʾ</param>
        /// <param name="parameterInfo">������Ϣ</param>
        internal DescriptionArgument(int id, bool canBeNull, PropertyInfo parameterInfo)
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");
            _id = id;
            _canNull = canBeNull;
            _fullName = parameterInfo.PropertyType.Name;
            _name = parameterInfo.Name;
            _parameterType = parameterInfo.PropertyType;
        }

        /// <summary>
        ///     �����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="id">˳����</param>
        /// <param name="canBeNull">�ɿձ�ʾ</param>
        /// <param name="parameterInfo">������Ϣ</param>
        internal DescriptionArgument(int id, bool canBeNull, FieldInfo parameterInfo)
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");
            _id = id;
            _canNull = canBeNull;
            _fullName = parameterInfo.FieldType.Name;
            _name = parameterInfo.Name;
            _parameterType = parameterInfo.FieldType;
        }

        #endregion

        #region Implementation of IDisposable

        protected int _id;
        protected string _fullName;
        protected string _name;
        protected bool _canNull;
        protected Type _parameterType;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IDescriptionArgument

        /// <summary>
        ///     ��ȡ�����ò���˳����
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        ///     ��ȡ�����ò���ȫ����
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     ��ȡ������һ����ʾ����ʾ��ǰ�����Ƿ����Ϊ��
        /// </summary>
        public bool CanNull
        {
            get { return _canNull; }
            set { _canNull = value; }
        }

        /// <summary>
        ///     ��ȡ�����ò�������
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        #endregion
    }
}