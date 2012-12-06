namespace KJFramework.ServiceModel.Enums
{
    /// <summary>
    ///     ��������ö��
    /// </summary>
    public enum ServiceProcessResult : byte
    {
        /// <summary>
        ///     �ɹ�
        /// </summary>
        Success,
        /// <summary>
        ///     ����
        /// </summary>
        Error,
        /// <summary>
        ///     �쳣
        /// </summary>
        Exception,
        /// <summary>
        ///     ��ʱ
        /// </summary>
        Timeout,
        /// <summary>
        ///     δ�����
        /// </summary>
        UnDefinded
    }
}