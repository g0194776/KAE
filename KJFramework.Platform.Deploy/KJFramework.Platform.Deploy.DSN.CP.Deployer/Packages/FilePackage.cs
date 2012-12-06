using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Logger;

namespace KJFramework.Platform.Deploy.DSN.CP.Deployer.Packages
{
    /// <summary>
    ///     文件包，提供了相关的基本操作。
    /// </summary>
    internal class FilePackage : IFilePackage
    {
        #region Constructor

        /// <summary>
        ///     文件包，提供了相关的基本操作。
        /// </summary>
        /// <param name="requestToken">请求令牌</param>
        public FilePackage(string requestToken)
        {
            _requestToken = requestToken;
        }

        #endregion

        #region Implementation of IFilePackage

        private string _requestToken;
        private int _totalPackageCount;
        private SortedDictionary<int, IFileData> _datas = new SortedDictionary<int, IFileData>();
        private bool _isComplate;
        private string _serviceName;
        private string _name;
        private string _version;
        private string _description;

        /// <summary>
        ///     获取当前文件数据包所关联的请求令牌
        /// </summary>
        public string RequestToken
        {
            get { return _requestToken; }
        }

        /// <summary>
        ///     获取或设置总共的封包片数目
        /// </summary>
        public int TotalPackageCount
        {
            get { return _totalPackageCount; }
            set { _totalPackageCount = value; }
        }

        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置服务版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     添加一个文件数据封包片
        /// </summary>
        /// <param name="fileData">封包片</param>
        public void Add(IFileData fileData)
        {
            if (fileData == null)
            {
                throw new ArgumentNullException("fileData");
            }
            if (!_datas.ContainsKey(fileData.CurrentId))
            {
                _datas.Add(fileData.CurrentId, fileData);
                return;
            }
            throw new System.Exception("Can not add a file data to this file package #" + _requestToken + ", beacuse a same key already existed.");
        }

        /// <summary>
        ///     检测当前的文件包是否已经接收完整
        /// </summary>
        /// <returns>返回确实的文件包ID</returns>
        public int[] CheckComplate()
        {
            if (_datas.Count == _totalPackageCount)
            {
                _isComplate = true;
                return null;
            }
            int[] noneId = new int[_datas.Count - _totalPackageCount];
            int currentOffset = 0;
            int lastFlag = 0;
            foreach (KeyValuePair<int, IFileData> pair in _datas)
            {
                for (int i = lastFlag; i < _datas.Count; i++)
                {
                    if (pair.Key != i)
                    {
                        noneId[currentOffset++] = i;
                        continue;
                    }
                    lastFlag = i + 1;
                    break;
                }
            }
            return noneId;
        }

        /// <summary>
        ///     获取此文件包的完整二进制数据
        /// </summary>
        /// <returns>返回完整的二进制数据</returns>
        /// <exception cref="System.Exception">不完整的数据</exception>
        public byte[] GetData()
        {
            if (!_isComplate)
            {
                throw new System.Exception("UnComplate data. for this file package. #" + _requestToken);
            }
            try
            {
                //get total length for this file package.
                int sum = _datas.Sum(d => d.Value.Data.Length);
                int offset = 0;
                byte[] data = new byte[sum];
                foreach (KeyValuePair<int, IFileData> pair in _datas)
                {
                    Buffer.BlockCopy(pair.Value.Data, 0, data, offset, pair.Value.Data.Length);
                    offset += pair.Value.Data.Length;
                }
                return data;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        #endregion
    }
}