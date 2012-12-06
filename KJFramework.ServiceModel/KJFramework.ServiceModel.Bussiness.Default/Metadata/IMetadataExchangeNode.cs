using System;
using KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata
{
    /// <summary>
    ///     Ԫ���ݽ�������ڵ�Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IMetadataExchangeNode
    {
        /// <summary>
        ///     ע��һ����Ҫ����Ԫ���ݵķ�����Լ
        /// </summary>
        /// <param name="path">����·��</param>
        /// <param name="pageAction">Ԫ����ҳ�涯��</param>
        void Regist(String path, IHttpMetadataPageAction pageAction);
        /// <summary>
        ///     ע��һ������������Ϣ
        /// </summary>
        /// <param name="argumentId">�������</param>
        /// <param name="argMetadata">����������Ϣ</param>
        void Regist(string argumentId, string argMetadata);
        /// <summary>
        ///     ����ָ��������Ż�ȡ����������Ϣ
        /// </summary>
        /// <param name="argumentId">�������</param>
        /// <returns>����������Ϣ</returns>
        string GetArgumentMetadata(string argumentId);
        /// <summary>
        ///     ע��һ�����ŵķ�����Լ
        /// </summary>
        /// <param name="name">��Լ����</param>
        void UnRegist(String name);
        /// <summary>
        ///     ����Ԫ���ݽ���
        /// </summary>
        void Start();
        /// <summary>
        ///     ֹͣԪ���ݽ���
        /// </summary>
        void Stop();
    }
}