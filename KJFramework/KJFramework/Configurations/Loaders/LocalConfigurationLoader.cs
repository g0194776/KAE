using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Xml;
using KJFramework.Attribute;
using KJFramework.Basic.Enum;
using KJFramework.Configurations.Objects;
using KJFramework.Configurations.Statistics;
using KJFramework.Helpers;
using KJFramework.Statistics;

namespace KJFramework.Configurations.Loaders
{
    /// <summary>
    ///     ���������ļ����������ṩ����صĻ�������
    /// </summary>
    public class LocalConfigurationLoader : IConfigurationLoader
    {
        #region ���캯��

        /// <summary>
        ///     ���������ļ����������ṩ����صĻ�������
        /// </summary>
        public LocalConfigurationLoader()
        {
            LocalConfigurationLoaderStatistics statistic = new LocalConfigurationLoaderStatistics();
            statistic.Initialize(this);
            _statistics.Add(StatisticTypes.Other, statistic);
        }

        #endregion

        #region ��������

        ~LocalConfigurationLoader()
        {
            DisposeStatistics();
            Dispose();
        }

        #endregion

        #region IConfigurationLoader ��Ա

        /// <summary>
        ///     ��ȡ���ü���������
        /// </summary>
        public ConfigurationLoaderTypes ConfigurationLoaderType
        {
            get { return ConfigurationLoaderTypes.Local; }
        }

        public void Load<T>(Action<T> action)
            where T : class, new()
        {
            //��ȡ�ڲ��Զ�������
            CustomerSectionAttribute sectionAttribute = AttributeHelper.GetCustomerAttribute<CustomerSectionAttribute>(typeof(T));
            if (sectionAttribute == null)
            {
                LoadFailedHandler(null);
                throw new System.Exception("�޷���ȡ�������Ϊ�����ýڵ�û�б��CustomerSectionAttribute���ԡ�");
            }
            Configuration configuration = ConfigurationHelper.GetApplicationConfiguration();
            if (configuration == null)
            {
                LoadFailedHandler(null);
                throw new System.Exception("�޷���ȡӦ�ó��������");
            }
            T section = new T();
            List<FieldWithAttribute<CustomerFieldAttribute>> fields = TypeHelper.GetFields<CustomerFieldAttribute>(section.GetType());
            if (fields == null || fields.Count == 0)
            {
                return;
            }
            var checkResult = fields.Where(attribute => attribute.Attribute.IsList && (String.IsNullOrEmpty(attribute.Attribute.ElementName) || attribute.Attribute.ElementType == null));
            if (checkResult.Count() > 0)
            {
                LoadFailedHandler(null);
                throw new System.Exception("�޷����ر��������ļ�����Ϊ�����һ��CustomerFieldAttribute��IsList = true, ������Ǹ����Ե�ElementName�Լ�ElementType�ֶ�ֵ��");
            }
            XmlDocument document = new XmlDocument();
#if !MONO
            document.LoadXml(configuration.GetSection("CustomerConfig").SectionInformation.GetRawXml());
#else
            XmlDocument tempDocument = new XmlDocument();
            tempDocument.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            document.LoadXml(string.Format("<CustomerConfig>{0}</CustomerConfig>", tempDocument.SelectSingleNode("configuration/CustomerConfig").InnerXml));

#endif
            //��ȡָ�����ý�
            XmlNode spNode = document.ChildNodes[0].ChildNodes.Cast<XmlNode>().Where(child => child.Name == sectionAttribute.Name).First();
            if (spNode == null)
            {
                return;
            }
            List<InnerXmlNodeInfomation> nodes = XmlHelper.GetNodes(spNode.ChildNodes);
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }
            var items = nodes.Where(node => node.Name != "#comment").GroupBy(node => node.Name);
            var result = items.Select(
                node => new
                {
                    GroupInfomation = node,
                    FieldAttribute = fields.Where(field => node.Key == (field.Attribute.IsList ? field.Attribute.ElementName : field.Attribute.Name)).FirstOrDefault()
                });
            var last = result.Where(res => res.FieldAttribute != null);
            if (last.Count() > 0)
            {
                foreach (var item in last)
                {
                    //�����б���ʽ��
                    if (!item.FieldAttribute.Attribute.IsList)
                    {
                        InnerGetSingelConfiguration(section, item.FieldAttribute, item.GroupInfomation.First());
                    }
                    else if (item.FieldAttribute.Attribute.IsList)
                    {
                        InnerGetListConfiguration(section, item.FieldAttribute, item.GroupInfomation);
                    }
                }
            }
            action(section);
            LoadSuccessfullyHandler(null);
        }

