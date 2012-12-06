using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Components;
using KJFramework.Logger;
using KJFramework.Statistics;

namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     ��̬�����������������ṩ����صĻ���������
    /// </summary>
    public class DynamicObjectVisitor : IDynamicObjectVisitor
    {
        #region ���캯��

        /// <summary>
        ///     ��̬�����������������ṩ����صĻ���������
        /// </summary>
        /// <param name="component">���������</param>
        internal DynamicObjectVisitor(DynamicDomainComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }
            _component = component;
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();
        protected DynamicDomainComponent _component;

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IDynamicObjectVisitor

        /// <summary>
        ///     ��ȡһ������ָ�����ƵĶ�̬���������
        ///     <para>* �����ǰҪ��ȡ������������������ȡ�������Ϊ����ǰ�汾��</para>
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="flag">�������</param>
        /// <param name="args">��ѡ����</param>
        /// <returns>����ָ������</returns>
        public T GetObject<T>(String flag, params Object[] args)
        {
            //��ֵ���
            if (String.IsNullOrEmpty(flag))
            {
                return default(T);
            }
            try
            {
                if (_component == null || !_component.RuleTable.Exists(flag))
                {
                    return default(T);
                }
                return _component.RuleTable.Get<T>(flag, args);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        #endregion
    }
}