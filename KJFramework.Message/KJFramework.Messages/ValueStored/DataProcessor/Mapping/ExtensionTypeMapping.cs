using System;
using System.Collections.Generic;
using KJFramework.Messages.Configuration;
using KJFramework.Tracing;

namespace KJFramework.Messages.ValueStored.DataProcessor.Mapping
{
    /// <summary>
    ///     可扩展类型处理器映射
    /// </summary>
    public static class ExtensionTypeMapping
    {
        #region Members

        private static readonly Dictionary<byte, BaseValueStored> _valueStoreds = new Dictionary<byte, BaseValueStored>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DataProcessorMapping));

        #endregion

        /// <summary>
        ///     注册扩展类型数据valueStored
        /// </summary>
        /// <param name="type">数据处理器类型</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static void Regist(Type type)
        {
            BaseValueStored valueStored = (BaseValueStored)type.Assembly.CreateInstance(type.FullName);
            if (valueStored == null) throw new ArgumentNullException("type");
            if (valueStored.TypeId <= MetadataObjectSetting.TYPE_BOUNDARY) throw new ArgumentException(string.Format("#Extensionable Type id can not be setted in System Type range, boundary type id is {0}", MetadataObjectSetting.TYPE_BOUNDARY));
            if (valueStored.Clone() == null) throw new NullReferenceException("#There is a null reference object instance when called Clone method.");
            if (!(valueStored.Clone().GetType().IsSubclassOf(typeof(BaseValueStored)))) throw new ArgumentException("#Generated instance of Clone method was not the sub class of BaseValueStored");
            _valueStoreds[valueStored.TypeId] = valueStored;
        }   

        /// <summary>
        ///     返回一个指定扩展类型数据valueStored
        /// </summary>
        /// <param name="typeId">数据类型</param>
        /// <returns>返回一个数组处理器</returns>
        public static BaseValueStored GetValueStored(byte typeId)
        {
            BaseValueStored result;
            return _valueStoreds.TryGetValue(typeId, out result) ? result : null;
        }

        /// <summary>
        ///     注销一个指定扩展类型数据valueStored
        /// </summary>
        /// <param name="typeId">数据类型</param>
        /// <returns>返回一个数组处理器</returns>
        public static void RemoveValueStored(byte typeId)
        {
            try { _valueStoreds.Remove(typeId); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }
    }
}
