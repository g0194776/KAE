using System;
using System.Reflection;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     描述参数父类，提供了相关的基本操作。
    /// </summary>
    internal class DescriptionArgument : IDescriptionArgument
    {
        #region 构造函数

        /// <summary>
        ///     描述参数父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="id">顺序编号</param>
        /// <param name="canBeNull">可空标示</param>
        /// <param name="parameterInfo">参数信息</param>
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
        ///     描述参数父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="id">顺序编号</param>
        /// <param name="canBeNull">可空标示</param>
        /// <param name="parameterInfo">参数信息</param>
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
        ///     描述参数父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="id">顺序编号</param>
        /// <param name="canBeNull">可空标示</param>
        /// <param name="parameterInfo">参数信息</param>
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
        ///     获取或设置参数顺序编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        ///     获取或设置参数全名称
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        /// <summary>
        ///     获取或设置参数名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置一个标示，表示当前参数是否可以为空
        /// </summary>
        public bool CanNull
        {
            get { return _canNull; }
            set { _canNull = value; }
        }

        /// <summary>
        ///     获取或设置参数类型
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        #endregion
    }
}