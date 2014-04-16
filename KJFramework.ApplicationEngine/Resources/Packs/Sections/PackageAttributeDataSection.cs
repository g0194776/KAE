using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Hosting;
using System.Text;
using KJFramework.Messages.Exceptions;

namespace KJFramework.ApplicationEngine.Resources.Packs.Sections
{
    /// <summary>
    ///    KPP资源包中的包属性节
    /// </summary>
    internal class PackageAttributeDataSection : MarshalByRefObject, IKPPDataResource
    {
        #region Constructor.

        /// <summary>
        ///    KPP资源包中的包属性节
        /// </summary>
        static PackageAttributeDataSection()
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
            _keys.Add("PackName", typeof(string));
            _keys.Add("PackDescription", typeof(string));
            _keys.Add("EnvironmentFlag", typeof(byte));
            _keys.Add("Version", typeof(string));
            _keys.Add("PackTime", typeof(DateTime));
            _keys.Add("ApplicationMainFileName", typeof(string));
            _keys.Add("GlobalUniqueIdentity", typeof(Guid));
            _keys.Add("SectionLength", typeof(int));
            _keys.Add("ApplicationLevel", typeof(byte));
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
            return (T) _values[name];
        }

        //internal method.
        internal T GetFieldSafety<T>(string name)
        {
            object obj;
            return _values.TryGetValue(name, out obj) ? (T) obj : default(T);
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
            if(!(value.GetType() == destType)) throw new UnexpectedValueException(string.Format("#Illegal value's destination data type. #Target field type should be: " +destType.Name ));
            _values[name] = value;
        }

        /// <summary>
        ///    将一个目录下的所有文件打包为一个KPP资源包
        /// </summary>
        /// <param name="stream">资源流</param>
        public unsafe void Pack(MemoryStream stream)
        {
            //section id.
            stream.WriteByte(0x00);
            //section length.
            byte[] sectionLengthData = BitConverter.GetBytes(0);
            stream.Write(sectionLengthData, 0, sectionLengthData.Length);
            /*logs start position.*/
            long position = stream.Position;
            //pack name.
            string pckName = GetFieldSafety<string>("PackName");
            byte[] pckNameData = string.IsNullOrEmpty(pckName) ? null : Encoding.UTF8.GetBytes(pckName);
            byte[] pckNameLengthData = BitConverter.GetBytes(pckNameData == null ? 0 : pckNameData.Length);
            stream.Write(pckNameLengthData, 0, pckNameLengthData.Length);
            if (pckNameData != null) stream.Write(pckNameData, 0, pckNameData.Length);
            //pack description.
            string pckDescription = GetFieldSafety<string>("PackDescription");
            byte[] pckDescriptionData = string.IsNullOrEmpty(pckDescription) ? null : Encoding.UTF8.GetBytes(pckDescription);
            byte[] pckDescriptionLengthData = BitConverter.GetBytes(pckDescriptionData == null ? 0 : pckDescriptionData.Length);
            stream.Write(pckDescriptionLengthData, 0, pckDescriptionLengthData.Length);
            if (pckDescriptionData != null) stream.Write(pckDescriptionData, 0, pckDescriptionData.Length);
            //env flag.
            byte envFlag = GetFieldSafety<byte>("EnvironmentFlag");
            stream.WriteByte(envFlag);
            //version.
            string version = GetFieldSafety<string>("Version");
            byte[] versionData = string.IsNullOrEmpty(version) ? null : Encoding.UTF8.GetBytes(version);
            byte[] versionLengthData = BitConverter.GetBytes(versionData == null ? 0 : versionData.Length);
            stream.Write(versionLengthData, 0, versionLengthData.Length);
            if (versionData != null) stream.Write(versionData, 0, versionData.Length);
            //pack time.
            DateTime pckTime = GetFieldSafety<DateTime>("PackTime");
            byte[] pckTimeData = new byte[8];
            fixed (byte* pByte = pckTimeData) *(DateTime*) pByte = pckTime;
            stream.Write(pckTimeData, 0, pckTimeData.Length);
            //application main file name.
            string mainFile = GetFieldSafety<string>("ApplicationMainFileName");
            byte[] mainFileData = string.IsNullOrEmpty(mainFile) ? null : Encoding.UTF8.GetBytes(mainFile);
            byte[] mainFileLengthData = BitConverter.GetBytes(mainFileData == null ? 0 : mainFileData.Length);
            stream.Write(mainFileLengthData, 0, mainFileLengthData.Length);
            if (mainFileData != null) stream.Write(mainFileData, 0, mainFileData.Length);
            //application level.
            stream.WriteByte(GetFieldSafety<byte>("ApplicationLevel"));
            //global unique identity.
            Guid identity = GetFieldSafety<Guid>("GlobalUniqueIdentity");
            byte[] identityData = new byte[16];
            fixed (byte* pByte = identityData) *(Guid*)pByte = identity;
            stream.Write(identityData, 0, identityData.Length);
            /*writes back section length*/
            long endingPos = stream.Position;
            int length = (int) (stream.Position - position);
            stream.Position = position - 4;
            stream.Write(BitConverter.GetBytes(length), 0, 4);
            stream.Position = endingPos;
        }

