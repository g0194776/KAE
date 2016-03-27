using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     ��������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDescriptionMethod : IServiceMethod
    {
        /// <summary>
        ///     ��ȡ���иò����Ĳ���
        /// </summary>
        /// <returns>���ز�������</returns>
        IDescriptionArgument[] GetArguments();
        /// <summary>
        ///     ��ȡ�����ı�ʾ
        /// </summary>
        int MethodToken { get; }
    }
}