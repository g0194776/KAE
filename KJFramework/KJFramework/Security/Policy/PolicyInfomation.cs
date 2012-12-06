using System;

namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     策略信息
    /// </summary>
    public class PolicyInfomation : IPolicyInfomation
    {
        #region IPolicyInfomation Members

        private String _name;

        /// <summary>
        ///     获取或设置名称
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
        ///     获取或设略版本
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
        ///     获取或设置描述信息
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