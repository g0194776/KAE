using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ��չ���춯��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IExtendBuildAction : IBuildAction
    {
        /// <summary>
        ///     ����һ����Ϣ��չ
        /// </summary>
        /// <param name="attribute">�ֶα�ǩ</param>
        /// <param name="data">�����ֶ�Ԫ����</param>
        /// <returns>������չֵ</returns>
        byte[] Bind(IntellectPropertyAttribute attribute, byte[] data);
        /// <summary>
        ///     ��ȡ��Ϣ��չֵ
        /// </summary>
        /// <typeparam name="T">��Ϣ��չ����</typeparam>
        /// <param name="attribute">�ֶα�ǩ</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>������ȡ������Ϣ��չֵ</returns>
        T Pickup<T>(IntellectPropertyAttribute attribute, byte[] data);
    }
}