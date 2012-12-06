namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages
{
    /// <summary>
    ///     �ļ����ݰ�Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IFilePackage
    {
        /// <summary>
        ///     ��ȡ��ǰ�ļ����ݰ�����������������
        /// </summary>
        string RequestToken { get;  }
        /// <summary>
        ///     ��ȡ�������ܹ��ķ��Ƭ��Ŀ
        /// </summary>
        int TotalPackageCount { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�����
        /// </summary>
        string ServiceName { get; set; }
        /// <summary>
        ///     ��ȡ�����÷������
        /// </summary>
        string Name { get; set; }
        /// <summary>
        ///     ��ȡ�����÷���汾
        /// </summary>
        string Version { get; set; }
        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        string Description { get; set; }
        /// <summary>
        ///     ���һ���ļ����ݷ��Ƭ
        /// </summary>
        /// <param name="fileData">���Ƭ</param>
        void Add(IFileData fileData);
        /// <summary>
        ///     ��⵱ǰ���ļ����Ƿ��Ѿ���������
        /// </summary>
        /// <returns>����ȷʵ���ļ���ID</returns>
        int[] CheckComplate();
        /// <summary>
        ///     ��ȡ���ļ�������������������
        /// </summary>
        /// <returns>���������Ķ���������</returns>
        /// <exception cref="System.Exception">������������</exception>
        byte[] GetData();
    }
}