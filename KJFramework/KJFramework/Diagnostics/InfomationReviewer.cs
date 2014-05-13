using System;
using System.Collections.Generic;
using KJFramework.Diagnostics.Collectors;
using KJFramework.Enums;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     ��Ϣ��������ṩ����صĻ���������
    /// </summary>
    public class InfomationReviewer : IInfomationReviewer
    {
        #region ���캯��

        private InfomationReviewer()
        {
            
        }

        #endregion

        #region ��Ա

        protected Dictionary<Guid, IInfomationCollector> _collectors = new Dictionary<Guid, IInfomationCollector>();
        public static readonly InfomationReviewer Instance = new InfomationReviewer();

        #endregion

        #region Implementation of IInfomationReviewer

        /// <summary>
        ///     ע����Ϣ�ռ���
        /// </summary>
        /// <param name="collector">��Ϣ�ռ���</param>
        public void Regist(IInfomationCollector collector)
        {
            if (collector != null && !_collectors.ContainsKey(collector.Id))
            {
                collector.NewInfomation += CollectorNewInfomation;
                _collectors.Add(collector.Id, collector);
            }
        }

        /// <summary>
        ///     ע����Ϣ�ռ���
        /// </summary>
        /// <param name="collector">��Ϣ�ռ���</param>
        public void UnRegist(IInfomationCollector collector)
        {
            if (collector != null && _collectors.ContainsKey(collector.Id))
            {
                UnRegist(collector.Id);
            }
        }

        /// <summary>
        ///     ע����Ϣ�ռ���
        /// </summary>
        /// <param name="id">��Ϣ�ռ���Ψһ��ʾ</param>
        public void UnRegist(Guid id)
        {
            if (_collectors.ContainsKey(id))
            {
                IInfomationCollector collector = _collectors[id];
                collector.NewInfomation -= CollectorNewInfomation;
                _collectors.Remove(id);
                collector.Dispose();
            }
        }

        /// <summary>
        ///     ��ȡ����ָ��Ψһ��ʾ����Ϣ�ռ���
        /// </summary>
        /// <param name="id">��Ϣ�ռ���Ψһ��ʾ</param>
        /// <returns>���ض�Ӧ����Ϣ�ռ���</returns>
        public IInfomationCollector GetCollector(Guid id)
        {
            return _collectors.ContainsKey(id) ? _collectors[id] : null;
        }

        #endregion

        #region �¼�

        void CollectorNewInfomation(object sender, NewInfomationEventArgs e)
        {
            //for test
            Console.WriteLine(e.Infomation);
        }

        #endregion

        #region ����

        /// <summary>
        ///     ����һ���µ���Ϣ�ռ���
        /// </summary>
        /// <param name="collectType">�ռ�����</param>
        /// <param name="collectorTypes">�ռ�������</param>
        /// <returns>����һ���µ���Ϣ�ռ���</returns>
        public static IInfomationCollector CreateCollector(Type collectType, InfomationCollectorTypes collectorTypes)
        {
            IInfomationCollector collector = null;
            switch (collectorTypes)
            {
                case InfomationCollectorTypes.Thread:
                    collector = new ThreadInfomationCollector(Instance);
                    break;
                case InfomationCollectorTypes.Process:
                    collector = new ProcessInfomationCollector(Instance);
                    break;
            }
            if (collector != null)
            {
                Instance.Regist(collector);
                return collector;
            }
            return null;
        }

        #endregion
    }
}