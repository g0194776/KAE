using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     远程服务
    /// </summary>
    public class RemotingService : IRemotingService
    {
        #region Members

        private readonly string _path;
        private readonly bool _canAsync;
        private readonly string _previewPath;
        private IList<IMethod> _methods; 

        #endregion

        #region Constructor

        public RemotingService()
        {
            _path = "http://localhost:65300/demo1";
            _previewPath = "http://localhost:65300/demo1/Preview";
            _canAsync = true;
            LoadPreviewData();
        }

        /// <summary>
        ///     远程服务
        /// </summary>
        /// <param name="path">远程服务地址</param>
        public RemotingService(string path, bool canAsync)
        {
            _path = path;
            _canAsync = canAsync;
            _previewPath = path + "/Preview";
            LoadPreviewData();
        }

        #endregion

        #region Implementation of IRemotingService

        private IServiceInfomation _infomation;

        /// <summary>
        ///     获取服务相关信息
        /// </summary>
        public IServiceInfomation Infomation
        {
            get { return _infomation; }
        }

        /// <summary>
        ///     获取服务的描述方法集合
        /// </summary>
        /// <returns>返回描述方法集合</returns>
        public IList<IMethod> GetPreviewMethods()
        {
            return _methods;
        }

        #endregion

        #region Methods

        private void LoadPreviewData()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_previewPath);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string metadata = stream.ReadToEnd();
                XmlDocument document = new XmlDocument();
                document.LoadXml(metadata);
                //infomation
                XmlNode infoNode = document.ChildNodes[1].ChildNodes[0];
                _infomation = new ServiceInfomation
                                  {
                                      Description = infoNode.Attributes["description"].InnerText,
                                      Name = infoNode.Attributes["name"].InnerText,
                                      Version = infoNode.Attributes["version"].InnerText,
                                      FullName = "I" + infoNode.Attributes["full-name"].InnerText,
                                      EndPoint = _path
                                  };
                //contracts.
                if (document.ChildNodes[1].ChildNodes[1].ChildNodes.Count > 0)
                {
                    _methods = new List<IMethod>(document.ChildNodes[1].ChildNodes[1].ChildNodes.Count);
                    foreach (XmlNode node in document.ChildNodes[1].ChildNodes[1].ChildNodes)
                    {
                        string retType = node.Attributes["ret"].InnerText;
                        XmlAttribute cusAttr = node.Attributes["referenceId"];
                        Method method = new Method(_path)
                                            {
                                                FullName = node.Attributes["fullsign"].InnerText,
                                                IsAsync = bool.Parse(node.Attributes["isasync"].InnerText),
                                                IsOneway = bool.Parse(node.Attributes["isoneway"].InnerText),
                                                MethodToken = node.Attributes["token"].InnerText,
                                                Name = node.Attributes["name"].InnerText,
                                                HasReturnValue = retType != "void",
                                                ReturnType = retType,
                                                ReferenceId = cusAttr != null ? cusAttr.InnerText : null
                                            };
                        method.InitializeArgument();
                        _methods.Add(method);
                        if (_canAsync) _methods.Add(new AsyncMethod((Method) method.Clone(), _path));
                    }
                }
            }
            response.Close();
        }

        #endregion
    }
}