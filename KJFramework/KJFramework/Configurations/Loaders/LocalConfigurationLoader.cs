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
    ///     本地配置文件加载器，提供了相关的基本操作
    /// </summary>
    public class LocalConfigurationLoader : IConfigurationLoader
    {
        #region 构造函数

        /// <summary>
        ///     本地配置文件加载器，提供了相关的基本操作
        /// </summary>
        public LocalConfigurationLoader()
        {
            LocalConfigurationLoaderStatistics statistic = new LocalConfigurationLoaderStatistics();
            statistic.Initialize(this);
            _statistics.Add(StatisticTypes.Other, statistic);
        }

        #endregion

        #region 析构函数

        ~LocalConfigurationLoader()
        {
            DisposeStatistics();
            Dispose();
        }

        #endregion

        #region IConfigurationLoader 成员

        /// <summary>
        ///     获取配置加载器类型
        /// </summary>
        public ConfigurationLoaderTypes ConfigurationLoaderType
        {
            get { return ConfigurationLoaderTypes.Local; }
        }

        public void Load<T>(Action<T> action)
            where T : class, new()
        {
            //获取内部自定义属性
            CustomerSectionAttribute sectionAttribute = AttributeHelper.GetCustomerAttribute<CustomerSectionAttribute>(typeof(T));
            if (sectionAttribute == null)
            {
                LoadFailedHandler(null);
                throw new System.Exception("无法获取配置项，因为该配置节点没有标记CustomerSectionAttribute属性。");
            }
            Configuration configuration = ConfigurationHelper.GetApplicationConfiguration();
            if (configuration == null)
            {
                LoadFailedHandler(null);
                throw new System.Exception("无法获取应用程序配置项。");
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
                throw new System.Exception("无法加载本地配置文件，因为如果将一个CustomerFieldAttribute的IsList = true, 则必须标记该属性的ElementName以及ElementType字段值。");
            }
            XmlDocument document = new XmlDocument();
#if !MONO
            document.LoadXml(configuration.GetSection("CustomerConfig").SectionInformation.GetRawXml());
#else
            XmlDocument tempDocument = new XmlDocument();
            tempDocument.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            document.LoadXml(string.Format("<CustomerConfig>{0}</CustomerConfig>", tempDocument.SelectSingleNode("configuration/CustomerConfig").InnerXml));

#endif
            //获取指定配置节
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
                    //不是列表形式的
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

        #region IDisposable 成员

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     注销统计器
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
                    //开始赋值
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
                    //开始赋值
                    foreach (FieldInfo info in result)
                    { }
                    //找到Add方法
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

        #region 事件

        /// <summary>
        ///     加载成功
        /// </summary>
        internal event EventHandler LoadSuccessfully;
        private void LoadSuccessfullyHandler(System.EventArgs e)
        {
            EventHandler successfully = LoadSuccessfully;
            if (successfully != null) successfully(this, e);
        }

        /// <summary>
        ///     加载失败
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
        ///     获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, Statistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion
    }
}