using KJFramework.Net.Channels;

namespace KJFramework.ServiceModel.Elements
{
    public class PipeBindingElement : BindingElement<IServiceChannel>
    {
        #region Overrides of CommunicationObject

        /// <summary>
        ///     ֹͣ
        /// </summary>
        public override void Abort()
        {
        }

        /// <summary>
        ///     ��
        /// </summary>
        public override void Open()
        {
        }

        /// <summary>
        ///     �ر�
        /// </summary>
        public override void Close()
        {
        }

        #endregion

        #region Overrides of BindingElement<IServiceChannel>

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        ///     ����ͨ��
        /// </summary>
        /// <returns>���ش������ͨ��</returns>
        public override IServiceChannel CreateChannel()
        {
            return null;
        }

        #endregion
    }
}