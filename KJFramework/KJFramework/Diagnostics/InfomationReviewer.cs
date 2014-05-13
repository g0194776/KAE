using System;
using System.Collections.Generic;
using KJFramework.Diagnostics.Collectors;
using KJFramework.Enums;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     信息审查器，提供了相关的基本操作。
    /// </summary>
    public class InfomationReviewer : IInfomationReviewer
    {
        #region 构造函数

        private InfomationReviewer()
        {
            
        }

        #endregion

        #region 成员

        protected Dictionary<Guid, IInfomationCollector> _collectors = new Dictionary<Guid, IInfomationCollector>();
        public static readonly InfomationReviewer Instance = new InfomationReviewer();

        #endregion

        #region Implementation of IInfomationReviewer

        /// <summary>
        ///     注册信息收集器
        /// </summary>
        /// <param name="collector">信息收集器</param>
        public void Regist(IInfomationCollector collector)
        {
            if (collector != null && !_collectors.ContainsKey(collector.Id))
            {
                collector.NewInfomation += CollectorNewInfomation;
                _collectors.Add(collector.Id, collector);
            }
        }

        /// <summary>
        ///     注销信息收集器
        /// </summary>
        /// <param name="collector">信息收集器</param>
        public void UnRegist(IInfomationCollector collector)
        {
            if (collector != null && _collectors.ContainsKey(collector.Id))
            {
                UnRegist(collector.Id);
            }
        }

        /// <summary>
        ///     注册信息收集器
        /// </summary>
        /// <param name="id">信息收集器唯一标示</param>
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
        ///     获取具有指定唯一标示的信息收集器
        /// </summary>
        /// <param name="id">信息收集器唯一标示</param>
        /// <returns>返回对应的信息收集器</returns>
        public IInfomationCollector GetCollector(Guid id)
        {
            return _collectors.ContainsKey(id) ? _collectors[id] : null;
        }

        #endregion

        #region 事件

        void CollectorNewInfomation(object sender, NewInfomationEventArgs e)
        {
            //for test
            Console.WriteLine(e.Infomation);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     创建一个新的信息收集器
        /// </summary>
        /// <param name="collectType">收集对象</param>
        /// <param name="collectorTypes">收集器类型</param>
        /// <returns>返回一个新的信息收集器</returns>
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