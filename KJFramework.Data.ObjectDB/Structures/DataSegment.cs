using System.IO.MemoryMappedFiles;

namespace KJFramework.Data.ObjectDB.Structures
{
    /// <summary>
    ///     ����Ƭ��
    ///     <para>* ����Ƭ�εı�ţ��Ǵ�1��ʼ��</para>
    /// </summary>
    internal class DataSegment : IDataSegment
    {
        #region Constructor

        /// <summary>
        ///     ����Ƭ��
        ///     <para>* ����Ƭ�εı�ţ��Ǵ�1��ʼ��</para>
        /// </summary>
        /// <param name="mappFile">�ڴ�ӳ���ļ����</param>
        /// <param name="segmentId">����Ƭ�α��</param>
        /// <param name="startOffset">��ʼƫ��</param>
        public DataSegment(MemoryMappedFile mappFile, ushort segmentId, uint startOffset)
        {
            _mappFile = mappFile;
            _segmentId = segmentId;
            _startOffset = startOffset*segmentId;
        }

        #endregion

        #region Members

        private readonly uint _startOffset;
        private readonly ushort _segmentId;
        private readonly MemoryMappedFile _mappFile;

        #endregion

        #region Implementation of IDataSegment

        /// <summary>
        ///     ��ȡ����Ƭ��ͷ
        /// </summary>
        public ISegmentHead Head { get; private set; }

        /// <summary>
        ///     ��ȡ��ǰ����Ƭ�εĽ�����
        /// </summary>
        public float Health { get; private set; }

        /// <summary>
        ///     д��һ�����ݷ�Χ
        /// </summary>
        /// <param name="dataRage">���ݷ�Χ</param>
        public void Write(IDataRange dataRage)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     ��ȡһ�����ݷ�Χ
        /// </summary>
        /// <param name="offset">��ȡ��ʼƫ��</param>
        /// <returns>�������ݷ�Χ</returns>
        public IDataRange Read(ushort offset)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     ��ȡ��ǰ����Ƭ���ڲ����е�����
        /// </summary>
        /// <returns>�������ݷ�Χ�ļ���</returns>
        public IDataRange[] ReadAll()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///     �����ڲ�������
        /// </summary>
        public void Arrange()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}