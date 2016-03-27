namespace KJFramework.Messages.Contracts
{
    /// <summary>
    ///     ���ܶ���ӿڣ��ṩ�˶�������ת�������������Ļ���֧�֡�
    /// </summary>
    public interface IIntellectObject
    {
        /// <summary>
        ///     ��ȡ�����ö�����������
        /// </summary>
        byte[] Body { get; set; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�Ƿ��Ѿ��ӵ������ͻ�����ת��ΪԪ���ݡ�
        /// </summary>
        bool IsBind { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰʵ�����ǲ����Լ���ģʽ�����ġ�
        /// </summary>
        bool CompatibleMode { get; }
        /// <summary>
        ///     �󶨵�Ԫ����
        /// </summary>
        void Bind();
    }
}