        /// <summary>
        ///    将一个目录下的PP资源包进行解包
        /// </summary>
        /// <param name="stream">资源流</param>
        public unsafe void UnPack(MemoryStream stream)
        {
            //section id
            byte id = (byte) stream.ReadByte();
            if (id != 0x00) throw new FormatException("#Target is NOT a package attribute data section.");
            //section length
            byte[] sectionLengthData = new byte[4];
            stream.Read(sectionLengthData, 0, sectionLengthData.Length);
            SetField("SectionLength", BitConverter.ToInt32(sectionLengthData, 0));
            //pack name.
            byte[] pckNameLengthData = new byte[4];
            stream.Read(pckNameLengthData, 0, pckNameLengthData.Length);
            int pckNameLength = BitConverter.ToInt32(pckNameLengthData, 0);
            string pckName = string.Empty;
            if (pckNameLength > 0)
            {
                byte[] pckNameData = new byte[pckNameLength];
                stream.Read(pckNameData, 0, pckNameData.Length);
                pckName = Encoding.UTF8.GetString(pckNameData);
            }
            SetField("PackName", pckName);
            //pack description.
            byte[] pckDescriptionLengthData = new byte[4];
            stream.Read(pckDescriptionLengthData, 0, pckDescriptionLengthData.Length);
            int pckDescriptionLength = BitConverter.ToInt32(pckDescriptionLengthData, 0);
            string pckDescription = string.Empty;
            if (pckDescriptionLength > 0)
            {
                byte[] pckDescriptionData = new byte[pckDescriptionLength];
                stream.Read(pckDescriptionData, 0, pckDescriptionData.Length);
                pckDescription = Encoding.UTF8.GetString(pckDescriptionData);
            }
            SetField("PackDescription", pckDescription);
            //env flag.
            byte[] envFlagData = new byte[1];
            stream.Read(envFlagData, 0, envFlagData.Length);
            SetField("EnvironmentFlag", envFlagData[0]);
            //version.
            byte[] versionLengthData = new byte[4];
            stream.Read(versionLengthData, 0, versionLengthData.Length);
            int versionLength = BitConverter.ToInt32(versionLengthData, 0);
            string version = string.Empty;
            if (versionLength > 0)
            {
                byte[] versionData = new byte[versionLength];
                stream.Read(versionData, 0, versionData.Length);
                version = Encoding.UTF8.GetString(versionData);
            }
            SetField("Version", version);
            //pack time.
            byte[] pckTimeData = new byte[8];
            stream.Read(pckTimeData, 0, pckTimeData.Length);
            DateTime pckTime;
            fixed (byte* pByte = pckTimeData) pckTime = *(DateTime*)pByte;
            SetField("PackTime", pckTime);
            //application main file name.
            byte[] mainFileLengthData = new byte[4];
            stream.Read(mainFileLengthData, 0, mainFileLengthData.Length);
            int mainFileLength = BitConverter.ToInt32(mainFileLengthData, 0);
            string mainFile = string.Empty;
            if (mainFileLength > 0)
            {
                byte[] mainFileData = new byte[mainFileLength];
                stream.Read(mainFileData, 0, mainFileData.Length);
                mainFile = Encoding.UTF8.GetString(mainFileData);
            }
            SetField("ApplicationMainFileName", mainFile);
            //application level.
            byte applicationLevel = (byte) stream.ReadByte();
            SetField("ApplicationLevel", applicationLevel);
            //global unique identity.
            byte[] identityData = new byte[16];
            stream.Read(identityData, 0, identityData.Length);
            Guid identity;
            fixed (byte* pByte = identityData) identity = *(Guid*)pByte;
            SetField("GlobalUniqueIdentity", identity);
        }

        #endregion
    }
}