namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     服务信息
    /// </summary>
    public class ServiceInfomation : IServiceInfomation
    {
        #region Implementation of IServiceInfomation

        private string _name;
        private string _fullName;
        private string _description;
        private string _version;
        private string _endPoint;

        /// <summary>
        ///     获取服务名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        ///     获取服务全名称
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            internal set { _fullName = value; }
        }

        /// <summary>
        ///     获取服务描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            internal set { _description = value; }
        }

        /// <summary>
        ///     获取服务当前版本号
        /// </summary>
        public string Version
        {
            get { return _version; }
            internal set { _version = value; }
        }

        /// <summary>
        ///     获取远程服务地址
        /// </summary>
        public string EndPoint
        {
            get { return _endPoint; }
            internal set { _endPoint = value; }
        }

        #endregion
    }
}