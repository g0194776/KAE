using System;
using System.Collections;

namespace KJFramework.Messages.Types
{
    /// <summary>
    ///     位标示表示类
    ///     <para>* 可容纳8位的标示.</para>
    /// </summary>
    public class BitFlag
    {
        #region Constructor

        /// <summary>
        ///     位标示表示类
        /// </summary>
        public BitFlag()
        {
            //empty.
            _arr = new BitArray(new byte[] {0x00});
        }

        /// <summary>
        ///     位标示表示类
        /// </summary>
        /// <param name="value">初始值</param>
        public BitFlag(byte value)
        {
            _arr = new BitArray(new[] {value});
        }

        #endregion

        #region Members

        private BitArray _arr;

        #endregion

        #region Methods

        /// <summary>
        ///     获取或设置指定索引位的值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>返回相应的值</returns>
        /// <exception cref="ArgumentException">非法的索引</exception>
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > 8) throw new ArgumentException("Illegal flag index! #" + index);
                return _arr[index];
            }
            set
            {
                if (index < 0 || index > 8) throw new ArgumentException("Illegal flag index! #" + index);
                _arr[index] = value;
            }
        }

        #endregion
    }
}