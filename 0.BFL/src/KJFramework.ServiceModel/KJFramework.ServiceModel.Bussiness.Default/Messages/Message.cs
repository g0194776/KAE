using KJFramework.Cache;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.ServiceModel.Bussiness.Default.Messages
{
    /// <summary>
    ///     CONNECT.��ܻ�����Ϣ
    ///     <para>* ������Ϣ, �ֶα�Ŵ�10��ʼ</para>
    /// </summary>
    public abstract class Message : IntellectObject, IClearable
    {
        #region Constructor

        /// <summary>
        ///     CONNECT.��ܻ�����Ϣ
        ///     <para>* ������Ϣ, �ֶα�Ŵ�10��ʼ</para>
        /// </summary>
        public Message()
        {
            Flags = new BitFlag();
        }

        #endregion

        #region Transfer Members

        /// <summary>
        ///     ��ȡ��������ϢΨһ��ʶ
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public MessageIdentity MessageIdentity { get; set; }
        /// <summary>
        ///     ��ȡ����������Ψһ��ʶ
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public TransactionIdentity TransactionIdentity { get; set; }
        /// <summary>
        ///     ��ȡ��������Ϣ��ر�ʶ
        /// </summary>
        [IntellectProperty(2, IsRequire = true)]
        public BitFlag Flags { get; set; }
        /// <summary>
        ///     ��ȡ�������߼������ַ
        /// </summary>
        [IntellectProperty(3)]
        public string LogicalRequestAddress { get; set; }

        #endregion

        #region Local Members

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��Ϣ�Ƿ���ҪACK��
        /// </summary>
        public bool IsOneway
        {
            get { return Flags[0]; }
            set { Flags[0] = value; }
        }
        /// <summary>
        ///     ��ȡ������һ����ʾ���ñ�ʾ��ʾ�˵�ǰ��Ϣ�Ƿ�ѹ��
        /// </summary>
        public bool IsZip
        {
            get { return Flags[1]; }
            set { Flags[1] = value; }
        }
        /// <summary>
        ///     ��ȡ������һ����ʾ���ñ�ʾ��ʾ�˵�ǰ��Ϣ�Ƿ񱻼���
        /// </summary>
        public bool IsEncrypt
        {
            get { return Flags[2]; }
            set { Flags[2] = value; }
        }
        /// <summary>
        ///     ��ȡ������һ����ʾ���ñ�ʾ��ʾ�˵�ǰ�������Ƿ����첽��
        /// </summary>
        public bool IsAsync
        {
            get { return Flags[3]; }
            set { Flags[3] = value; }
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        /// �����������
        /// </summary>
        public abstract void Clear();

        #endregion
    }
}