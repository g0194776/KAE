using System;
using KJFramework.Net.Channels;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     ��Ԫ��Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    internal interface IBindingElement<TChannel> : ICommunicationObject
        where TChannel : IServiceChannel
    {
        /// <summary>
        ///     ��ȡ��Ԫ������
        /// </summary>
        String Name { get; } 
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ��Ԫ���Ƿ���԰�
        /// </summary>
        bool CanBind { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���������Ƿ��Ѿ���ʼ���ɹ�
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        ///     ��ʼ��
        /// </summary>
        void Initialize();
        /// <summary>
        ///     ����ͨ��
        /// </summary>
        /// <returns>���ش������ͨ��</returns>
        TChannel CreateChannel();
    }
}