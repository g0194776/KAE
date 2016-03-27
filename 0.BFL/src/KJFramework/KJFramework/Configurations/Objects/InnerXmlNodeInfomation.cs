using System;
namespace KJFramework.Configurations.Objects
{
    /// <summary>
    ///     �ڲ���XML�ڵ���Ϣ��ʱ�ṹ
    /// </summary>
    public class InnerXmlNodeInfomation : IDisposable
    {
        #region ��������

        ~InnerXmlNodeInfomation()
        {
            Dispose();
        }

        #endregion

        #region IDisposable ��Ա

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ��Ա

        /// <summary>
        ///     ��ȡ������XML�ڵ�����
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        ///     ��ȡ�����øýڵ��XMLԪ����
        /// </summary>
        public String OutputXml { get; set; }

        #endregion
    }
}