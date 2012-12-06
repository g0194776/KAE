using System;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Core.Attributes
{
    /// <summary>
    ///     ָ��ָ������Ϊ���ŷ������Լ
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceContractAttribute : System.Attribute
    {
        #region ��Ա

        private ServiceConcurrentTypes _serviceConcurrentType = ServiceConcurrentTypes.Multi;
        private String _description;
        private String _version;
        private String _name;

        /// <summary>
        ///     ��ȡ�����÷���ʵ������ö��
        ///     <para>* �����õ�Ĭ��ֵΪ��Multi��</para>
        /// </summary>
        public ServiceConcurrentTypes ServiceConcurrentType
        {
            get { return _serviceConcurrentType; }
            set { _serviceConcurrentType = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷�����Լ��������Ϣ
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷�����Լ�İ汾
        /// </summary>
        public String Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷�����Լ�ı���
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}