        #endregion

        #region IDisposable ��Ա

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ����

        /// <summary>
        ///     ע��ͳ����
        /// </summary>
        private void DisposeStatistics()
        {
            if (_statistics != null)
            {
                foreach (Statistic statistic in _statistics.Values)
                {
                    statistic.Close();
                }
                _statistics.Clear();
                _statistics = null;
            }
        }
        
        private void InnerGetSingelConfiguration(Object source, FieldWithAttribute<CustomerFieldAttribute> fieldWithAttribute, InnerXmlNodeInfomation grouping)
        {
            try
            {
                Dictionary<String, String> sectionValues = XmlHelper.GetNodeAttributes(grouping.OutputXml);
                if (sectionValues != null)
                {
                    Object obj = fieldWithAttribute.FieldInfo.FieldType.Assembly.CreateInstance(fieldWithAttribute.FieldInfo.FieldType.FullName);
                    List<FieldWithName> names = TypeHelper.GetFields(obj.GetType());
                    var result = names.Select(name => TypeHelper.SetValue(obj, name.FieldInfo, sectionValues.Where(value => name.Name == value.Key).First().Value));
                    //��ʼ��ֵ
                    foreach (FieldInfo info in result)
                    {}
                    fieldWithAttribute.FieldInfo.SetValue(source, obj);
                }
            }
            catch (System.Exception)
            {
                LoadFailedHandler(null);
            }
        }

        private void InnerGetListConfiguration(Object source, FieldWithAttribute<CustomerFieldAttribute> fieldWithAttribute, IGrouping<String, InnerXmlNodeInfomation> grouping)
        {
            try
            {
                Object list = fieldWithAttribute.FieldInfo.GetType().Assembly.CreateInstance(fieldWithAttribute.FieldInfo.FieldType.FullName);
                foreach (var infomation in grouping)
                {
                    Object fieldInstance = fieldWithAttribute.Attribute.ElementType.Assembly.CreateInstance(fieldWithAttribute.Attribute.ElementType.FullName);
                    List<FieldWithName> names = TypeHelper.GetFields(fieldInstance.GetType());
                    Dictionary<String, String> sectionValues = XmlHelper.GetNodeAttributes(infomation.OutputXml);
                    var result = names.Select(name => TypeHelper.SetValue(fieldInstance, name.FieldInfo, sectionValues.Where(value => name.Name == value.Key).First().Value));
                    //��ʼ��ֵ
                    foreach (FieldInfo info in result)
                    { }
                    //�ҵ�Add����
                    MethodInfo add = list.GetType().GetMethod("Add", new[] { fieldWithAttribute.Attribute.ElementType });
                    add.Invoke(list, new[] { fieldInstance });
                }
                fieldWithAttribute.FieldInfo.SetValue(source, list);
            }
            catch (System.Exception)
            {
                LoadFailedHandler(null);
            }
        }

        #endregion

        #region �¼�

        /// <summary>
        ///     ���سɹ�
        /// </summary>
        internal event EventHandler LoadSuccessfully;
        private void LoadSuccessfullyHandler(System.EventArgs e)
        {
            EventHandler successfully = LoadSuccessfully;
            if (successfully != null) successfully(this, e);
        }

        /// <summary>
        ///     ����ʧ��
        /// </summary>
        internal event EventHandler LoadFailed;
        private void LoadFailedHandler(System.EventArgs e)
        {
            EventHandler failed = LoadFailed;
            if (failed != null) failed(this, e);
        }

        #endregion

        #region Implementation of IStatisticable<Statistic>

        private Dictionary<StatisticTypes, Statistic> _statistics = new Dictionary<StatisticTypes, Statistic>();
        /// <summary>
        ///     ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, Statistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion
    }
}