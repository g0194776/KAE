using System;
using System.Threading;
using KJFramework.Cache;
using KJFramework.Net.Channels.Identities;
using KJFramework.Timer;

namespace KJFramework.ServiceModel.Core.Objects
{
    /// <summary>
    ///     �������ĵȴ�����
    /// </summary>
    public class RequestCenterWaitObject : IClearable
    {
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

        #region ��Ա

        /// <summary>
        ///     ��ȡ�����ó�ʱ��
        /// </summary>
        public LightTimer Timer { get; set; }
        /// <summary>
        ///     ��ȡ������ͬ����
        /// </summary>
        public AutoResetEvent ResetEvent { get; set; }
        /// <summary>
        ///     ��ȡ������Ψһ��ֵ
        /// </summary>
        public TransactionIdentity Key { get; set; }

        public DateTime Time { get; set; }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// �����������
        /// </summary>
        public void Clear()
        {
            Key = null;
            ResetEvent = null;
            Timer = null;
        }

        #endregion
    }
}