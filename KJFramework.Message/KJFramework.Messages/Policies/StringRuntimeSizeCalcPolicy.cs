using System.Text;

namespace KJFramework.Messages.Policies
{
    /// <summary>
    ///     字符串运行时长度计算策略
    /// </summary>
    internal class StringRuntimeSizeCalcPolicy : IRuntimeSizeCalcPolicy
    {
        #region Implementation of IRuntimeSizeCalcPolicy

        /// <summary>
        ///     计算一个类型的运行时长度
        /// </summary>
        /// <param name="obj">被计算的对象</param>
        /// <param name="isArray">当前的判断条件是否为数组元素</param>
        /// <returns>返回计算后的长度</returns>
        public int Calc(object obj, bool isArray = false)
        {
            string str = (string)obj;
            //length(4)
            if (!isArray) return string.IsNullOrEmpty(str) ? 4 : (4 + Encoding.UTF8.GetByteCount(str));
            return string.IsNullOrEmpty(str) ? 0 : Encoding.UTF8.GetByteCount(str);
        }

        #endregion
    }
}