using System;

namespace KJFramework.Messages.Extends.Rules
{
    /// <summary>
    ///     分割规则，提供了相关的基本属性结构。
    /// </summary>
    public class SplitRule : ISplitRule
    {
        #region 析构函数

        ~SplitRule()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        protected Type _supportType;
        protected int _splitLength;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ISplitRule

        /// <summary>
        ///     获取或设置支持的类型
        /// </summary>
        public Type SupportType
        {
            get { return _supportType; }
            set { _supportType = value; }
        }

        /// <summary>
        ///     获取或设置分割长度
        /// </summary>
        public int SplitLength
        {
            get { return _splitLength; }
            set { _splitLength = value; }
        }


        /// <summary>
        ///     取值检查
        ///     <para>* 此方法通常用于检测可选字段是否拥有值时的判断。</para>
        /// </summary>
        /// <param name="offset">当前元数据的偏移量</param>
        /// <param name="data">元数据</param>
        /// <param name="tagOffset">
        ///     返回的附属偏移量
        ///     <para>* 如果返回的结果是false的话，就需要填写附属偏移量。</para>
        /// </param>
        /// <param name="targetContentLength">当前要截取的内容长度</param>
        /// <returns>返回是否决定取值的结果</returns>
        public virtual bool Check(int offset, byte[] data, ref int tagOffset, ref int targetContentLength)
        {
            tagOffset = 0;
            targetContentLength = SplitRuleManager.GetRule(_supportType).SplitLength;
            return true;
        }

        #endregion
    }
}