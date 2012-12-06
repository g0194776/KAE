using System;
using System.Collections.Generic;
using KJFramework.Messages.Extends.Actions;
using KJFramework.Messages.Extends.Splitters;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     ��������Ϣ�ṹ������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ICustomerMessageDefiner : IDisposable
    {
        /// <summary>
        ///     ��ȡ������Ψһ���
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ȡ��������ֵ
        /// </summary>
        String Key { get; }
        /// <summary>
        ///     ��ȡ��չ��������
        /// </summary>
        SortedSet<IExtendBuildAction> ExtendActions { get; }
        /// <summary>
        ///     ��ȡ��Ϣ�ֶι��춯��
        /// </summary>
        IFieldBuildAction FieldAction { get; }
        /// <summary>
        ///     ��ȡ��Ϣͷ�����춯��
        /// </summary>
        IHeadBuildAction HeadAction { get; }
        /// <summary>
        ///     ��ȡ��Ϣ��β�����춯��
        /// </summary>
        IEndBuildAction EndAction { get; }
        /// <summary>
        ///     ��ȡ��ϢԪ�����ֶη�����
        /// </summary>
        IMetadataFieldSplitter Splitter { get; }
        /// <summary>
        ///     �������Ϸ���
        /// </summary>
        void Check();
    }
}