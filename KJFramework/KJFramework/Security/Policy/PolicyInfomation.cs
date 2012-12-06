using System;

namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     ������Ϣ
    /// </summary>
    public class PolicyInfomation : IPolicyInfomation
    {
        #region IPolicyInfomation Members

        private String _name;

        /// <summary>
        ///     ��ȡ����������
        /// </summary>
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private String _version;

        /// <summary>
        ///     ��ȡ�����԰汾
        /// </summary>
        public String Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        private String _description;

        /// <summary>
        ///     ��ȡ������������Ϣ
        /// </summary>
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        #endregion
    }
}