using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;

namespace KJFramework.PerformanceProvider
{
    /// <summary>
    ///   性能计数器工厂
    /// </summary>
    public static class PerfCounterFactory
    {
        #region Members

        private static object _syncRoot = new object();
        private static List<PerfCounter> _counters = new List<PerfCounter>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(PerfCounterFactory));

        #endregion

        static PerfCounterFactory()
        {
            AppDomain.CurrentDomain.DomainUnload += delegate
                {
                PerfCounter[] counters;
                lock (_syncRoot)
                {
                    counters = new PerfCounter[_counters.Count];
                    _counters.CopyTo(counters);
                    _counters.Clear();
                }
                foreach (PerfCounter counter in counters)
                {
                    counter.RemoveInstance();
                    counter.Close();
                }
                PerformanceCounter.CloseSharedResources();
            };
        }

        public static T GetCounters<T>()
        {
            return GetCounters<T>(string.Empty);
        }

        public static T GetCounters<T>(string instance)
        {
            Type type = typeof(T);

            PerfCategoryAttribute category = GetCategoryAttribute(type);
            switch (category.Type)
            {
                case PerformanceCounterCategoryType.SingleInstance:
                    instance = string.Empty;
                    break;
                case PerformanceCounterCategoryType.MultiInstance:
                    if (string.IsNullOrEmpty(instance))
                    {
                        if (HttpRuntime.AppDomainAppId == null)
                            instance = Process.GetCurrentProcess().ProcessName;
                        else
                            instance = HttpRuntime.AppDomainAppVirtualPath;
                    }
                    break;
                default:
                    break;
            }

            Dictionary<FieldInfo, PerfCounterAttribute> counters = GetCounterAttribute(type, category);
			try 
            {
				EnsureCategoryExist(category, counters.Values);
				return CreateInstance<T>(category, instance, counters);
			}
			catch (System.Exception ex) 
            {
                _tracing.Error(ex, null);
				return CreateEmptyInstance<T>(category, instance, counters);
			}
        }

		private static T CreateEmptyInstance<T>(PerfCategoryAttribute category, string instance, Dictionary<FieldInfo, PerfCounterAttribute> counters) 
        {
			T result = (T)Activator.CreateInstance(typeof(T), true);
			foreach (KeyValuePair<FieldInfo, PerfCounterAttribute> counter in counters) 
            {
				counter.Key.SetValue(result, new PerfCounter(null, null, null));
			}
			return result;
		}

        private static T CreateInstance<T>(PerfCategoryAttribute category, string instance, IDictionary<FieldInfo, PerfCounterAttribute> counters)
        {
            T result = (T)Activator.CreateInstance(typeof(T), true);
            foreach (KeyValuePair<FieldInfo, PerfCounterAttribute> counter in counters)
            {
                PerfCounter pc = new PerfCounter(category.Name, instance, counter.Value);
                lock (_syncRoot) _counters.Add(pc);
                counter.Key.SetValue(result, pc);
                pc.Reset();
            }
            return result;
        }

        private static void EnsureCategoryExist(PerfCategoryAttribute category, ICollection<PerfCounterAttribute> counters)
        {
            try
            {
                if(!PerformanceCounterCategory.Exists(category.Name))
                    DoCreateCategory(category, counters);
            }
            catch { }
            
        }

        private static void DoCreateCategory(PerfCategoryAttribute category, ICollection<PerfCounterAttribute> counters)
        {
            CounterCreationDataCollection collection = new CounterCreationDataCollection();
                
            CounterCreationData data;
            foreach (PerfCounterAttribute counter in counters)
            {
                data = new CounterCreationData();
                data.CounterName = counter.Name;
                data.CounterHelp = counter.Help ?? string.Empty;
                data.CounterType = counter.Type;
                collection.Add(data);

                if (counter.HasBase())
                {
                    data = new CounterCreationData();
                    data.CounterName = counter.BaseName;
                    data.CounterHelp = counter.BaseHelp ?? string.Empty;
                    data.CounterType = counter.BaseType;
                    collection.Add(data);
                }
            }

            PerformanceCounterCategory.Create(category.Name, category.Help, category.Type, collection);
        }

        private static PerfCategoryAttribute GetCategoryAttribute(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(PerfCategoryAttribute), true);
            if (attributes.Length != 1)
                throw new System.Exception("unknown performance counter category type");
            return (PerfCategoryAttribute)attributes[0];
        }

        private static Dictionary<FieldInfo, PerfCounterAttribute> GetCounterAttribute(Type type, PerfCategoryAttribute categoryAttr)
        {
            Dictionary<FieldInfo, PerfCounterAttribute> result = new Dictionary<FieldInfo, PerfCounterAttribute>();

            FieldInfo[] fields = type.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
            );

            foreach (FieldInfo field in fields)
            {
                object[] counterAttrs = field.GetCustomAttributes(typeof(PerfCounterAttribute), true);
                if (counterAttrs.Length != 1)
                    throw new System.Exception("unknown performance counter type");

                result.Add(field, (PerfCounterAttribute)counterAttrs[0]);
            }

            return result;
        }
    }
}