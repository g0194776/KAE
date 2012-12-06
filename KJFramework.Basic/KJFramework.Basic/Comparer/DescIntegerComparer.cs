using System.Collections.Generic;

namespace KJFramework.Basic.Comparer
{
    /// <summary>
    ///     按照降序排序的整型比较器
    /// </summary>
    public class DescIntegerComparer : IComparer<int>
    {
        #region IComparer<int> 成员

        public int Compare(int x, int y)
        {
            if(x > y)
            {
                return 1;
            }
            if (x == y)
            {
                return 0;
            }
            return -1;
        }

        #endregion
    }
}
