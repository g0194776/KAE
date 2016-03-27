using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.Attribute;
using KJFramework.Configurations.Loaders;
using KJFramework.Configurations.Objects;
using KJFramework.Enums;
using KJFramework.Helpers;
using KJFramework.Statistics;

namespace KJFramework.ApplicationEngine.Configurations.Loaders
{
    /// <summary>
    ///     KJFramework内部用到的段落级别远程配置加载器
    /// </summary>
    public class RemoteConfigurationLoader : IConfigurationLoader
    {
        #region Constructor

        /// <summary>
        ///     KJFramework内部用到的段落级别远程配置加载器
        /// </summary>
        public RemoteConfigurationLoader()
            : this(RemoteConfigurationSetting.Default)
        {

        }

        /// <summary>
        ///     KJFramework内部用到的段落级别远程配置加载器
        /// </summary>
        /// <param name="setting">远程配置设置</param>
        public RemoteConfigurationLoader(RemoteConfigurationSetting setting)
        {
            if (setting == null) throw new ArgumentNullException("setting");
            _setting = setting;
        }

        #endregion

        #region Members

        private RemoteConfigurationSetting _setting;
        private static XmlDocument _xmlDocument;
        private static object _lockObj = new object();

        #endregion

        #region Implementation of IStatisticable<Statistic>

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, Statistic> Statistics { get; set; }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region Implementation of IConfigurationLoader

