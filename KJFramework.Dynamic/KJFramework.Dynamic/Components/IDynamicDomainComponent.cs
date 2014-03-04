using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Dynamic.Visitors;

namespace KJFramework.Dynamic.Components
{
    /// <summary>
    ///     ��̬���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDynamicDomainComponent
    {
        /// <summary>
        ///     ��ȡ����
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     ��鵱ǰ����Ľ���״��
        /// </summary>
        /// <returns>���ؽ���״��</returns>
        HealthStatus CheckHealth();
        /// <summary>
        ///     ��ȡΨһ��ʾ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///      ��ȡ�����ÿ��ñ�ʾ
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        ///     ��ȡ�����ò����Ϣ
        /// </summary>
        PluginInfomation PluginInfo { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��������ͨѶ�������
        /// </summary>
        [Obsolete("#We hadn't use this technology, Please DO NOT use this property again!")]
        bool IsUseTunnel { get; }
        /// <summary>
        ///     ��ȡ�����ͨѶ����ĵ�ַ
        ///     <para>* �����������IsUseTunnel = trueʱ��������</para>
        /// </summary>
        /// <exception cref="NotSupportedException">��֧�ָù���</exception>
        /// <returns>���������ַ</returns>
        [Obsolete("#We hadn't use this technology, Please DO NOT use this method again!")]
        string GetTunnelAddress();
        /// <summary>
        ///     ��ȡ���������
        /// </summary>
        [Obsolete("#We hadn't use this technology, Please DO NOT use this object again!")]
        IComponentTunnelVisitor TunnelVisitor { get; }
        /// <summary>
        ///     ��ȡ�����õ�ǰ����������ķ���
        /// </summary>
        IDynamicDomainService OwnService { get; set; }
        /// <summary>
        ///     �������п���ϵ����������ַ
        /// </summary>
        /// <param name="addresses">�����ַ</param>
        [Obsolete("#We hadn't use this technology, Please DO NOT use this method again!")]
        void SetTunnelAddresses(Dictionary<string, string> addresses);
        /// <summary>
        ///     ��ȡָ�������ͨѶ���
        /// </summary>
        /// <param name="componentName">�������</param>
        /// <exception cref="ArgumentNullException">��������</exception>
        /// <exception cref="System.Exception">�޷��ҵ���ǰ�����ͨѶ�����ַ�����ߴ������ʧ��</exception>
        /// <returns>����ָ�������ͨѶ���</returns>
        [Obsolete("#We hadn't use this technology, Please DO NOT use this method again!")]
        T GetTunnel<T>(string componentName) where T : class;
        /// <summary>
        ///     ��ʼִ��
        /// </summary>
        void Start();
        /// <summary>
        ///     ִֹͣ��
        /// </summary>
        void Stop();
        /// <summary>
        ///     ʹ������������
        ///     <para>* ���ô˷����� ���Ὺ���������ͨѶ������ܣ�ʹ�ô�������Ա������������</para>
        /// </summary>
        /// <param name="metadataExchange">
        ///     Ԫ���ݿ��ű�ʾ
        ///     <para>* Ĭ��Ϊ������Ԫ����</para>
        /// </param>
        /// <exception cref="System.Exception">����ʧ��</exception>
        [Obsolete("#We hadn't use this technology, Please DO NOT use this method again!")]
        void UseTunnel<T>(bool metadataExchange = false);
        /// <summary>
        ///     ���غ���Ҫ���Ķ���
        /// </summary>
        void OnLoading();
    }
}