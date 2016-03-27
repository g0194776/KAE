using System;
using KJFramework.Messages.Types;
using NUnit.Framework;

namespace KJFramework.Messages.UnitTest
{
    [TestFixture]
    public class BitFlagTest
    {
        #region Methods

        [Test]
        public void SetTest()
        {
            BitFlag flag = new BitFlag();
            flag[0] = true;
            flag[2] = true;
            byte value = ConvertToByte(flag);
            Assert.IsTrue(value == 0x05);
            Console.WriteLine(flag);
        }

        [Test]
        public void GetTest()
        {
            BitFlag flag = new BitFlag(0x05);
            Assert.IsTrue(flag[0]);
            Assert.IsTrue(flag[2]);
            Console.WriteLine(flag);
        }

        [Test]
        public void ToStringTest()
        {
            BitFlag flag = new BitFlag();
            flag[5] = true;
            Assert.AreEqual(flag.ToString(), "(0-0-0-0-0-1-0-0)");
            Console.WriteLine(flag);
        }

        [Test]
        public void ToStringTest2()
        {
            BitFlag flag = new BitFlag(0xFF);
            Assert.AreEqual(flag.ToString(), "(1-1-1-1-1-1-1-1)");
            Console.WriteLine(flag);
        }

        [Test]
        public void ToStringTest3()
        {
            BitFlag flag = new BitFlag();
            Assert.AreEqual(flag.ToString(), "(0-0-0-0-0-0-0-0)");
            Console.WriteLine(flag);
        }

        [Test]
        public void GetDataTest()
        {
            BitFlag flag = new BitFlag();
            flag[0] = true;
            flag[2] = true;
            byte value = flag.GetData();
            Assert.IsTrue(value == 0x05);
            Console.WriteLine(flag);

            flag = new BitFlag(0x05);
            Console.WriteLine(flag);
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