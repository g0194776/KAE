using KJFramework.IO.Helper;
using KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata
{
    /// <summary>
    ///     元数据交换网络节点抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class MetadataExchangeNode : IMetadataExchangeNode
    {
        #region Constructor

        /// <summary>
        ///     元数据交换网络节点抽象父类，提供了相关的基本操作。
        /// </summary>
        public MetadataExchangeNode()
        {
             Initialize();
        }

        #endregion

        #region Members

        protected bool _isEnable;
        protected Dictionary<String, IHttpMetadataPageAction> _actions = new Dictionary<string, IHttpMetadataPageAction>();
        private static Dictionary<Type, String> _typeTable = new Dictionary<Type, String>();
        protected readonly Dictionary<string, string> _argMetadatas = new Dictionary<string, string>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(MetadataExchangeNode));

        #endregion

        #region Implementation of IMetadataExchangeNode

        /// <summary>
        ///     注册一个将要开放元数据的服务契约
        /// </summary>
        /// <param name="path">服务路径</param>
        /// <param name="pageAction">元数据页面动作</param>
        public void Regist(String path, IHttpMetadataPageAction pageAction)
        {
            #region lazy open metadata exchange node

            try
            {
                //lazy open.
                if (!_isEnable)
                {
                    InitializeMetadataNode();
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }

            #endregion

            try
            {
                if (pageAction == null) throw new ArgumentNullException("pageAction");
                if (String.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
                if (_actions.ContainsKey(path)) _actions.Remove(path);
                _actions.Add(path, pageAction);
                #if (DEBUG)
                {
                    ConsoleHelper.PrintLine("Registed Page: " + path, ConsoleColor.DarkCyan);
                }
                #endif
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     注册一个参数描述信息
        /// </summary>
        /// <param name="argumentId">参数编号</param>
        /// <param name="argMetadata">参数描述信息</param>
        public void Regist(string argumentId, string argMetadata)
        {
            if (string.IsNullOrEmpty(argumentId)) throw new ArgumentNullException("argumentId");
            if (string.IsNullOrEmpty(argMetadata)) throw new ArgumentNullException("argMetadata");
            if(!_argMetadatas.ContainsKey(argumentId))
                _argMetadatas.Add(argumentId, argMetadata);
        }

        /// <summary>
        ///     根据指定参数编号获取参数描述信息
        /// </summary>
        /// <param name="argumentId">参数编号</param>
        /// <returns>参数描述信息</returns>
        public string GetArgumentMetadata(string argumentId)
        {
            string content;
            return _argMetadatas.TryGetValue(argumentId, out content) ? content : null;
        }

        /// <summary>
        ///     注销一个开放的服务契约
        /// </summary>
        /// <param name="name">契约名称</param>
        public void UnRegist(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }
            _actions.Remove(name);
            #if (DEBUG)
            {
                ConsoleHelper.PrintLine("UnRegisted Page: " + name, ConsoleColor.DarkRed);
            }
            #endif
        }

        /// <summary>
        ///     开启元数据交换
        /// </summary>
        public abstract void Start();

        /// <summary>
        ///     停止元数据交换
        /// </summary>
        public abstract void Stop();

        #endregion

        #region Methods

        /// <summary>
        ///     初始化
        /// </summary>
        private void Initialize()
        {
            _typeTable.Add(typeof(int), String.Empty);
            _typeTable.Add(typeof(double), String.Empty);
            _typeTable.Add(typeof(long), String.Empty);
            _typeTable.Add(typeof(float), String.Empty);
            _typeTable.Add(typeof(ulong), String.Empty);
            _typeTable.Add(typeof(ushort), String.Empty);
            _typeTable.Add(typeof(short), String.Empty);
            _typeTable.Add(typeof(SByte), String.Empty);
            _typeTable.Add(typeof(byte), String.Empty);
            _typeTable.Add(typeof(DateTime), String.Empty);
            _typeTable.Add(typeof(Type), String.Empty);
            _typeTable.Add(typeof(String), String.Empty);
            _typeTable.Add(typeof(decimal), String.Empty);
        }

        /// <summary>
        ///     判断当前类型是否为.NETFRAMEWORK的内置类型
        ///     <para>* 目前此方法只做为简单的数据类型判断</para>
        /// </summary>
        /// <param name="type">要判断的类型</param>
        /// <returns>返回判断的结果</returns>
        public static bool IsFrameworkType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return _typeTable.ContainsKey(type);
        }

        /// <summary>
        ///     初始化开放元数据节点
        /// </summary>
        protected abstract void InitializeMetadataNode();

        #endregion
    }
}