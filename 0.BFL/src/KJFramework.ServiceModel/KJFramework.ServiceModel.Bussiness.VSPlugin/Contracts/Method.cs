using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///         Ԥ������
    /// </summary>
    //[DebuggerDisplay("{Name}")]
    public class Method : IMethod
    {
        #region Members

        protected readonly string _basePath;
        protected IList<IArgument> _arguments;
        protected static readonly string _template = "{0} {1}({2})";

        #endregion

        #region Constructor


        /// <summary>
        ///     Ԥ������
        /// </summary>
        /// <param name="basePath">��Լ������ַ</param>
        public Method(string basePath)
        {
            _basePath = basePath;
        }

        #endregion

        #region Implementation of IPreviewMethod

        protected string _referenceId;
        protected string _returnType;
        protected bool _hasReturnValue;
        protected string _name;
        protected string _fullName;
        protected string _methodToken;
        protected bool _isAsync;
        protected bool _isOneway;

        /// <summary>
        ///     ��ȡ������
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            internal set { _name = value; }
        }

        /// <summary>
        ///     ��ȡ����ȫ��
        /// </summary>
        public virtual string FullName
        {
            get { return _fullName; }
            internal set { _fullName = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public virtual string MethodToken
        {
            get { return _methodToken; }
            internal set { _methodToken = value; }
        }

        /// <summary>
        ///     ��ȡ����ֵ����
        /// </summary>
        public virtual string ReturnType
        {
            get { return _returnType; }
            internal set { _returnType = value; }
        }

        /// <summary>
        ///     ����ֵ���Ͷ���Id
        ///     <para>* ��ֵ����IsRetTypeCustome = trueʱ��Ч.</para>
        /// </summary>
        public virtual string ReferenceId
        {
            get { return _referenceId; }
            internal set { _referenceId = value; }
        }

        /// <summary>
        ///      ��ȡһ����ʾ���ñ�ʾ��ʾ�˵�ǰ����ʱ����з���ֵ
        /// </summary>
        public virtual bool HasReturnValue
        {
            get { return _hasReturnValue; }
            internal set { _hasReturnValue = value; }
        }

        /// <summary>
        ///     ��ȡһ����ʾ���ñ�ʾ��ʾ�˵�ǰ�����Ƿ�Ϊ�첽����
        /// </summary>
        public virtual bool IsAsync
        {
            get { return _isAsync; }
            internal set { _isAsync = value; }
        }

        /// <summary>
        ///     ��ȡһ����ʾ���ñ�ʾ��ʾ�˵�ǰ�����Ƿ�Ϊ�����
        /// </summary>
        public virtual bool IsOneway
        {
            get { return _isOneway; }
            internal set { _isOneway = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public virtual IList<IArgument> Arguments
        {
            get { return _arguments; }
            internal set { _arguments = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼ��������Ϣ
        /// </summary>
        internal virtual void InitializeArgument()
        {
            string argPath = string.Format(_basePath + "/Metadata/Operations.aspx?Name={0}&Token={1}", _name, _methodToken);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(argPath);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string metadata = stream.ReadToEnd();
                XmlDocument document = new XmlDocument();
                document.LoadXml(metadata);
                if (document.ChildNodes[0].ChildNodes.Count != 0)
                {
                    _arguments = new List<IArgument>(document.ChildNodes[0].ChildNodes.Count);
                    foreach (XmlNode node in document.ChildNodes[0].ChildNodes)
                    {
                        XmlAttribute cusAttr = node.Attributes["reference"];
                        IArgument argument = new Argument
                                                 {
                                                     Id = int.Parse(node.Attributes["id"].InnerText),
                                                     Name = node.Attributes["name"].InnerText,
                                                     Type = node.Attributes["fullname"].InnerText,
                                                     ReferenceId = cusAttr == null ? null : cusAttr.InnerText
                                                 };
                        _arguments.Add(argument);
                    }
                }
            }
            response.Close();
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            Method method = new Method(_basePath)
            {
                FullName = _fullName,
                IsAsync = _isAsync,
                IsOneway = _isOneway,
                MethodToken = _methodToken,
                Name = _name,
                HasReturnValue = _hasReturnValue,
                ReturnType = _returnType,
                ReferenceId = _referenceId
            };
            if (_arguments != null)
            {
                method.Arguments = new List<IArgument>();
                foreach (IArgument argument in _arguments)
                {
                    method.Arguments.Add(new Argument
                    {
                        Id = argument.Id,
                        Name = argument.Name,
                        ReferenceId = argument.ReferenceId,
                        Type = argument.Type
                    });
                }
            }
            return method;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            string arguments = string.Empty;
            if(Arguments != null)
            {
                for (int i = 0; i < Arguments.Count; i++)
                {
                    IArgument currArg = Arguments[i];
                    arguments += (i == Arguments.Count - 1
                                      ? currArg.Type + currArg.Name
                                      : currArg.Type + currArg.Name + " ");
                }
            }
            return string.Format(_template, ReturnType, Name, arguments);
        }
        #endregion
    }
}