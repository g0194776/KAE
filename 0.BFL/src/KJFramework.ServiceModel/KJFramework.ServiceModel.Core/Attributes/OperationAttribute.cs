using System;

namespace KJFramework.ServiceModel.Core.Attributes
{
    /// <summary>
    ///     操作属性，表示指定方法为服务中的开放操作。
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OperationAttribute : System.Attribute
    {
        #region 成员

        private bool _isOneWay;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前方法的操作是否是单向的。
        /// </summary>
        public bool IsOneWay
        {
            get { return _isOneWay; }
            set { _isOneWay = value; }
        }

        private int _methodToken;
        /// <summary>
        ///     获取或设置方法序列标记
        ///     <para>* 当客户端持有的契约为远程创建的副本(非引用程序集)时，必须标注此属性。</para>
        /// </summary>
        public int MethodToken
        {
            get { return _methodToken; }
            set { _methodToken = value; }
        }

        private bool _isAsync;
        /// <summary>
        ///     获取或设置一个值，该值表示了当前方法是否会异步执行
        ///     <para>* 如果一个方法的IsAsync = true, 那么必须设置该方法的Name属性，而且对于重载方法，请不要设置一样的Name，此属性用于回调函数的区别。</para>
        /// </summary>
        public bool IsAsync
        {
            get { return _isAsync; }
            set { _isAsync = value; }
        }

        #endregion
    }
}