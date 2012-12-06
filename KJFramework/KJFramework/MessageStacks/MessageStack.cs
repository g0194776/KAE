using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Plugin;
using KJFramework.Statistics;

namespace KJFramework.MessageStacks
{
    /// <summary>
    ///     ��ϢЭ��ջ�����࣬�ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class MessageStack<TMessage> : IMessageStack<TMessage>
    {
        #region ��������

        ~MessageStack()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        protected Dictionary<StatisticTypes, IStatistic> _statistics;
        protected int _key;
        protected PluginInfomation _pluginInfo;
        protected bool _enable;
        protected PluginTypes _pluginType;
        protected int _count;
        protected string _name;
        protected string _version;

        /// <summary>
        ///     ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IMetadata<int>

        /// <summary>
        /// ��ȡ����������Լ�����ж����Ψһ��ʾ
        /// </summary>
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        ///     ��ȡ�����ò����Ϣ
        /// </summary>
        public PluginInfomation PluginInfo
        {
            get { return _pluginInfo; }
        }

        /// <summary>
        ///      ��ȡ�����ÿ��ñ�ʾ
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        ///     ��ȡ�����ò������
        /// </summary>
        public PluginTypes PluginType
        {
            get { return _pluginType; }
        }

        /// <summary>
        ///     ���غ���Ҫ���Ķ���
        /// </summary>
        public abstract void OnLoading();

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///     ע��
        /// </summary>
        public void Dispose()
        {
           GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IMessageStack<TMessage>

        /// <summary>
        ///     ��ȡһ������ָ��Э���ŵ���Ϣ
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <returns>����ָ����Ϣ</returns>
        public abstract TMessage Pickup(int protocolId);
        /// <summary>
        ///      ��ȡһ������ָ��Э�����Լ������ŵ���Ϣ
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <param name="serviceId">������</param>
        /// <returns>����ָ����Ϣ</returns>
        public abstract TMessage Pickup(int protocolId, int serviceId);
        /// <summary>
        ///      ��ȡһ������ָ��Э���ţ��������Լ���ϸ�����ŵ���Ϣ
        /// </summary>
        /// <param name="protocolId">Э����</param>
        /// <param name="serviceId">������</param>
        /// <param name="detailServiceId">��ϸ������</param>
        /// <returns>����ָ����Ϣ</returns>
        public abstract TMessage Pickup(int protocolId, int serviceId, int detailServiceId);

        /// <summary>
        ///     ��ȡ��ǰЭ��ջ�е���Ϣ����
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        ///     ��ȡЭ��ջ����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     ��ȡЭ��ջ�汾
        /// </summary>
        public string Version
        {
            get { return _version; }
        }

        /// <summary>
        ///     ��ȡ��Ϣ�ɹ��¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<TMessage>> PickupSuccessfully;
        protected void PickupSuccessfullyHandler(LightSingleArgEventArgs<TMessage> e)
        {
            EventHandler<LightSingleArgEventArgs<TMessage>> successfully = PickupSuccessfully;
            if (successfully != null) successfully(this, e);
        }

        /// <summary>
        ///     ��ȡ��Ϣʧ���¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<int>> PickupFailed;
        protected void PickupFailedHandler(LightSingleArgEventArgs<int> e)
        {
            EventHandler<LightSingleArgEventArgs<int>> failed = PickupFailed;
            if (failed != null) failed(this, e);
        }

        #endregion
    }
}