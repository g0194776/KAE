using System;
using KJFramework.ServiceModel.Elements;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     �������ڵ�Դ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface INetServiceNode
    {
        /// <summary>
        ///     ���ӵ�һ��Զ�̵ķ���ڵ�
        /// </summary>
        /// <param name="binding">�����ַ�󶨶���</param>
        /// <returns>�������Ӻ�Ŀͻ��˴���</returns>
        T Connect<T>(Binding binding) where T : class;
        /// <summary>
        ///     ��ȡԶ�̷������
        /// </summary>
        /// <typeparam name="T">Զ�̷�����Լ����</typeparam>
        /// <param name="uri">Զ�̷����ַ</param>
        /// <returns>���ؿͻ��˴���</returns>
        T GetService<T>(Uri uri) where T : class;
        /// <summary>
        ///     ע��һ��Զ�̷���
        /// </summary>
        /// <param name="binding">�󶨷�ʽ</param>
        /// <param name="type">��Լ����</param>
        void Regist(Binding binding, Type type);
        /// <summary>
        ///     ע��һ��Զ�̷���
        /// </summary>
        /// <param name="uri">���õ�ַ</param>
        void UnRegist(Uri uri);
    }
}