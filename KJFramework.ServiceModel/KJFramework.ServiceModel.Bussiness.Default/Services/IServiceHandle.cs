using System;
using KJFramework.ServiceModel.Elements;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface IServiceHandle : IMetadataExchange
    {
        /// <summary>
        ///     ��ȡ�󶨶���
        /// </summary>
        Binding[] Bindings { get; }
        /// <summary>
        ///     ��ȡ�����õ�ַURL
        /// </summary>
        Uri Uri { get; set; }
        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        HostService GetService();
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�������Ŀ�����
        /// </summary>
        bool Enable { get; }
        /// <summary>
        ///     ��������
        /// </summary>
        void Open();
        /// <summary>
        ///     �رշ���
        /// </summary>
        void Close();
        /// <summary>
        ///     �رշ��񣬲��رյ�ǰ���������ŵ�����ϵͳ��Դ
        /// </summary>
        void Shutdown();
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        void Initialize();
        /// <summary>
        ///     �Ѿ������¼�
        /// </summary>
        event EventHandler Opened;
        /// <summary>
        ///     �ر��¼�
        /// </summary>
        event EventHandler Closed;
    }
}