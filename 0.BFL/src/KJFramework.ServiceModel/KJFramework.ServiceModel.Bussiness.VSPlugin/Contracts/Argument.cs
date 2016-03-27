using System.Diagnostics;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     ������Ϣ
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
        ///     ��ȡ�������
        /// </summary>
        public int Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public string Type
        {
            get { return _type; }
            internal set { _type = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        ///     ��ȡ�������������Ϣ
        /// </summary>
        public string ReferenceId
        {
            get { return _referenceId; }
            internal set { _referenceId = value; }
        }

        #endregion
    }
}