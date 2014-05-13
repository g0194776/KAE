using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     ����TCPЭ��İ󶨷�ʽ�� �ṩ����صĻ���������
    /// </summary>
    public class TcpBinding : Binding
    {
        #region ���캯��

        /// <summary>
        ///     ����TCPЭ��İ󶨷�ʽ�� �ṩ����صĻ���������
        /// </summary>
        /// <param name="localAddress">�󶨵�ַ</param>
        public TcpBinding(string localAddress)
            : this(new TcpUri(localAddress))
        {
        }

        /// <summary>
        ///     ����TCPЭ��İ󶨷�ʽ�� �ṩ����صĻ���������
        /// </summary>
        /// <param name="localAddress">�󶨵�ַ</param>
        public TcpBinding(TcpUri localAddress)
            : base(new TcpBindingElement(), localAddress)
        {
            _bindingType = BindingTypes.Tcp;
        }

        /// <summary>
        ///     ����TCPЭ��İ󶨷�ʽ�� �ṩ����صĻ���������
        /// </summary>
        /// <param name="bingdingElements">��Ԫ��</param>
        /// <param name="localAddress">�󶨵�ַ</param>
        public TcpBinding(BindingElement<IServiceChannel> bingdingElements, TcpUri localAddress)
            : base(bingdingElements, localAddress)
        {
            _bindingType = BindingTypes.Tcp;
        }

        #endregion

        #region Overrides of Binding

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public override void Initialize()
        {
        }

        #endregion
    }
}