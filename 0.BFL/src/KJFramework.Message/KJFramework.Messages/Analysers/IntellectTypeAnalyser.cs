﻿using System;
using System.Collections.Generic;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Objects;

namespace KJFramework.Messages.Analysers
{
    /// <summary>
    ///     类型分析器，提供了相关的基本操作。
    /// </summary>
    internal abstract class IntellectTypeAnalyser<T, K> : IIntellectTypeAnalyser<T, K>
    {
        #region 成员

        protected readonly object _lockObj = new object();
        protected Dictionary<string, T> _result = new Dictionary<string, T>();

        #endregion

        #region 方法

        /// <summary>
        ///     获取指定对象
        /// </summary>
        /// <param name="token">类型编号</param>
        /// <returns>返回分析结果</returns>
        protected T GetObject(string token)
        {
            lock (_lockObj)
            {
                T result;
                return (_result.TryGetValue(token, out result) ? result : default(T));
            }
        }

        /// <summary>
        ///     注册一个分析结果
        /// </summary>
        /// <param name="token">类型编号</param>
        /// <param name="result">分析结果</param>
        protected void RegistAnalyseResult(string token, T result)
        {
            lock (_lockObj)
            {
                if (_result.ContainsKey(token)) return;
                _result.Add(token, result);
            }
        }

        protected VT GetVT(Type type)
        {
            Type innerType;
            if (type.IsEnum) return FixedTypeManager.IsVT(type.GetEnumUnderlyingType());
            if ((innerType = Nullable.GetUnderlyingType(type)) != null) return FixedTypeManager.IsVT(innerType);
            return FixedTypeManager.IsVT(type);
        }

        #endregion

        #region Implementation of IIntellectTypeAnalyser<T>

        /// <summary>
        ///     分析一个类型中的所有智能属性
        /// </summary>
        /// <param name="type">要分析的类型</param>
        /// <returns>返回分析的结果</returns>
        public abstract T Analyse(K type);

        /// <summary>
        ///     清空当前所有的分析结果
        /// </summary>
        public void Clear()
        {
            lock (_lockObj) _result.Clear();
        }

        #endregion
    }
}