        /// <summary>
        ///     加载配置
        /// </summary>
        /// <typeparam name="T">自定义配置节类型</typeparam>
        /// <param name="action">赋值自定义配置节的动作</param>
        public void Load<T>(Action<T> action) where T : class, new()
        {
            lock (_lockObj)
            {
                if (_xmlDocument != null)
                {
                    Generate(action);
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<configuration>");
                string sectionsConfig = SystemWorker.ConfigurationProxy.GetPartialConfig("SECTIONS");
                if (string.IsNullOrEmpty(sectionsConfig)) throw new KeyNotFoundException("#Sadly, we cannot found \"SECTION\" keyword in remote configuration node.");
                builder.AppendLine(sectionsConfig);
                string appsettingConfig = SystemWorker.ConfigurationProxy.GetPartialConfig(SystemWorker.Role + ":APP-SETTINGS");
                if (!string.IsNullOrEmpty(appsettingConfig)) builder.AppendLine(appsettingConfig);
                #region Get CustomerConfig Section Configuration.

                builder.AppendLine("<CustomerConfig>");
                string upConfig, upConfigKey;
                #region Get CustomerConfig.System section configuration *REQUIRE*.

                if (!_setting.IsSpecific_System_Element_Config) upConfig = SystemWorker.ConfigurationProxy.GetPartialConfig(upConfigKey = "CustomerConfig.System");
                else upConfig = SystemWorker.ConfigurationProxy.GetPartialConfig(upConfigKey = (SystemWorker.Role + ":CustomerConfig.System"));
                if (string.IsNullOrEmpty(upConfig)) throw new KeyNotFoundException("#Sadly, we cannot found \"" + upConfigKey + "\" keyword in remote configuration node.");
                builder.AppendLine(upConfig);

                if (_setting.IsSpecific_Customer_Profile_Config)
                {
                    upConfig = SystemWorker.ConfigurationProxy.GetPartialConfig(upConfigKey = (SystemWorker.Role + ":CustomerConfig.Profile"));
                    if (string.IsNullOrEmpty(upConfig)) throw new KeyNotFoundException("#Sadly, we cannot found \"" + upConfigKey + "\" keyword in remote configuration node.");
                    builder.AppendLine(upConfig);
                }

                #endregion
                #region KJFramework Configuration.
                if (_setting.IsCustomizeKJFrameworkConfig)
                {
                    //从远程获取每一个配置节
                    string config, configKey;
                    builder.AppendLine("<KJFramework>");
                    #region Get KJFramework.Net section configuration *REQUIRE*.

                    if (!_setting.IsCustomize_KJFramework_Net_Config) config = SystemWorker.ConfigurationProxy.GetPartialConfig(configKey = "KJFramework.Net");
                    else config = SystemWorker.ConfigurationProxy.GetPartialConfig(configKey = (SystemWorker.Role + ":KJFramework.Net"));
                    if (string.IsNullOrEmpty(config)) throw new KeyNotFoundException("#Sadly, we cannot found \"" + configKey + "\" keyword in remote configuration node.");
                    builder.AppendLine(config);

                    #endregion
                    #region Get KJFramework.Net.Channels section configuration *REQUIRE*.

                    if (!_setting.IsCustomize_KJFramework_Net_Channels_Config) config = SystemWorker.ConfigurationProxy.GetPartialConfig(configKey = "KJFramework.Net.Channels");
                    else config = SystemWorker.ConfigurationProxy.GetPartialConfig(configKey = (SystemWorker.Role + ":KJFramework.Net.Channels"));
                    if (string.IsNullOrEmpty(config)) throw new KeyNotFoundException("#Sadly, we cannot found \"" + configKey + "\" keyword in remote configuration node.");
                    builder.AppendLine(config);

                    #endregion
                    #region Get KJFramework.Net.Transaction section configuration *OPTIONAL*.

                    if (!_setting.IsCustomize_KJFramework_Net_Transaction_Config) config = SystemWorker.ConfigurationProxy.GetPartialConfig(configKey = "KJFramework.Net.Transaction");
                    else config = SystemWorker.ConfigurationProxy.GetPartialConfig(configKey = (SystemWorker.Role + ":KJFramework.Net.Transaction"));
                    if (!string.IsNullOrEmpty(config)) builder.AppendLine(config);

                    #endregion
                    builder.AppendLine("</KJFramework>");
                }
                else
                {
                    string kjConfig = SystemWorker.ConfigurationProxy.GetPartialConfig("KJFRAMEWORK-FAMILY");
                    if (string.IsNullOrEmpty(kjConfig)) throw new KeyNotFoundException("#Sadly, we cannot found \"KJFRAMEWORK-FAMILY\" keyword in remote configuration node.");
                    builder.AppendLine(kjConfig);
                }
                #endregion
                builder.AppendLine("</CustomerConfig>");

                #endregion
                #region Get Other .NETFRAMEWORK section configuration *OPTIONAL*.

                if (!_setting.IsCustomize_DotNetFramework_Config) upConfig = SystemWorker.ConfigurationProxy.GetPartialConfig(upConfigKey = "Configuration.DotNetFramework");
                else upConfig = SystemWorker.ConfigurationProxy.GetPartialConfig(upConfigKey = (SystemWorker.Role + ":Configuration.DotNetFramework"));
                if (!string.IsNullOrEmpty(upConfig)) builder.AppendLine(upConfig);

                #endregion
                builder.AppendLine("</configuration>");
                _xmlDocument = new XmlDocument();
                _xmlDocument.LoadXml(builder.ToString());
                Generate(action);
            }
        }

        /// <summary>
        ///     获取配置加载器类型
        /// </summary>
        public ConfigurationLoaderTypes ConfigurationLoaderType
        {
            get
            {
                return ConfigurationLoaderTypes.Remote;
            }
        }

        #endregion

        #region Methods

        private void Generate<T>(Action<T> action)
            where T : class, new()
        {
            //获取内部自定义属性
            CustomerSectionAttribute sectionAttribute = AttributeHelper.GetCustomerAttribute<CustomerSectionAttribute>(typeof(T));
            if (sectionAttribute == null)
            {
                LoadFailedHandler(null);
                throw new Exception("无法获取配置项，因为该配置节点没有标记CustomerSectionAttribute属性。");
            }
            T section = new T();
            List<FieldWithAttribute<CustomerFieldAttribute>> fields = TypeHelper.GetFields<CustomerFieldAttribute>(section.GetType());
            if (fields == null || fields.Count == 0) return;
            var checkResult = fields.Where(attribute => attribute.Attribute.IsList && (String.IsNullOrEmpty(attribute.Attribute.ElementName) || attribute.Attribute.ElementType == null));
            if (checkResult.Count() > 0)
            {
                LoadFailedHandler(null);
                throw new Exception("无法加载本地配置文件，因为如果将一个CustomerFieldAttribute的IsList = true, 则必须标记该属性的ElementName以及ElementType字段值。");
            }
            //获取指定配置节
            XmlNode spNode = _xmlDocument.ChildNodes[0].ChildNodes[1].ChildNodes.Cast<XmlNode>().Where(child => child.Name == sectionAttribute.Name).First();
            if (spNode == null) return;
            List<InnerXmlNodeInfomation> nodes = XmlHelper.GetNodes(spNode.ChildNodes);
            if (nodes == null || nodes.Count == 0) return;
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
                        InnerGetSingelConfiguration(section, item.FieldAttribute, item.GroupInfomation.First());
                    else if (item.FieldAttribute.Attribute.IsList)
                        InnerGetListConfiguration(section, item.FieldAttribute, item.GroupInfomation);
                }
            }
            action(section);
            LoadSuccessfullyHandler(null);
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
                    { }
                    fieldWithAttribute.FieldInfo.SetValue(source, obj);
                }
            }
            catch (Exception)
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
            catch (Exception)
            {
                LoadFailedHandler(null);
            }
        }

        #endregion

        #region Event.

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
    }
}