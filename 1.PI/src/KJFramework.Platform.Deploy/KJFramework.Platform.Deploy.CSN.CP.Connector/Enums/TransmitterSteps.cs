namespace KJFramework.Platform.Deploy.CSN.CP.Connector.Enums
{
    /// <summary>
    ///     ���䲽��ö��
    /// </summary>
    public enum TransmitterSteps
    {
        /// <summary>
        ///     ��ʼ������
        /// </summary>
        InitializePolicy,
        /// <summary>
        ///     ����֪ͨ��Ҫ���еĶ������
        /// </summary>
        Notify,
        /// <summary>
        ///     ���俪ʼ���͵ı�ʾ
        /// </summary>
        BeginTransfer,
        /// <summary>
        ///     ����������
        /// </summary>
        TransferData,
        /// <summary>
        ///     ����������͵ı�ʾ
        /// </summary>
        EndTransfer,
        /// <summary>
        ///     ����һ������������÷ְ�����
        /// </summary>
        TransferDataWithoutMultiPackage,
        /// <summary>
        ///     ��ʱ��
        /// </summary>
        Timeout,
        /// <summary>
        ///     �쳣���
        /// </summary>
        Exception,
        /// <summary>
        ///     �������
        /// </summary>
        Finish
    }
}