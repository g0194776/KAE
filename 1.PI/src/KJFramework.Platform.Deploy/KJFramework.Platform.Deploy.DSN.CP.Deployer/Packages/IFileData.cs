namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages
{
    /// <summary>
    ///     �ļ�����Ԫ�ӿڣ��ṩ����صĻ������Խṹ��
    /// </summary>
    public interface IFileData
    {
        /// <summary>
        ///     ��ȡ�����õ�ǰ�ļ����ݰ��ı��
        /// </summary>
        int CurrentId { get; set; }
        /// <summary>
        ///     ��ȡ��ǰ���Ķ���������
        /// </summary>
        byte[] Data { get;  }
    }
}