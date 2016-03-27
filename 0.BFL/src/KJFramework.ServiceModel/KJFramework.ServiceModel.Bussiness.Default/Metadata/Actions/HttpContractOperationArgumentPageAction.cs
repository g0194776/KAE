using System;
using System.Linq;
using System.Net;
using System.Text;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;
using KJFramework.ServiceModel.Core.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     契约操作相关参数信息页面动作
    /// </summary>
    internal class HttpContractOperationArgumentPageAction : HttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     契约操作相关参数信息页面动作
        /// </summary>
        /// <param name="contractDescription">契约描述接口</param>
        /// <param name="methodToken">方法令牌</param>
        public HttpContractOperationArgumentPageAction(IContractDescription contractDescription, int methodToken)
            : base(contractDescription)
        {
            _methodToken = methodToken;
        }

        #endregion

        #region Members

        private readonly int _methodToken;

        #endregion

        #region Methods

        /// <summary>
        ///     执行动作
        /// </summary>
        /// <param name="httpListenerRequest">HTTP输入请求</param>
        public override string Execute(HttpListenerRequest httpListenerRequest)
        {
            if (_result != null) return _result;
            IDescriptionMethod method = _contractDescription.GetMethods().Where(m => m.MethodToken == _methodToken).First();
            IDescriptionArgument[] arguments = method.GetArguments();
            if (arguments == null || arguments.Length == 0)
            {
                _result = "<arguments></arguments>";
                return _result;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<arguments>");
            foreach (IDescriptionArgument argument in arguments)
            {
                if (MetadataTypeGenerator.IsDefaultType(argument.ParameterType))
                    stringBuilder.AppendLine(string.Format(@"<argument id=""{0}"" cannull=""{1}"" name=""{2}"" fullname=""{3}""/>", argument.Id, argument.CanNull, argument.Name, argument.FullName));
                else
                    stringBuilder.AppendLine(string.Format(@"<argument id=""{0}"" cannull=""{1}"" name=""{2}"" fullname=""{3}"" reference=""{4}""/>", argument.Id, argument.CanNull, argument.Name, argument.FullName, Math.Abs(argument.ParameterType.FullName.GetHashCode())));
            }
            stringBuilder.AppendLine("</arguments>");
            _result = stringBuilder.ToString();
            return _result;
        }

        #endregion
    }
}