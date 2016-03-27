using KJFramework.Net.Channels;

namespace KJFramework.ServiceModel.Elements
{
    public class PipeBindingElement : BindingElement<IServiceChannel>
    {
        #region Overrides of CommunicationObject

        /// <summary>
        ///     停止
        /// </summary>
        public override void Abort()
        {
        }

        /// <summary>
        ///     打开
        /// </summary>
        public override void Open()
        {
        }

        /// <summary>
        ///     关闭
        /// </summary>
        public override void Close()
        {
        }

        #endregion

        #region Overrides of BindingElement<IServiceChannel>

        /// <summary>
        ///     初始化
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        ///     创建通道
        /// </summary>
        /// <returns>返回创建后的通道</returns>
        public override IServiceChannel CreateChannel()
        {
            return null;
        }

        #endregion
    }
}