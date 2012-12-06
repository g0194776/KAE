using System;
using System.Collections.Generic;

namespace KJFramework.Messages.Extends.Rules
{
    /// <summary>
    ///     分割规则管理器，提供了相关的基本操作。
    /// </summary>
    public static class SplitRuleManager
    {
        #region 成员

        private static Dictionary<Type, ISplitRule> _rules = new Dictionary<Type, ISplitRule>();

        #endregion

        #region 构造函数

        /// <summary>
        ///     分割规则管理器，提供了相关的基本操作。
        /// </summary>
        static SplitRuleManager()
        {
            Initialize();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     注册一个分割规则
        ///     <para>* 如果已经拥有指定规则，则进行替换。</para>
        /// </summary>
        /// <param name="supportType">支持类型</param>
        /// <param name="splitLength">分割长度</param>
        public static void Regist(Type supportType, int splitLength)
        {
            Regist(new SplitRule {SplitLength = splitLength, SupportType = supportType});
        }

        /// <summary>
        ///     注册一个分割规则
        ///     <para>* 如果已经拥有指定规则，则进行替换。</para>
        /// </summary>
        /// <param name="rule">分割规则</param>
        public static void Regist(ISplitRule rule)
        {
            if (rule == null || rule.SupportType == null || rule.SplitLength < 0)
            {
                throw new System.Exception("非法的分割规则。");
            }
            lock (typeof(SplitRuleManager))
            {
                if (_rules.ContainsKey(rule.SupportType))
                {
                    _rules[rule.SupportType] = rule;
                    return;
                }
                _rules.Add(rule.SupportType, rule);
            }
        }

        /// <summary>
        ///     注销一个具有指定支持类型的分割规则
        /// </summary>
        /// <param name="supportType">支持类型</param>
        public static void UnRegist(Type supportType)
        {
            if (supportType == null)
            {
                return;
            }
            lock (typeof(SplitRuleManager))
            {
                if (_rules.ContainsKey(supportType))
                {
                    _rules.Remove(supportType);
                }
            }
        }

        /// <summary>
        ///     获取具有支持指定类型的分割规则
        /// </summary>
        /// <param name="supportType">支持类型</param>
        /// <returns>返回分割规则</returns>
        public static ISplitRule GetRule(Type supportType)
        {
            if (supportType == null)
            {
                return null;
            }
            lock (typeof(SplitRuleManager))
            {
                return _rules.ContainsKey(supportType) ? _rules[supportType] : null;
            }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        private static void Initialize()
        {
            Regist(typeof(bool), 1);
            Regist(typeof(char), 1);
            Regist(typeof(sbyte), 1);
            Regist(typeof(byte), 1);
            Regist(typeof(double), 8);
            Regist(typeof(float), 4);
            Regist(typeof(short), 2);
            Regist(typeof(int), 4);
            Regist(typeof(long), 8);
            Regist(typeof(ushort), 2);
            Regist(typeof(uint), 4);
            Regist(typeof(ulong), 8);
        }

        #endregion
    }
}