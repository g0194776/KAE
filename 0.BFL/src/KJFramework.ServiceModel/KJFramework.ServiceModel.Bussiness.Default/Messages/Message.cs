using KJFramework.Cache;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
    /// <summary>
    ///     CONNECT.框架基础消息
    ///     <para>* 派生消息, 字段编号从10开始</para>
    /// </summary>
    public abstract class Message : IntellectObject, IClearable
    {
        #region Constructor

        /// <summary>
        ///     CONNECT.框架基础消息
        ///     <para>* 派生消息, 字段编号从10开始</para>
        /// </summary>
        public Message()
        {
            Flags = new BitFlag();
        }

        #endregion

        #region Transfer Members

        /// <summary>
        ///     获取或设置消息唯一标识
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public MessageIdentity MessageIdentity { get; set; }
        /// <summary>
        ///     获取或设置事务唯一标识
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public TransactionIdentity TransactionIdentity { get; set; }
        /// <summary>
        ///     获取或设置消息相关标识
        /// </summary>
        [IntellectProperty(2, IsRequire = true)]
        public BitFlag Flags { get; set; }
        /// <summary>
        ///     获取或设置逻辑请求地址
        /// </summary>
        [IntellectProperty(3)]
        public string LogicalRequestAddress { get; set; }

        #endregion

        #region Local Members

        /// <summary>
        ///     获取或设置一个值，该值标示了当前消息是否需要ACK。
        /// </summary>
        public bool IsOneway
        {
            get { return Flags[0]; }
            set { Flags[0] = value; }
        }
        /// <summary>
        ///     获取或设置一个标示，该标示表示了当前消息是否被压缩
        /// </summary>
        public bool IsZip
        {
            get { return Flags[1]; }
            set { Flags[1] = value; }
        }
        /// <summary>
        ///     获取或设置一个标示，该标示表示了当前消息是否被加密
        /// </summary>
        public bool IsEncrypt
        {
            get { return Flags[2]; }
            set { Flags[2] = value; }
        }
        /// <summary>
        ///     获取或设置一个标示，该标示表示了当前的请求是否是异步的
        /// </summary>
        public bool IsAsync
        {
            get { return Flags[3]; }
            set { Flags[3] = value; }
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// 清除对象自身
        /// </summary>
        public abstract void Clear();

        #endregion
    }
}