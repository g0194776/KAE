using System;

namespace KJFramework.Messages.Extends.Rules
{
    /// <summary>
    ///     �ָ�����ṩ����صĻ������Խṹ��
    /// </summary>
    public class SplitRule : ISplitRule
    {
        #region ��������

        ~SplitRule()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        protected Type _supportType;
        protected int _splitLength;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ISplitRule

        /// <summary>
        ///     ��ȡ������֧�ֵ�����
        /// </summary>
        public Type SupportType
        {
            get { return _supportType; }
            set { _supportType = value; }
        }

        /// <summary>
        ///     ��ȡ�����÷ָ��
        /// </summary>
        public int SplitLength
        {
            get { return _splitLength; }
            set { _splitLength = value; }
        }


        /// <summary>
        ///     ȡֵ���
        ///     <para>* �˷���ͨ�����ڼ���ѡ�ֶ��Ƿ�ӵ��ֵʱ���жϡ�</para>
        /// </summary>
        /// <param name="offset">��ǰԪ���ݵ�ƫ����</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="tagOffset">
        ///     ���صĸ���ƫ����
        ///     <para>* ������صĽ����false�Ļ�������Ҫ��д����ƫ������</para>
        /// </param>
        /// <param name="targetContentLength">��ǰҪ��ȡ�����ݳ���</param>
        /// <returns>�����Ƿ����ȡֵ�Ľ��</returns>
        public virtual bool Check(int offset, byte[] data, ref int tagOffset, ref int targetContentLength)
        {
            tagOffset = 0;
            targetContentLength = SplitRuleManager.GetRule(_supportType).SplitLength;
            return true;
        }

        #endregion
    }
}