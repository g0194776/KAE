using System;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     �Զ������ýڱ�ǩ����
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CustomerSectionAttribute : System.Attribute
    {
        #region ���캯��

        /// <summary>
        ///     �Զ������ýڱ�ǩ����
        /// </summary>
        /// <param name="name">���ý�����</param>
        public CustomerSectionAttribute(string name)
        {
            _name = name;
        }

        #endregion

        #region ��Ա

        private String _name;
        private bool _remoteConfig;

        /// <summary>
        ///     ��ȡ�Զ������ý�����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ�����ý��Ƿ��Զ�̻�ȡ
        /// </summary>
        public bool RemoteConfig
        {
            get { return _remoteConfig; }
            set { _remoteConfig = value; }
        }

        #endregion
    }
}