using System;
using System.Collections.Generic;
using System.Diagnostics;
using KJFramework.Attribute;
using KJFramework.Helpers;
using KJFramework.Logger;

namespace KJFramework.PerformanceProvider
{
    /// <summary>
    ///     性能计数器初始化操作帮助器
    /// </summary>
    public static class PerformanceCounters
    {
        #region Members

        private static Dictionary<string, string> _counterKey = new Dictionary<string, string>();
        private static Dictionary<string, Dictionary<int, PerformanceCounter>> _counters = new Dictionary<string, Dictionary<int, PerformanceCounter>>();
        private static Dictionary<string, Dictionary<string, PerformanceCounter>> _customerCounters = new Dictionary<string, Dictionary<string, PerformanceCounter>>();

        #endregion

        #region Methods

        /// <summary>
        ///     注册一个性能计数器的拥有者
        /// </summary>
        /// <param name="ownerType">拥有者类型</param>
        public static void Regist(Type ownerType)
        {
            if (ownerType == null)
            {
                throw new ArgumentNullException("ownerType");
            }
            try
            {
                CounterCreationDataCollection creationDataCollection = null;
                Dictionary<string, int> customerCountersTemp = null;
                string complateName = string.Intern(ownerType.Assembly.FullName);
                string assemblyName = complateName;
                PerformanceCounterAttribute[] attrs = AttributeHelper.GetCustomerAttributes<PerformanceCounterAttribute>(ownerType);
                complateName = attrs[0].CategoryName;
                if (!_counterKey.ContainsKey(assemblyName))
                {
                    _counterKey.Add(assemblyName, complateName);
                }
                if (attrs != null)
                {
                    Dictionary<int, PerformanceCounter> entry = null;
                    foreach (PerformanceCounterAttribute attribute in attrs)
                    {
                        if (!_counters.TryGetValue(complateName, out entry))
                        {
                            entry = new Dictionary<int, PerformanceCounter>();
                            _counters.Add(complateName, entry);
                        }
                        if (attribute.IsSystemCounter)
                        {
                            entry.Add(attribute.Id, new PerformanceCounter(attribute.CategoryName, attribute.CounterName, attribute.ReadOnly));
                        }
                        else
                        {
                            if (creationDataCollection == null)
                            {
                                creationDataCollection = new CounterCreationDataCollection();
                                customerCountersTemp = new Dictionary<string, int>();
                            }
                            CounterCreationData counterCreationData = new CounterCreationData(attribute.CounterName, attribute.CounterHelp, attribute.CounterType);
                            creationDataCollection.Add(counterCreationData);
                            customerCountersTemp.Add(attribute.CounterName, attribute.Id);
                        }
                    }
                    if (creationDataCollection != null)
                    {
                        if (PerformanceCounterCategory.Exists(complateName))
                        {
                            PerformanceCounterCategory.Delete(complateName);
                        }
                        PerformanceCounterCategory category = PerformanceCounterCategory.Create(complateName, string.Empty, PerformanceCounterCategoryType.MultiInstance, creationDataCollection);
                        foreach (PerformanceCounter performanceCounter in category.GetCounters())
                        {
                            performanceCounter.ReadOnly = false;
                            performanceCounter.InstanceName = Process.GetCurrentProcess().ProcessName;
                            entry.Add(customerCountersTemp[performanceCounter.CounterName], performanceCounter);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        ///     获取一个指定拥有者的性能计数器
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static PerformanceCounter GetCounter(Type ownerType, int id)
        {
            if (ownerType == null)
            {
                return null;
            }
            try
            {
                string complateName = string.Intern(ownerType.Assembly.FullName);
                string counterCategoryName;
                //Convert name failed.
                if (!_counterKey.TryGetValue(complateName, out counterCategoryName))
                {
                    return null;
                }
                complateName = counterCategoryName;
                Dictionary<int, PerformanceCounter> entry;
                if (!_counters.TryGetValue(complateName, out entry))
                {
                    return null;
                }
                PerformanceCounter counter;
                if (entry.TryGetValue(id, out counter))
                {
                    return counter;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return null;
            }
        }

        #endregion
    }
}