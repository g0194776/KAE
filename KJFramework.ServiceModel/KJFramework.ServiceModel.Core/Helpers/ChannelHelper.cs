using System;
using System.Net;
using KJFramework.Basic.Enum;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Uri;
using KJFramework.ServiceModel.Elements;

namespace KJFramework.ServiceModel.Core.Helpers
{
    public class ChannelHelper
    {
        /// <summary>
        ///     �����ƶ������ʹ�������ͨ��
        /// </summary>
        /// <param name="binding">������</param>
        /// <returns>��������ͨ��</returns>
        public static HostTransportChannel Create(Binding binding)
        {
            if (binding == null) throw new ArgumentNullException("binding");
            switch (binding.BindingType)
            {
                //IPC
                case BindingTypes.Ipc:
                    PipeHostTransportChannel hostTransportChannel = new PipeHostTransportChannel(new PipeUri(binding.LogicalAddress.Url), 254);
                    return hostTransportChannel;
                //TCP
                case BindingTypes.Tcp:
                    TcpUri tcpUri = (TcpUri) binding.LogicalAddress;
                    TcpHostTransportChannel tcphostTransportChannel = new TcpHostTransportChannel(tcpUri.Port);
                    return tcphostTransportChannel;
                default:
                    Logs.Logger.Log("un supported binding type : " + binding.BindingType);
                    break;
            }
            return null;
        }

        /// <summary>
        ///     ����һ������ͨ��
        /// </summary>
        /// <param name="uri">Զ���߼���ַ</param>
        /// <param name="binding">�󶨷�ʽ</param>
        /// <returns>���ش���ͨ��</returns>
        public static IReconnectionTransportChannel Create(Net.Channels.Uri.Uri uri, Binding binding)
        {
            if (uri == null || binding == null) throw new System.Exception("�Ƿ��Ĵ�������ͨ��������");
            switch (binding.BindingType)
            {
                //IPC
                case BindingTypes.Ipc:
                    PipeTransportChannel transportChannel = new PipeTransportChannel(uri);
                    return transportChannel;
                //TCP
                case BindingTypes.Tcp:
                    TcpUri tcpUri = (TcpUri) uri;
                    TcpTransportChannel tcpTransportChannel = new TcpTransportChannel(new IPEndPoint(IPAddress.Parse(tcpUri.HostAddress), tcpUri.Port));
                    return tcpTransportChannel;
                default:
                    Logs.Logger.Log("un supported binding type : " + binding.BindingType);
                    break;
            }
            return null;
        }
    }
}