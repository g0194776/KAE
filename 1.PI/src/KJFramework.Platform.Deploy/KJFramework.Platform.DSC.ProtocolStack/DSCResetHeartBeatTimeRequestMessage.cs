using KJFramework.Messages.Attributes;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     重置心跳时间请求消息
    /// </summary>
    public class DSCResetHeartBeatTimeRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     重置心跳时间请求消息
        /// </summary>
        public DSCResetHeartBeatTimeRequestMessage()
        {
            Header.ProtocolId = 10;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取或设置心跳间隔
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public int Interval { get; set; }
        /// <summary>
        ///     获取或设置重置心跳时间间隔的对象
        ///     <para>* 根据协议规定，如果此值为 *SMC*，则证明是设置SMC的时间间隔</para>
        /// </summary>
        [IntellectProperty(12, IsRequire = true)]
        public string Target { get; set; }
        /// <summary>
        ///     获取或设置机器名
        ///     <para>* 当设置Target = *SMC*时，请设置MachineName</para>
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public string MachineName { get; set; }

        #endregion

    }
}