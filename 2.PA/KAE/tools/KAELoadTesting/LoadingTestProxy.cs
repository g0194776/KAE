using System;
using System.Collections.Generic;
using System.Net;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Net.Transaction.Objects;

namespace KAELoadTesting
{
    public class LoadingTestProxy : IKAEResourceProxy
    {
        private readonly IPEndPoint _iep;

        #region Constructor.

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public LoadingTestProxy(IPEndPoint iep)
        {
            _iep = iep;
        }

        #endregion

        #region Implementation of IKAEResourceProxy

        /// <summary>
        ///     ����һ����ɫ����һ���������KEY��������ȡһ��������Ϣ
        /// </summary>
        /// <param name="role">��ɫ����</param>
        /// <param name="field">������Ϣ��KEY</param>
        /// <returns>������Ӧ��������Ϣ</returns>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        public string GetField(string role, string field)
        {
            return SystemWorker.ConfigurationProxy.GetField(role, field, false);
        }

        /// <summary>
        ///     ����һ�������KAE������ȡԶ��Ŀ���ַ�ļ���
        /// </summary>
        /// <param name="appName">Ŀ��APP������</param>
        /// <param name="version">Ŀ��APP�İ汾</param>
        /// <param name="appUniqueId">��ȡԶ��Ŀ���ַ��ԴKAE APPΨһID</param>
        /// <param name="protocol">ҵ��Э����</param>
        /// <param name="level">KAEӦ�õȼ�</param>
        /// <param name="protocolTypes">ͨ��Э������</param>
        /// <returns>����Զ��Ŀ��ɷ��ʵ�ַ�ļ���, �������null, ��֤��������ָ��������Զ��Ŀ��</returns>
        public IList<string> GetRemoteAddresses(string appName, string version, Guid appUniqueId, Protocols protocol,
            ProtocolTypes protocolTypes, ApplicationLevel level)
        {
            return new List<string> { _iep.ToString() };
        }

        #endregion
    }
}