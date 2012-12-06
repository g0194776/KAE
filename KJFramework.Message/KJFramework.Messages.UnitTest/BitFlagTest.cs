using System;
using System.Collections;
using KJFramework.Messages.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Messages.UnitTest
{
    [TestClass]
    public class BitFlagTest
    {
        #region Methods

        [TestMethod]
        public void SetTest()
        {
            BitFlag flag = new BitFlag();
            flag[0] = true;
            flag[2] = true;
            byte value = ConvertToByte(flag);
            Assert.IsTrue(value == 0x05);
        }

        [TestMethod]
        public void GetTest()
        {
            BitFlag flag = new BitFlag(0x05);
            Assert.IsTrue(flag[0]);
            Assert.IsTrue(flag[2]);
        }

        public static byte ConvertToByte(BitFlag bits)
        {
            byte result = 0;
            for (byte i = 0; i < 8; i++)
            {
                if (bits[i])
                    result |= (byte)(1 << i);
            }
            return result;
        }

        #endregion
    }
}