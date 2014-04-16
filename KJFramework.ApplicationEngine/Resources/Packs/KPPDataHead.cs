using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.Messages.Exceptions;

namespace KJFramework.ApplicationEngine.Resources.Packs
{
    /// <summary>
    ///    KPP资源文件头
    /// </summary>
    public class KPPDataHead : MarshalByRefObject, IKPPDataResource
    {
        #region Constructor.

        /// <summary>
        ///    KPP资源包中的包属性节
        /// </summary>
        static KPPDataHead()
        {
            _keys = new Dictionary<string, Type>();
            InitializeKey();
        }

        #endregion

        #region Members.

        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private static readonly Dictionary<string, Type> _keys; 

        #endregion

        #region Methods.

        /// <summary>
        ///    初始化当前数据节所有合法的KEY
        /// </summary>
        private static void InitializeKey()
        {
            _keys.Add("TotalSize", typeof(ulong));
            _keys.Add("CRC", typeof(long));
            _keys.Add("SectionCount", typeof(ushort));
        }

        /// <summary>
        ///    获取具有指定名称字段数据信息
        /// </summary>
        /// <typeparam name="T">字段数据信息类型</typeparam>
        /// <param name="name">字段名</param>
        /// <returns>返回指定名称所代表的数据信息</returns>
        /// <exception cref="KeyNotFoundException">指定的KEY不存在</exception>
        public T GetField<T>(string name)
        {
            return (T)_values[name];
        }

        //internal method.
        internal T GetFieldSafety<T>(string name)
        {
            object obj;
            return _values.TryGetValue(name, out obj) ? (T)obj : default(T);
        }

        /// <summary>
        ///    设置一个具有指定名称字段的数据信息
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">数据值</param>
        /// <exception cref="KeyNotFoundException">指定的KEY不存在</exception>
        /// <exception cref="UnexpectedValueException">非法的数据值类型</exception>
        public void SetField(string name, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            Type destType;
            if (!_keys.TryGetValue(name, out destType)) throw new KeyNotFoundException(string.Format("#Illegal resource key: {0}", name));
            if (!(value.GetType() == destType)) throw new UnexpectedValueException(string.Format("#Illegal value's destination data type."));
            _values[name] = value;
        }

        /// <summary>
        ///    将一个目录下的所有文件打包为一个KPP资源包
        /// </summary>
        /// <param name="stream">资源流</param>
        public void Pack(MemoryStream stream)
        {
            //file mark.
            byte[] fileMark = { (byte)'K', (byte)'P', (byte)'P' };
            stream.Write(fileMark, 0, fileMark.Length);
            //total length.
            byte[] totalLength = BitConverter.GetBytes(GetFieldSafety<ulong>("TotalSize"));
            stream.Write(totalLength, 0, totalLength.Length);
            //CRC.
            byte[] crc = BitConverter.GetBytes(GetFieldSafety<long>("CRC"));
            stream.Write(crc, 0, crc.Length);
            //section count.
            byte[] sCount = BitConverter.GetBytes(GetFieldSafety<ushort>("SectionCount"));
            stream.Write(sCount, 0, sCount.Length);
        }

        /// <summary>
        ///    将一个目录下的PP资源包进行解包
        /// </summary>
        /// <param name="stream">资源流</param>
        /// <exception cref="BadImageFormatException">错误的KPP资源包格式</exception>
        public unsafe void UnPack(MemoryStream stream)
        {
            //file mark.
            byte[] fileMark = new byte[3];
            stream.Read(fileMark, 0, fileMark.Length);
            if ((char) fileMark[0] != 'K') throw new BadImageFormatException("#Bad KPP file format.");
            if ((char) fileMark[1] != 'P') throw new BadImageFormatException("#Bad KPP file format.");
            if ((char) fileMark[2] != 'P') throw new BadImageFormatException("#Bad KPP file format.");
            //total length.
            byte[] totalLengthData = new byte[8];
            stream.Read(totalLengthData, 0, totalLengthData.Length);
            ulong totalLength;
            fixed (byte* pByte = totalLengthData) totalLength = *(ulong*) pByte;
            SetField("TotalSize", totalLength);
            //CRC.
            byte[] crcData = new byte[8];
            stream.Read(crcData, 0, crcData.Length);
            long crc;
            fixed (byte* pByte = crcData) crc = *(long*)pByte;
            SetField("CRC", crc);
            //section count.
            byte[] sectionCountData = new byte[2];
            stream.Read(sectionCountData, 0, sectionCountData.Length);
            ushort sectionCount;
            fixed (byte* pByte = sectionCountData) sectionCount = *(ushort*)pByte;
            SetField("SectionCount", sectionCount);
        }

        #endregion
    }
}