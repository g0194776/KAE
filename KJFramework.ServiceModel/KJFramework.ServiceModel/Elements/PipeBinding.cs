using KJFramework.Enums;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.ServiceModel.Elements
{
    /// <summary>
    ///     IPC�󶨷�ʽ
    /// </summary>
    public class PipeBinding : Binding
    {
        #region ���캯��

        
        /// <summary>
        ///     ����TCPЭ��İ󶨷�ʽ�� �ṩ����صĻ���������
        /// </summary>
        /// <param name="localAddress">�󶨵�ַ</param>
        public PipeBinding(string localAddress)
            : this(new PipeUri(localAddress))
        {
        }

        /// <summary>
        ///     IPC�󶨷�ʽ
        /// </summary>
        /// <param name="localAddress">Զ�̵�ַ</param>
        public PipeBinding(PipeUri localAddress)
            : base(new PipeBindingElement(), localAddress)
        {
            _bindingType = BindingTypes.Ipc;
        }

        /// <summary>
        ///     IPC�󶨷�ʽ
        /// </summary>
        /// <param name="bingdingElements">��Ԫ��</param>
        /// <param name="localAddress">Զ�̵�ַ</param>
        public PipeBinding(BindingElement<IServiceChannel> bingdingElements, PipeUri localAddress)
            : base(bingdingElements, localAddress)
        {
            _bindingType = BindingTypes.Ipc;
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