using System;
using System.Collections.Generic;
using KJFramework.Enums;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Statistics;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     ���ܵ����ʹ����������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class IntellectTypeProcessor : IIntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     ���ܵ����ʹ����������࣬�ṩ����صĻ���������
        /// </summary>
        protected IntellectTypeProcessor()
        {
            _id = Guid.NewGuid();
            //support this act by default.
            _supportUnmanagement = true;
        }

        #endregion

        #region Members

        private Guid _id;
        protected int? _supportedId;
        protected Type _supportedType;
        protected bool _supportUnmanagement;
        protected Dictionary<StatisticTypes,IStatistic> _statistics = new Dictionary<StatisticTypes, IStatistic>();

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// ��ȡ������ͳ����
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _statistics; }
            set { _statistics = value; }
        }

        #endregion

        #region Implementation of IIntellectTypeProcessor

        /// <summary>
        ///     ��ȡΨһ���
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ�֧���Է��йܵķ�ʽ����ִ��
        /// </summary>
        public bool SupportUnmanagement
        {
            get { return _supportUnmanagement; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��Ҫ�����Id��š�
        ///     <para>* ��һ�����ܶ�������Լ����д���ָ���ı�����ԣ��򽫻ύ�������ʹ���������</para>
        ///     <para>* ��SupportedId == nullʱ�������˵�ǰ�������ʹ��������������Ե�ID��ֻ�������Ե����͡�</para>
        /// </summary>
        public int? SupportedId
        {
            get { return _supportedId; }
            set { _supportedId = value; }
        }

        /// <summary>
        ///     ��ȡ֧�ֵ�����
        /// </summary>
        public Type SupportedType
        {
            get { return _supportedType; }
        }

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="attribute">�ֶ�����</param>
        /// <param name="analyseResult">�������</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        public abstract void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        ///     <para>* �˷������ᱻ��������DataHelper��ʹ�ã�����д������ݽ�����ӵ�б��(Id)</para>
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        public abstract void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public abstract object Process(IntellectPropertyAttribute attribute, byte[] data);
        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="instance">Ŀ�����</param>
        /// <param name="result">�������</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ�������ڵ�ƫ����</param>
        /// <param name="length">Ԫ���ݳ���</param>
        public abstract void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0);

        #endregion
    }
}