namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试信息
    /// </summary>
    public class TestInfo : ITestInfo
    {
        #region Implementation of ITestInfo

        private string _name;
        private string _description;

        /// <summary>
        ///     获取或设置测试事务名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置测试事务描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion
    }
}