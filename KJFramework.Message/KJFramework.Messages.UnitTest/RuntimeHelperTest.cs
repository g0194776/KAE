using System;
using KJFramework.Messages.Helpers;
using KJFramework.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Messages.UnitTest
{
    [TestClass]
    public class RuntimeHelperTest
    {
        [TestMethod]
        [Description("含有普通数据类型的智能对象长度计算测试")]
        public void NormalSizeTest1()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型的智能对象长度计算测试",1,delegate
            {
                Test1 test1 = new Test1();
                int size = RuntimeHelper.CalcSize(test1);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及字符串的智能对象长度计算测试")]
        public void NormalSizeTest2()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及字符串的智能对象长度计算测试", 1, delegate
            {
                Test2 test2 = new Test2();
                int size = RuntimeHelper.CalcSize(test2);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);
                //set value
                test2.Name = "Kevin";
                size = RuntimeHelper.CalcSize(test2);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 24);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及整型数组的智能对象长度计算测试")]
        public void NormalSizeTest3()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及字符串的智能对象长度计算测试", 1, delegate
            {
                Test3 test3 = new Test3();
                int size = RuntimeHelper.CalcSize(test3);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 19);
                //set value
                test3.Numbers = new[] {1, 2, 3};
                size = RuntimeHelper.CalcSize(test3);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 40);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及字符串数组的智能对象长度计算测试")]
        public void NormalSizeTest4()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及字符串数组的智能对象长度计算测试", 1, delegate
            {
                Test4 test4 = new Test4();
                int size = RuntimeHelper.CalcSize(test4);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);                
                //set value
                test4.Names = new[] { null, "Kline" };
                size = RuntimeHelper.CalcSize(test4);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 32);
                //set value
                test4.Names = new[] { null, ""};
                size = RuntimeHelper.CalcSize(test4);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 27);
                //set value
                test4.Names = new[] {"Kevin", "Kline"};
                size = RuntimeHelper.CalcSize(test4);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 37);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及内嵌一个智能对象的长度计算测试")]
        public void NormalSizeTest5()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及内嵌一个智能对象的长度计算测试", 1, delegate
            {
                Test5 test5 = new Test5();
                int size = RuntimeHelper.CalcSize(test5);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);
                //set value
                test5.InnerObject = new Test2();
                size = RuntimeHelper.CalcSize(test5);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 29);
                //set value
                test5.InnerObject = new Test2 {Name = "Kevin"};
                size = RuntimeHelper.CalcSize(test5);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 39);
                //set value
                test5.InnerObject = new Test2 { Name = "" };
                size = RuntimeHelper.CalcSize(test5);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 29);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及内嵌一个智能对象数组的长度计算测试")]
        public void NormalSizeTest6()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及内嵌一个智能对象数组的长度计算测试", 1, delegate
            {
                Test6 test6 = new Test6();
                int size = RuntimeHelper.CalcSize(test6);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);
                //set value
                test6.InnerObjects = new[] { null, new Test2() };
                size = RuntimeHelper.CalcSize(test6);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 41);
                //set value
                test6.InnerObjects = new[] {new Test2 {Name = "Kevin"}, new Test2()};
                size = RuntimeHelper.CalcSize(test6);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 65);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及枚举的长度计算测试")]
        public void NormalSizeTest7()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及枚举的长度计算测试", 1, delegate
            {
                Test7 test7 = new Test7();
                int size = RuntimeHelper.CalcSize(test7);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 16);
            });
        }

        [TestMethod]
        [Description("含有普通数据类型以及字节数组的长度计算测试")]
        public void NormalSizeTest8()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及字节数组的长度计算测试", 1, delegate
            {
                Test10 test10 = new Test10();
                int size = RuntimeHelper.CalcSize(test10);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);
                //set valuet
                test10.Data = new byte[] {0x01, 0x02};
                size = RuntimeHelper.CalcSize(test10);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 25);
            }); 
        }

        [TestMethod]
        [Description("含有普通数据类型以及枚举数组的长度计算测试")]
        public void NormalSizeTest9()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("含有普通数据类型以及枚举数组的长度计算测试", 1, delegate
            {
                Test23 test23 = new Test23();
                int size = RuntimeHelper.CalcSize(test23);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 14);
                //set valuet
                test23.Color = new[] {Colors.Yellow, Colors.Red};
                size = RuntimeHelper.CalcSize(test23);
                Console.WriteLine("Calc size: " + size);
                Assert.IsTrue(size == 25);
            });
        }
    }
}
