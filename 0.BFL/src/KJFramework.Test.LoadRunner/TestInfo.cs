namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     ������Ϣ
    /// </summary>
    public class TestInfo : ITestInfo
    {
        #region Implementation of ITestInfo

        private string _name;
        private string _description;

        /// <summary>
        ///     ��ȡ�����ò�����������
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     ��ȡ�����ò�����������
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion
    }
}