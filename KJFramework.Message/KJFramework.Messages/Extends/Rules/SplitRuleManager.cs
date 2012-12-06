using System;
using System.Collections.Generic;

namespace KJFramework.Messages.Extends.Rules
{
    /// <summary>
    ///     �ָ������������ṩ����صĻ���������
    /// </summary>
    public static class SplitRuleManager
    {
        #region ��Ա

        private static Dictionary<Type, ISplitRule> _rules = new Dictionary<Type, ISplitRule>();

        #endregion

        #region ���캯��

        /// <summary>
        ///     �ָ������������ṩ����صĻ���������
        /// </summary>
        static SplitRuleManager()
        {
            Initialize();
        }

        #endregion

        #region ����

        /// <summary>
        ///     ע��һ���ָ����
        ///     <para>* ����Ѿ�ӵ��ָ������������滻��</para>
        /// </summary>
        /// <param name="supportType">֧������</param>
        /// <param name="splitLength">�ָ��</param>
        public static void Regist(Type supportType, int splitLength)
        {
            Regist(new SplitRule {SplitLength = splitLength, SupportType = supportType});
        }

        /// <summary>
        ///     ע��һ���ָ����
        ///     <para>* ����Ѿ�ӵ��ָ������������滻��</para>
        /// </summary>
        /// <param name="rule">�ָ����</param>
        public static void Regist(ISplitRule rule)
        {
            if (rule == null || rule.SupportType == null || rule.SplitLength < 0)
            {
                throw new System.Exception("�Ƿ��ķָ����");
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
        ///     ע��һ������ָ��֧�����͵ķָ����
        /// </summary>
        /// <param name="supportType">֧������</param>
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
        ///     ��ȡ����֧��ָ�����͵ķָ����
        /// </summary>
        /// <param name="supportType">֧������</param>
        /// <returns>���طָ����</returns>
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
        ///     ��ʼ��
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