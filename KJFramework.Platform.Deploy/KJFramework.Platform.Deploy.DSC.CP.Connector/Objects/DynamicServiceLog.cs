using System;
using System.Collections.Generic;
using KJFramework.Platform.Deploy.DSC.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.Metadata.Performances;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector.Objects
{
    public class DynamicServiceLog
    {
        #region Constructor

        public DynamicServiceLog()
        {
            Componnets = new Dictionary<string, OwnComponentItem>();
        }

        #endregion

        #region Members

        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���汾��
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        public int ComponentCount { get; set; }
        /// <summary>
        ///     ��ȡ�������������
        /// </summary>
        public Dictionary<string, OwnComponentItem> Componnets { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        ///     ��ȡ�������������ʱ��
        /// </summary>
        public DateTime LastHeartbeatTime { get; set; }
        /// <summary>
        ///     ��ȡ�����ÿ��Ʒ����ַ
        /// </summary>
        public string ControlServiceAddress { get; set; }
        /// <summary>
        ///     ��ȡ��������ǰ汾��
        /// </summary>
        public string ShellVersion { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     ��������Ľ���״̬
        /// </summary>
        /// <param name="responseMessage">��ȡ�������״̬������Ϣ��</param>
        internal void Update(DSCGetComponentHealthResponseMessage responseMessage)
        {
            if (responseMessage.Items != null && Componnets != null)
            {
                foreach (ComponentHealthItem healthItem in responseMessage.Items)
                {
                    OwnComponentItem item;
                    if (Componnets.TryGetValue(healthItem.ComponentName, out item))
                    {
                        item.Status = healthItem.Status;
                    }
                }
            }
        }

        #endregion
    }
}