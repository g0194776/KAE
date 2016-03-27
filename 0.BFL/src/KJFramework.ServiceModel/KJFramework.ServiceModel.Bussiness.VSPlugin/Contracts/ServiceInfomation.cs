namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     ������Ϣ
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
        ///     ��ȡ��������
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        ///     ��ȡ����ȫ����
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            internal set { _fullName = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public string Description
        {
            get { return _description; }
            internal set { _description = value; }
        }

        /// <summary>
        ///     ��ȡ����ǰ�汾��
        /// </summary>
        public string Version
        {
            get { return _version; }
            internal set { _version = value; }
        }

        /// <summary>
        ///     ��ȡԶ�̷����ַ
        /// </summary>
        public string EndPoint
        {
            get { return _endPoint; }
            internal set { _endPoint = value; }
        }

        #endregion
    }
}