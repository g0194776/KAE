using System;
using System.Collections.Generic;

namespace KJFramework.ApplicationEngine.Loggers
{
    /// <summary>
    ///     KAE����״̬��¼���ӿ�
    /// </summary>
    internal interface IKAEStateLogger
    {
        #region Members.

        /// <summary>
        ///     ��ȡ�������ڲ����ܹ���������־�������ֵ�����������Ŀ����ʷ���ݽ����Զ���ʧ
        /// </summary>
        int MaximumLogCount { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///     ��¼һ��״̬��Ϣ
        /// </summary>
        /// <param name="content">��Ҫ����¼��״̬��Ϣ����</param>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
        void Log(string content);
        /// <summary>
        ///    �����ڲ�������������״̬��Ϣ
        /// </summary>
        /// <returns>���ذ���������</returns>
        List<string> GetAllLogs();

        #endregion
    }
}