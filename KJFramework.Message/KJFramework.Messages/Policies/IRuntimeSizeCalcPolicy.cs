namespace KJFramework.Messages.Policies
{
    /// <summary>
    ///     对象运行时长度计算策略接口
    /// </summary>
    public interface IRuntimeSizeCalcPolicy
    {
        /// <summary>
        ///     计算一个类型的运行时长度
        /// </summary>
        /// <param name="obj">被计算的对象</param>
        /// <param name="isArray">当前的判断条件是否为数组元素</param>
        /// <returns>返回计算后的长度</returns>
        int Calc(object obj, bool isArray = false);
    }
}