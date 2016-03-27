using System.Diagnostics;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     参数信息
    /// </summary>
    [DebuggerDisplay("Id={Id}, Name={Name}, Type={Type}")]
    public class Argument : IArgument
    {
        #region Implementation of IArgument

        private int _id;
        private string _type;
        private string _name;
        private string _referenceId;

        /// <summary>
        ///     获取参数编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        /// <summary>
        ///     获取参数类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            internal set { _type = value; }
        }

        /// <summary>
        ///     获取参数名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        ///     获取参数相关类型信息
        /// </summary>
        public string ReferenceId
        {
            get { return _referenceId; }
            internal set { _referenceId = value; }
        }

        #endregion
    }
}