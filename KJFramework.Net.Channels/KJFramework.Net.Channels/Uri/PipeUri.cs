using System;
using KJFramework.Basic.Enum;
using KJFramework.Logger;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     Pipe��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
    /// </summary>
    public class PipeUri : Uri
    {
        #region ��Ա

        protected String _machineName;
        protected String _pipeName;

        /// <summary>
        ///     ��ȡ������IPC����
        /// </summary>
        public String PipeName
        {
            get { return _pipeName; }
            set { _pipeName = value; }
        }

        /// <summary>
        ///     ��ȡ�����û�����
        /// </summary>
        public String MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        #endregion

        #region ���캯��

        /// <summary>
        ///     ��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
        /// </summary>
        public PipeUri() : base("")
        { }

        /// <summary>
        ///     ��Դ��ַ��ʾ�࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         ������URL��ַ
        ///     </para>
        /// </param>
        public PipeUri(String url) : base(url)
        {
            int offset = url.LastIndexOf('/');
            _serverUri = url.Substring(offset, url.Length - offset);
        }

        #endregion

        #region ���෽��

        /// <summary>
        ///     ��ȡ�������ڲ�ʹ�õ�Uri��̬
        /// </summary>
        /// <returns>����Uri</returns>
        public override string GetServiceUri()
        {
            return _serverUri;
        }

        protected override void Split()
        {
            base.Split();
            try
            {
                if (_prefix.ToLower() != "pipe")
                {
                    throw new System.Exception("�Ƿ���Pipe��Դ��ַ��ʾ��");
                }
                String[] inners = Address.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (inners.Length <= 1 || inners.Length > 2)
                {
                    throw new System.Exception("�Ƿ���Pipe��Դ��ַ��ʾ��");
                }
                _machineName = inners[0];
                _pipeName = inners[1];
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex, DebugGrade.High, Logs.Name);
                throw new System.Exception("�Ƿ���Pipe��Դ��ַ��ʾ��");
            }
        }

        #endregion
    }
}