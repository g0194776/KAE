using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Enums;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.Types;
using KJFramework.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Messages.UnitTest
{
    [TestClass]
    public class IntellectObjectEngineUnitTest
    {
        [TestMethod]
        [Description("准字节数测试")]
        public void NormalBindTest()
        {
            Test1 test1 = new Test1 { ProtocolId = 1, ServicelId = 2 };
            test1.Bind();
            Assert.IsTrue(test1.IsBind);
            Assert.IsTrue(test1.Body.Length == 14);
            PrintBytes(test1.Body);
        }

        [TestMethod]
        [Description("准字节数解析测试")]
        public void NormalPickupTest()
        {
            Test1 test1 = new Test1 { ProtocolId = 1, ServicelId = 2 };
            test1.Bind();
            Assert.IsTrue(test1.IsBind);
            Assert.IsTrue(test1.Body.Length == 14);
            PrintBytes(test1.Body);
            Test1 newObj = IntellectObjectEngine.GetObject<Test1>(typeof(Test1), test1.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
        }

        [TestMethod]
        [Description("带有浮动数据类型的标准测试")]
        public void NormalBindTest1()
        {
            Test2 test2 = new Test2 { ProtocolId = 1, ServicelId = 2 };
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsTrue(test2.Body.Length == 14);
            PrintBytes(test2.Body);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "" };
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsTrue(test2.Body.Length == 19);
            PrintBytes(test2.Body);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "YangJie" };
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsTrue(test2.Body.Length == 26);
            PrintBytes(test2.Body);
        }

        [TestMethod]
        [Description("带有浮动数据类型的标准解析测试")]
        public void NormalPickupTest1()
        {
            Test2 test2 = new Test2 { ProtocolId = 1, ServicelId = 2 };
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsTrue(test2.Body.Length == 14);
            PrintBytes(test2.Body);
            Test2 newObj = IntellectObjectEngine.GetObject<Test2>(typeof(Test2), test2.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "" };
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsTrue(test2.Body.Length == 19);
            PrintBytes(test2.Body);
            newObj = IntellectObjectEngine.GetObject<Test2>(typeof(Test2), test2.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Name == null);

            test2 = new Test2 { ProtocolId = 1, ServicelId = 2, Name = "YangJie我~" };
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsTrue(test2.Body.Length == 30);
            PrintBytes(test2.Body);
            newObj = IntellectObjectEngine.GetObject<Test2>(typeof(Test2), test2.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Name == "YangJie我~");
        }

        [TestMethod]
        [Description("带有固定数据类型数组的标准测试")]
        public void NormalBindTest2()
        {
            Test3 test3 = new Test3 { ProtocolId = 1, ServicelId = 2 };
            test3.Numbers = new[] { 1111, 2222 };
            test3.Bind();
            Assert.IsTrue(test3.IsBind);
            Assert.IsTrue(test3.Body.Length == 36);
            PrintBytes(test3.Body);
        }

        [TestMethod]
        [Description("带有固定数据类型数组的标准解析测试")]
        public void NormalPickupTest2()
        {
            Test3 test3 = new Test3 { ProtocolId = 1, ServicelId = 2 };
            test3.Numbers = new[] { 1111, 2222 };
            test3.Bind();
            Assert.IsTrue(test3.IsBind);
            Assert.IsTrue(test3.Body.Length == 36);
            PrintBytes(test3.Body);

            Test3 newObj = IntellectObjectEngine.GetObject<Test3>(typeof(Test3), test3.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Numbers);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Numbers.Length == 2);
            Assert.IsTrue(newObj.Numbers[0] == 1111);
            Assert.IsTrue(newObj.Numbers[1] == 2222);
        }

        [TestMethod]
        [Description("带有浮动数据类型数组的标准测试")]
        public void NormalBindTest3()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", "jie" };
            test4.Bind();
            Assert.IsTrue(test4.IsBind);
            Assert.IsTrue(test4.Body.Length == 34);
            PrintBytes(test4.Body);
        }

        [TestMethod]
        [Description("带有浮动数据类型数组的标准解析测试")]
        public void NormalPickupTest3()
        {
            Test4 test4 = new Test4 { ProtocolId = 1, ServicelId = 2 };
            test4.Names = new[] { "yang", null, "jie" };
            test4.Bind();
            Assert.IsTrue(test4.IsBind);
            Assert.IsTrue(test4.Body.Length == 36);
            PrintBytes(test4.Body);

            Test4 newObj = IntellectObjectEngine.GetObject<Test4>(typeof(Test4), test4.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Names);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Names.Length == 3);
            Assert.IsTrue(newObj.Names[0] == "yang");
            Assert.IsTrue(newObj.Names[1] == null);
            Assert.IsTrue(newObj.Names[2] == "jie");
        }

        [TestMethod]
        [Description("具有内部智能对象自包含的标准测试")]
        public void SelfContainNormalBindTest()
        {
            Test5 test5 = new Test5 { ProtocolId = 1, ServicelId = 2 };
            test5.InnerObject = new Test2 { ProtocolId = 4, ServicelId = 5, Name = "Kevin" };
            test5.Bind();
            Assert.IsTrue(test5.IsBind);
            PrintBytes(test5.Body);
            Assert.IsTrue(test5.Body.Length == 43);
        }

        [TestMethod]
        [Description("具有内部智能对象自包含的标准解析测试")]
        public void SelfContainNormalPickupTest()
        {
            Test5 test5 = new Test5 { ProtocolId = 1, ServicelId = 2 };
            test5.InnerObject = new Test2 { ProtocolId = 4, ServicelId = 5, Name = "Kevin" };
            test5.Bind();
            Assert.IsTrue(test5.IsBind);
            Assert.IsTrue(test5.Body.Length == 43);
            PrintBytes(test5.Body);

            Test5 newObj = IntellectObjectEngine.GetObject<Test5>(typeof(Test5), test5.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.InnerObject);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.InnerObject.Name == "Kevin");
            Assert.IsTrue(newObj.InnerObject.ProtocolId == 4);
            Assert.IsTrue(newObj.InnerObject.ServicelId == 5);
        }

        [TestMethod]
        [Description("具有内部智能对象数组的标准测试")]
        public void SelfContainNormalBindTest1()
        {
            Test6 test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
            test6.InnerObjects = new[]
                                     {
                                         new Test2 {ProtocolId = 4, ServicelId = 5, Name = "Yang"},
                                         new Test2 {ProtocolId = 6, ServicelId = 7, Name = "Jie"}
                                     };
            test6.Bind();
            Assert.IsTrue(test6.IsBind);
            PrintBytes(test6.Body);
            test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
            test6.InnerObjects = new[]
                                     {
                                         null,
                                         new Test2 {ProtocolId = 6, ServicelId = 7, Name = "Jie"}
                                     };
            test6.Bind();
            Assert.IsTrue(test6.IsBind);
            PrintBytes(test6.Body);
        }

        [TestMethod]
        [Description("具有内部智能对象数组的标准解析测试")]
        public void SelfContainNormalPickupTest1()
        {
            Test6 test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
            test6.InnerObjects = new[]
                                     {
                                         new Test2 {ProtocolId = 4, ServicelId = 5, Name = "Yang"},
                                         new Test2 {ProtocolId = 6, ServicelId = 7, Name = "Jie"}
                                     };
            test6.Bind();
            Assert.IsTrue(test6.IsBind);
            PrintBytes(test6.Body);
            Test6 newObj = IntellectObjectEngine.GetObject<Test6>(typeof(Test6), test6.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.InnerObjects);
            Assert.IsTrue(test6.ProtocolId == 1);
            Assert.IsTrue(test6.ServicelId == 2);
            Assert.IsTrue(newObj.InnerObjects.Length == 2);
            Assert.IsNotNull(newObj.InnerObjects[0]);
            Assert.IsTrue(newObj.InnerObjects[0].ProtocolId == 4);
            Assert.IsTrue(newObj.InnerObjects[0].ServicelId == 5);
            Assert.IsTrue(newObj.InnerObjects[0].Name == "Yang");
            Assert.IsNotNull(newObj.InnerObjects[1]);
            Assert.IsTrue(newObj.InnerObjects[1].ProtocolId == 6);
            Assert.IsTrue(newObj.InnerObjects[1].ServicelId == 7);
            Assert.IsTrue(newObj.InnerObjects[1].Name == "Jie");

            test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
            test6.InnerObjects = new[]
                                     {
                                         null,
                                         new Test2 {ProtocolId = 6, ServicelId = 7, Name = "Jie"}
                                     };
            test6.Bind();
            Assert.IsTrue(test6.IsBind);
            PrintBytes(test6.Body);
            newObj = IntellectObjectEngine.GetObject<Test6>(typeof(Test6), test6.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.InnerObjects);
            Assert.IsTrue(test6.ProtocolId == 1);
            Assert.IsTrue(test6.ServicelId == 2);
            Assert.IsTrue(newObj.InnerObjects.Length == 2);
            Assert.IsNull(newObj.InnerObjects[0]);
            Assert.IsNotNull(newObj.InnerObjects[1]);
            Assert.IsTrue(newObj.InnerObjects[1].ProtocolId == 6);
            Assert.IsTrue(newObj.InnerObjects[1].ServicelId == 7);
            Assert.IsTrue(newObj.InnerObjects[1].Name == "Jie");

            test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
            test6.InnerObjects = new Test2[] { null, null };
            test6.Bind();
            Assert.IsTrue(test6.IsBind);
            Assert.IsNotNull(test6.Body);
            Assert.IsTrue(test6.Body.Length == 27);
            PrintBytes(test6.Body);
            newObj = IntellectObjectEngine.GetObject<Test6>(typeof(Test6), test6.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.InnerObjects);
            Assert.IsTrue(test6.ProtocolId == 1);
            Assert.IsTrue(test6.ServicelId == 2);
            Assert.IsTrue(newObj.InnerObjects.Length == 2);
            Assert.IsNull(newObj.InnerObjects[0]);
            Assert.IsNull(newObj.InnerObjects[1]);

            test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
            test6.InnerObjects = new Test2[0];
            test6.Bind();
            Assert.IsTrue(test6.IsBind);
            Assert.IsNotNull(test6.Body);
            Assert.IsTrue(test6.Body.Length == 23);
            PrintBytes(test6.Body);
            newObj = IntellectObjectEngine.GetObject<Test6>(typeof(Test6), test6.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.InnerObjects);
            Assert.IsTrue(test6.ProtocolId == 1);
            Assert.IsTrue(test6.ServicelId == 2);
            Assert.IsTrue(newObj.InnerObjects.Length == 0);
        }

        [TestMethod]
        [Description("复杂对象绑定测试")]
        public void ComplexObjectBindTest()
        {
            TestObject testObject = new TestObject
                                 {
                                     Uu = new string[] { null, null },
                                     //Nn = new[] { new TestObject2 { Nice = 8888 }, null },
                                     Pp = new[] { null, new TestObject1 { Haha = "..." } },
                                     Jj = new[] { "Kevin", null, "Jee" },
                                     Mm = new[] { 9988, 9999 },
                                     Wocao = 111111,
                                     //Obj2 = new TestObject2 { Nice = 9090 },
                                     Wokao = 222222,
                                     Metadata1 = 99,
                                     Metadata = Encoding.Default.GetBytes("haha"),
                                     Time = DateTime.Now,
                                     Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                                     NullableValue1 = 10,
                                     NullableValue3 = 16
                                 };
            testObject.Bind();
            Assert.IsTrue(testObject.IsBind);
            PrintBytes(testObject.Body);
        }

        [TestMethod]
        [Description("复杂对象解析测试")]
        public void ComplexObjectPickupTest()
        {
            DateTime tt = DateTime.Now;
            TestObject testObject = new TestObject
            {
                Uu = new string[] { null, null },
                //Nn = new[] { new TestObject2 { Nice = 8888 }, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Mm = new[] { 9988, 9999 },
                Wocao = 111111,
                //Obj2 = new TestObject2 { Nice = 9090 },
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = tt,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                NullableValue1 = 10,
                NullableValue3 = 16
            };
            testObject.Bind();
            Assert.IsTrue(testObject.IsBind);
            PrintBytes(testObject.Body);
            TestObject newObj = IntellectObjectEngine.GetObject<TestObject>(typeof(TestObject), testObject.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Uu);
            Assert.IsNull(newObj.Uu[0]);
            Assert.IsNull(newObj.Uu[1]);
            Assert.IsNotNull(newObj.Pp);
            Assert.IsTrue(newObj.Pp.Length == 2);
            Assert.IsNull(newObj.Pp[0]);
            Assert.IsNotNull(newObj.Pp[1]);
            Assert.IsTrue(newObj.Pp[1].Haha == "...");
            Assert.IsNotNull(newObj.Jj);
            Assert.IsTrue(newObj.Jj.Length == 3);
            Assert.IsTrue(newObj.Jj[0] == "Kevin");
            Assert.IsNull(newObj.Jj[1]);
            Assert.IsTrue(newObj.Jj[2] == "Jee");
            Assert.IsNotNull(newObj.Mm);
            Assert.IsTrue(newObj.Mm.Length == 2);
            Assert.IsTrue(newObj.Mm[0] == 9988);
            Assert.IsTrue(newObj.Mm[1] == 9999);
            Assert.IsTrue(newObj.Time.Ticks == tt.Ticks);
            Assert.IsNotNull(newObj.Obj);
            Assert.IsTrue(newObj.Obj.Haha == "你觉得这样的能力可以了吗？");
            Assert.IsTrue(newObj.Obj.Colors == Colors.Yellow);
            Assert.IsTrue(newObj.NullableValue1 == 10);
            Assert.IsTrue(newObj.NullableValue3 == 16);
            Assert.IsNull(newObj.NullableValue2);
            Assert.IsNull(newObj.NullableValue4);
        }

        [TestMethod]
        [Description("极限绑定测试")]
        public void ExtremeBindTest()
        {
            IntellectObjectEngine.Preheat(new TestObject());
            TestObject[] objects = new TestObject[100000];
            Stopwatch watch = new Stopwatch();
            Stopwatch totalWatch = new Stopwatch();
            for (int i = 0; i < 100000; i++)
            {
                objects[i] = new TestObject
                {
                    Uu = new string[] { null, null },
                    //Nn = new[] { new TestObject2 { Nice = 8888 }, null },
                    Pp = new[] { null, new TestObject1 { Haha = "..." } },
                    Jj = new[] { "Kevin", null, "Jee" },
                    Mm = new[] { 9988, 9999 },
                    Wocao = 111111,
                    //Obj2 = new TestObject2 { Nice = 9090 },
                    Wokao = 222222,
                    Metadata1 = 99,
                    Metadata = Encoding.Default.GetBytes("haha"),
                    Time = DateTime.Now,
                    Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                    NullableValue1 = 10,
                    NullableValue3 = 16
                };
            }
            Console.WriteLine("Initialize 100000 objects successed.");
            List<TimeSpan> count = new List<TimeSpan>();
            CodeTimer.Initialize();
            CodeTimer.Time("Extreme Bind Test: 100000", 1, delegate
            {
                totalWatch.Start();
                for (int i = 0; i < 100000; i++)
                {
                    watch.Start();
                    objects[i].Bind();
                    watch.Stop();
                    count.Add(watch.Elapsed);
                    watch.Reset();
                }
                totalWatch.Stop();
            });
            Console.WriteLine();
            TimeSpan span = count.Min();
            Console.WriteLine("Bind object min time is " + span);
            Console.WriteLine("Bind object max time is " + count.Max());
            Console.WriteLine("Total bind spend time : " + totalWatch.Elapsed);
        }

        [TestMethod]
        [Description("极限解析测试")]
        public void ExtremePickupTest()
        {
            IntellectObjectEngine.Preheat(new TestObject());
            TestObject[] objects = new TestObject[100000];
            Stopwatch watch = new Stopwatch();
            Stopwatch totalWatch = new Stopwatch();
            for (int i = 0; i < 100000; i++)
            {
                objects[i] = new TestObject
                {
                    Uu = new string[] { null, null },
                    //Nn = new[] { new TestObject2 { Nice = 8888 }, null },
                    Pp = new[] { null, new TestObject1 { Haha = "..." } },
                    Jj = new[] { "Kevin", null, "Jee" },
                    Mm = new[] { 9988, 9999 },
                    Wocao = 111111,
                    //Obj2 = new TestObject2 { Nice = 9090 },
                    Wokao = 222222,
                    Metadata1 = 99,
                    Metadata = Encoding.Default.GetBytes("haha"),
                    Time = DateTime.Now,
                    Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow },
                    NullableValue1 = 10,
                    NullableValue3 = 16
                };
            }
            Console.WriteLine("Initialize 100000 objects successed.");
            List<TimeSpan> count = new List<TimeSpan>();
            totalWatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                watch.Start();
                objects[i].Bind();
                watch.Stop();
                count.Add(watch.Elapsed);
                watch.Reset();
            }
            totalWatch.Stop();
            Console.WriteLine();
            TimeSpan span = count.Min();
            Console.WriteLine("Bind object min time is " + span);
            Console.WriteLine("Bind object max time is " + count.Max());
            Console.WriteLine("Total bind spend time : " + totalWatch.Elapsed);

            Type t = typeof(TestObject);
            Stopwatch watch1 = new Stopwatch();
            CodeTimer.Initialize();
            CodeTimer.Time("Extreme Pickup Test: 100000", 1, delegate
            {
                watch1.Start();
                for (int i = 0; i < 100000; i++)
                {
                    IntellectObjectEngine.GetObject<TestObject>(t, objects[i].Body);
                }
                watch1.Stop();
            });
            Console.WriteLine("Total pickup spend time : " + watch1.Elapsed);
        }

        [TestMethod]
        [Description("带有固定数据类型枚举的准字节数测试")]
        public void NormalBindTest4()
        {
            Test7 test = new Test7();
            test.ProtocolId = 1;
            test.ServicelId = 2;
            test.Color = Colors.Yellow;
            test.Bind();
            Assert.IsTrue(test.IsBind);
            Assert.IsNotNull(test.Body);
            Assert.IsTrue(test.Body.Length == 16);
            PrintBytes(test.Body);
        }

        [TestMethod]
        [Description("带有固定数据类型枚举的准字节数解析测试")]
        public void NormalPickupTest4()
        {
            Test7 test = new Test7();
            test.ProtocolId = 1;
            test.ServicelId = 2;
            test.Color = Colors.Yellow;
            test.Bind();
            Assert.IsTrue(test.IsBind);
            Assert.IsNotNull(test.Body);
            Assert.IsTrue(test.Body.Length == 16);
            PrintBytes(test.Body);
            Test7 newObj = IntellectObjectEngine.GetObject<Test7>(typeof(Test7), test.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Color == Colors.Yellow);
        }

        [TestMethod]
        [Description("带有托管指针数据类型的准字节数测试")]
        public void NormalBindTest5()
        {
            Test8 test8 = new Test8 { ProtocolId = 1, ServicelId = 2, Ptr = new IntPtr(1024) };
            test8.Bind();
            Assert.IsNotNull(test8.Body);
            Assert.IsTrue(test8.Body.Length == 19);
            PrintBytes(test8.Body);
        }

        [TestMethod]
        [Description("带有托管指针数据类型的准字节数解析测试")]
        public void NormalPickupTest5()
        {
            IntPtr intPtr = new IntPtr(1024);
            Test8 test8 = new Test8 { ProtocolId = 1, ServicelId = 2, Ptr = intPtr };
            test8.Bind();
            Assert.IsNotNull(test8.Body);
            Assert.IsTrue(test8.Body.Length == 19);
            PrintBytes(test8.Body);
            Test8 newObj = IntellectObjectEngine.GetObject<Test8>(typeof(Test8), test8.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsTrue(newObj.Ptr.ToInt32() == intPtr.ToInt32());
        }

        [TestMethod]
        [Description("带有索引器的准字节数测试")]
        public void NormalBindTest6()
        {
            Test9 test9 = new Test9 { ProtocolId = 1, ServicelId = 2 };
            test9.Bind();
            Assert.IsNotNull(test9.Body);
            Assert.IsTrue(test9.Body.Length == 14);
            PrintBytes(test9.Body);
        }

        [TestMethod]
        [Description("带有索引器的准字节数解析测试")]
        public void NormalPickupTest6()
        {
            Test9 test9 = new Test9 { ProtocolId = 1, ServicelId = 2 };
            test9.Bind();
            Assert.IsNotNull(test9.Body);
            Assert.IsTrue(test9.Body.Length == 14);
            PrintBytes(test9.Body);
            Test9 newObj = IntellectObjectEngine.GetObject<Test9>(typeof(Test9), test9.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
        }

        [TestMethod]
        [Description("带有字节数组的准字节绑定测试")]
        public void NormalBindTest7()
        {
            Test10 test10 = new Test10 { ProtocolId = 1, ServicelId = 2, Data = new byte[] { 0x01, 0x02 } };
            test10.Bind();
            Assert.IsTrue(test10.IsBind);
            //make byte array is a object.
            Assert.IsTrue(test10.Body.Length == 25);
            PrintBytes(test10.Body);
        }

        [TestMethod]
        [Description("带有字节数组的准字节绑定测试")]
        public void NormalPickupTest7()
        {
            Test10 test10 = new Test10 { ProtocolId = 1, ServicelId = 2, Data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 } };
            test10.Bind();
            Assert.IsTrue(test10.IsBind);
            //make byte array is a object.
            Assert.IsTrue(test10.Body.Length == 28);
            PrintBytes(test10.Body);
            Test10 newObj = IntellectObjectEngine.GetObject<Test10>(typeof(Test10), test10.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsNotNull(newObj.Data);
            Assert.IsTrue(newObj.Data.Length == 5);
            Assert.IsTrue(newObj.Data[0] == 0x01);
            Assert.IsTrue(newObj.Data[1] == 0x02);
            Assert.IsTrue(newObj.Data[2] == 0x03);
            Assert.IsTrue(newObj.Data[3] == 0x04);
            Assert.IsTrue(newObj.Data[4] == 0x05);
        }

        [TestMethod]
        [Description("带有Guid类型的准字节绑定测试")]
        public void NormalBindTest8()
        {
            Test11 test11 = new Test11 { ProtocolId = 1, Guid = Guid.NewGuid(), ServiceId = 2 };
            test11.Bind();
            Assert.IsTrue(test11.IsBind);
            //make byte array is a object.
            Assert.IsTrue(test11.Body.Length == 31);
            PrintBytes(test11.Body);
        }

        [TestMethod]
        [Description("带有字节数组的准字节绑定测试")]
        public void NormalPickupTest8()
        {
            Test11 test11 = new Test11 { ProtocolId = 1, Guid = Guid.NewGuid(), ServiceId = 2 };
            test11.Bind();
            Assert.IsTrue(test11.IsBind);
            //make byte array is a object.
            Assert.IsTrue(test11.Body.Length == 31);
            PrintBytes(test11.Body);
            Test11 newObj = IntellectObjectEngine.GetObject<Test11>(typeof(Test11), test11.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(test11.Guid == newObj.Guid);
        }

        [TestMethod]
        [Description("普通对象ToString方法测试")]
        public void NormalToStringTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("NormalToStringTest", 1, delegate
            {
                Test10 test10 = new Test10 { ProtocolId = 1, ServicelId = 2, Data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 } };
                string content = test10.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
            });
        }

        [TestMethod]
        [Description("含有Guid对象的ToString方法测试")]
        public void HasGuidToStringTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("HasGuidToStringTest", 1, delegate
            {
                Test11 test11 = new Test11 { ProtocolId = 1, Guid = Guid.NewGuid(), ServiceId = 2 };
                string content = test11.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
            });
        }

        [TestMethod]
        [Description("含有IntPtr对象的ToString方法测试")]
        public void HasIntPtrToStringTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("HasIntPtrToStringTest", 1, delegate
            {
                Test8 test8 = new Test8 { ProtocolId = 1, ServicelId = 2, Ptr = new IntPtr(1024) };
                string content = test8.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
            });
        }

        [TestMethod]
        [Description("内嵌智能对象的ToString方法测试")]
        public void HasIntellectObjectToStringTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("HasIntellectObjectToStringTest", 1, delegate
            {
                Test5 test5 = new Test5 { ProtocolId = 1, ServicelId = 2 };
                test5.InnerObject = new Test2 { ProtocolId = 4, ServicelId = 5, Name = "Kevin" };
                string content = test5.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
                //null intellect object.
                test5 = new Test5 { ProtocolId = 1, ServicelId = 2 };
                content = test5.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
            });
        }

        [TestMethod]
        [Description("内部包含智能对象数组的ToString方法测试")]
        public void HasIntellectObjectArrayToStringTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("HasIntellectObjectArrayToStringTest", 1, delegate
            {
                Test6 test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
                test6.InnerObjects = new[]
                                         {
                                             new Test2 {ProtocolId = 4, ServicelId = 5, Name = "Yang"},
                                             new Test2 {ProtocolId = 6, ServicelId = 7, Name = "Jie"}
                                         };
                string content = test6.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
                //null intellect object.
                test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
                test6.InnerObjects = new[]
                                         {
                                             new Test2 {ProtocolId = 4, ServicelId = 5, Name = "Yang"},
                                             null
                                         };
                content = test6.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);

                //null intellect object.
                test6 = new Test6 { ProtocolId = 1, ServicelId = 2 };
                content = test6.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
            });
        }

        [TestMethod]
        [Description("复杂对象的ToString方法测试")]
        public void ComplexObjectToStringTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("ComplexObjectToStringTest", 1, delegate
            {
                DateTime tt = DateTime.Now;
                TestObject testObject = new TestObject
                {
                    Uu = new string[] { null, null },
                    //Nn = new[] { new TestObject2 { Nice = 8888 }, null },
                    Pp = new[] { null, new TestObject1 { Haha = "..." } },
                    Jj = new[] { "Kevin", null, "Jee" },
                    Mm = new[] { 9988, 9999 },
                    Wocao = 111111,
                    //Obj2 = new TestObject2 { Nice = 9090 },
                    Wokao = 222222,
                    Metadata1 = 99,
                    Metadata = Encoding.Default.GetBytes("haha"),
                    Time = tt,
                    Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow }
                };
                string content = testObject.ToString();
                Assert.IsNotNull(content);
                Console.WriteLine(content);
            });
        }

        [TestMethod]
        [Description("极限复杂对象ToString方法测试")]
        public void ExtremeComplexObjectToStringTest()
        {
            DateTime tt = DateTime.Now;
            TestObject testObject = new TestObject
            {
                Uu = new string[] { null, null },
                Pp = new[] { null, new TestObject1 { Haha = "..." } },
                Jj = new[] { "Kevin", null, "Jee" },
                Mm = new[] { 9988, 9999 },
                Wocao = 111111,
                Wokao = 222222,
                Metadata1 = 99,
                Metadata = Encoding.Default.GetBytes("haha"),
                Time = tt,
                Obj = new TestObject1 { Haha = "你觉得这样的能力可以了吗？", Colors = Colors.Yellow }
            };
            CodeTimer.Initialize();
            CodeTimer.Time("ExtremeComplexObjectToStringTest", 1, delegate
            {
                for (int i = 0; i < 100000; i++) Assert.IsNotNull(testObject.ToString());
            });
        }

        [TestMethod]
        public void UlongBindTest()
        {
            Test12 test12 = new Test12 {ProtocolId = 1, ServiceId = 2, DetailsId = 3};
            test12.Bind();
            Assert.IsTrue(test12.IsBind);
            Assert.IsTrue(test12.Body.Length == 23);
        }

        [TestMethod]
        public void UShortBindTest()
        {
            Test15 test15 = new Test15 { ProtocolId = 1, ServiceId = 2, DetailsId = 3 };
            test15.Bind();
            Assert.IsTrue(test15.IsBind);
            Assert.IsTrue(test15.Body.Length == 17);
        }

        [TestMethod]
        public void UIntBindTest()
        {
            Test16 test16 = new Test16 { ProtocolId = 1, ServiceId = 2, DetailsId = 3 };
            test16.Bind();
            Assert.IsTrue(test16.IsBind);
            Assert.IsTrue(test16.Body.Length == 19);
        }

        [TestMethod]
        public void UlongPickupTest()
        {
            Test12 test12 = new Test12 { ProtocolId = 1, ServiceId = 2, DetailsId = 3 };
            test12.Bind();
            Assert.IsTrue(test12.IsBind);
            Assert.IsTrue(test12.Body.Length == 23);

            Test12 newObj = IntellectObjectEngine.GetObject<Test12>(typeof (Test12), test12.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.DetailsId == 3);
        }


        [TestMethod]
        public void UShortPickupTest()
        {
            Test15 test15 = new Test15 { ProtocolId = 1, ServiceId = 2, DetailsId = 3 };
            test15.Bind();
            Assert.IsTrue(test15.IsBind);
            Assert.IsTrue(test15.Body.Length == 17);

            Test15 newObj = IntellectObjectEngine.GetObject<Test15>(typeof(Test15), test15.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.DetailsId == 3);
        }

        [TestMethod]
        public void UIntPickupTest()
        {
            Test16 test16 = new Test16 { ProtocolId = 1, ServiceId = 2, DetailsId = 3 };
            test16.Bind();
            Assert.IsTrue(test16.IsBind);
            Assert.IsTrue(test16.Body.Length == 19);

            Test16 newObj = IntellectObjectEngine.GetObject<Test16>(typeof(Test16), test16.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.DetailsId == 3);
        }

        [TestMethod]
        public void SbyteBindTest()
        {
            Test13 test13 = new Test13 { ProtocolId = 1, ServiceId = 2, DetailsId = 127 };
            test13.Bind();
            Assert.IsTrue(test13.IsBind);
            Assert.IsTrue(test13.Body.Length == 16);
            PrintBytes(test13.Body);
        }

        [TestMethod]
        public void SbytePickupTest()
        {
            Test13 test13 = new Test13 { ProtocolId = 1, ServiceId = 2, DetailsId = 127 };
            test13.Bind();
            Assert.IsTrue(test13.IsBind);
            Assert.IsTrue(test13.Body.Length == 16);
            PrintBytes(test13.Body);
            Test13 newObj = IntellectObjectEngine.GetObject<Test13>(typeof(Test13), test13.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.DetailsId == 127);
        }

        [TestMethod]
        public void DecimalBindTest()
        {
            Test14 test14 = new Test14 { ProtocolId = 1, ServiceId = 2, DetailsId = 250 };
            test14.Bind();
            Assert.IsTrue(test14.IsBind);
            Assert.IsTrue(test14.Body.Length == 31);
            PrintBytes(test14.Body);
        }

        [TestMethod]
        public void DecimalPickupTest()
        {
            Test14 test14 = new Test14 { ProtocolId = 1, ServiceId = 2, DetailsId = 250 };
            test14.Bind();
            Assert.IsTrue(test14.IsBind);
            Assert.IsTrue(test14.Body.Length == 31);
            PrintBytes(test14.Body);
            Test14 newObj = IntellectObjectEngine.GetObject<Test14>(typeof(Test14), test14.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.DetailsId == test14.DetailsId);
        }

        [TestMethod]
        [Description("序列化差异测试，用于检测相同类型的不同实例是否具有不同的二进制数据")]
        public void DifferenceInstanceTest()
        {
            Test1 instance1 = new Test1 {ProtocolId = 1, ServicelId = 2};
            instance1.Bind();
            Assert.IsTrue(instance1.IsBind);
            PrintBytes(instance1.Body);
            Test1 obj1 = IntellectObjectEngine.GetObject<Test1>(instance1.Body);
            Assert.IsNotNull(obj1);
            Assert.IsTrue(obj1.ProtocolId == 1);
            Assert.IsTrue(obj1.ServicelId == 2);
            Test1 instance2 = new Test1 { ProtocolId = 3, ServicelId = 4 };
            instance2.Bind();
            Assert.IsTrue(instance2.IsBind);
            PrintBytes(instance2.Body);
            Test1 obj2 = IntellectObjectEngine.GetObject<Test1>(instance2.Body);
            Assert.IsNotNull(obj2);
            Assert.IsTrue(obj2.ProtocolId == 3);
            Assert.IsTrue(obj2.ServicelId == 4);
        }

        [TestMethod]
        [Description("可空类型一般性绑定二进制数据测试")]
        public void NullableTypeBindTest1()
        {
            Test17 test17 = new Test17 {ProtocolId = 1, ServiceId = 2};
            test17.Bind();
            Assert.IsTrue(test17.IsBind);
            Assert.IsTrue(test17.Body.Length == 14);
            PrintBytes(test17.Body);
        }

        [TestMethod]
        [Description("可空类型一般性绑定二进制数据测试")]
        public void NullableTypeBindTest2()
        {
            Test17 test17 = new Test17 { ProtocolId = 1, ServiceId = 2, DetailsId = 3};
            test17.Bind();
            Assert.IsTrue(test17.IsBind);
            Assert.IsTrue(test17.Body.Length == 19);
            PrintBytes(test17.Body);
        }

        [TestMethod]
        [Description("可空类型一般性绑定数据解析测试")]
        public void NullableTypePickupTest1()
        {
            Test17 test17 = new Test17 { ProtocolId = 1, ServiceId = 2, DetailsId = 3 };
            test17.Bind();
            Assert.IsTrue(test17.IsBind);
            Assert.IsTrue(test17.Body.Length == 19);
            PrintBytes(test17.Body);

            Test17 newObj = IntellectObjectEngine.GetObject<Test17>(test17.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.DetailsId == 3);
        }

        [TestMethod]
        [Description("可空类型一般性绑定数据解析测试")]
        public void NullableTypePickupTest2()
        {
            Test17 test17 = new Test17 { ProtocolId = 1, ServiceId = 2 };
            test17.Bind();
            Assert.IsTrue(test17.IsBind);
            Assert.IsTrue(test17.Body.Length == 14);
            PrintBytes(test17.Body);

            Test17 newObj = IntellectObjectEngine.GetObject<Test17>(test17.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsNull(newObj.DetailsId);
        }

        [TestMethod]
        [Description("空数组的绑定和解析一般性测试")]
        public void EmptyArrayTest()
        {
            Test18 test18 = new Test18 {Users = new int[0], ProtocolId = 1, ServiceId = 2};
            test18.Bind();
            Assert.IsTrue(test18.IsBind);
            Assert.IsTrue(test18.Body.Length == 23);
            PrintBytes(test18.Body);

            Test18 newobj = IntellectObjectEngine.GetObject<Test18>(test18.Body);
            Assert.IsTrue(newobj.ProtocolId == 1);
            Assert.IsTrue(newobj.ServiceId == 2);
            Assert.IsNotNull(newobj.Users);
            Assert.IsTrue(newobj.Users.Length == 0);
        }

        [TestMethod]
        [Description("BitFlag绑定一般性测试")]
        public void BitFlagToBytesTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("BitFlag::ToBytes", 1, delegate
            {
                Test19 test19 = new Test19 { Flag = new BitFlag(), ProtocolId = 1, ServiceId = 2 };
                test19.Flag[0] = true;
                test19.Flag[2] = true;
                test19.Bind();
                Assert.IsTrue(test19.IsBind);
                Assert.IsTrue(test19.Body.Length == 16);
                PrintBytes(test19.Body);
            });
        }

        [TestMethod]
        [Description("BitFlag解析一般性测试")]
        public void BitFlagGetObjectTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("BitFlag::GetObject", 1, delegate
            {
                Test19 test19 = new Test19 { Flag = new BitFlag(), ProtocolId = 1, ServiceId = 2 };
                test19.Flag[0] = true;
                test19.Flag[2] = true;
                test19.Bind();
                Assert.IsTrue(test19.IsBind);
                Assert.IsTrue(test19.Body.Length == 16);
                PrintBytes(test19.Body);

                Test19 newObj = IntellectObjectEngine.GetObject<Test19>(test19.Body);
                Assert.IsNotNull(newObj);
                Assert.IsTrue(newObj.ProtocolId == 1);
                Assert.IsTrue(newObj.ServiceId == 2);
                Assert.IsTrue(BitFlagTest.ConvertToByte(newObj.Flag) == 0x05);
            });
        }

        [TestMethod]
        [Description("IPEndPoint绑定一般性测试")]
        public void IPEndPointBindTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("IPEndPoint::ToBytes", 1, delegate
            {
                Test20 test20 = new Test20 { Iep = new IPEndPoint(IPAddress.Parse("192.160.110.1"), 55555), ProtocolId = 1, ServiceId = 2 };
                test20.Bind();
                Assert.IsTrue(test20.IsBind);
                Assert.IsTrue(test20.Body.Length == 27);
                PrintBytes(test20.Body);
            });
        }
        
        [TestMethod]
        [Description("IPEndPoint数组绑定一般性测试")]
        public void IPEndPointArrayBindTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("IPEndPoint Array::ToBytes", 1, delegate
            {
                Test66 test66 = new Test66 { Ieps = new[] { new IPEndPoint(IPAddress.Parse("192.160.110.1"), 55555), new IPEndPoint(IPAddress.Parse("192.160.110.1"), 55555) }, ProtocolId = 1, ServiceId = 2 };
                test66.Bind();
                Assert.IsTrue(test66.IsBind);
                Assert.IsTrue(test66.Body.Length == 47);
                PrintBytes(test66.Body);
            });
        }

        [TestMethod]
        [Description("IPEndPoint数组解析一般性测试")]
        public void IPEndPointArrayPickupTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("IPEndPoint Array::GetObject", 1, delegate
            {
                Test66 test66 = new Test66 { Ieps = new[] { new IPEndPoint(IPAddress.Parse("192.160.110.1"), 55555), new IPEndPoint(IPAddress.Parse("192.160.110.2"), 55556) }, ProtocolId = 1, ServiceId = 2 };
                test66.Bind();
                Assert.IsTrue(test66.IsBind);
                Assert.IsTrue(test66.Body.Length == 47);
                PrintBytes(test66.Body);

                Test66 newObj = IntellectObjectEngine.GetObject<Test66>(test66.Body);
                Assert.IsNotNull(newObj);
                Assert.IsTrue(newObj.ProtocolId == 1);
                Assert.IsTrue(newObj.ServiceId == 2);
                Assert.IsNotNull(newObj.Ieps);
                Assert.IsTrue(newObj.Ieps.Length == 2);
                Assert.IsTrue(newObj.Ieps[0].Address.ToString() == "192.160.110.1");
                Assert.IsTrue(newObj.Ieps[0].Port == 55555);
                Assert.IsTrue(newObj.Ieps[1].Address.ToString() == "192.160.110.2");
                Assert.IsTrue(newObj.Ieps[1].Port == 55556);
            });
        }

        [TestMethod]
        [Description("IPEndPoint解析一般性测试")]
        public void IPEndPointPickupTest()
        {
            CodeTimer.Initialize();
            CodeTimer.Time("IPEndPoint::ToBytes", 1, delegate
            {
                Test20 test20 = new Test20 { Iep = new IPEndPoint(IPAddress.Parse("192.160.110.1"), 55555), ProtocolId = 1, ServiceId = 2 };
                test20.Bind();
                Assert.IsTrue(test20.IsBind);
                Assert.IsTrue(test20.Body.Length == 27);
                PrintBytes(test20.Body);

                Test20 newObj = IntellectObjectEngine.GetObject<Test20>(test20.Body);
                Assert.IsNotNull(newObj);
                Assert.IsTrue(newObj.ProtocolId == 1);
                Assert.IsTrue(newObj.ServiceId == 2);
                Assert.IsNotNull(newObj.Iep);
                Assert.IsTrue(newObj.Iep.Address.Equals(IPAddress.Parse("192.160.110.1")));
                Assert.IsTrue(newObj.Iep.Port == 55555);
            });
        }

        [TestMethod]
        [Description("Guid的一般性绑定测试")]
        public void GuidBindTest()
        {
            Test21 test21 = new Test21 {ProtocolId = 1, ServiceId = 2, Identity = Guid.NewGuid()};
            test21.Bind();
            Assert.IsTrue(test21.IsBind);
            Assert.IsNotNull(test21.Body);
            Assert.IsTrue(test21.Body.Length == 31);
            PrintBytes(test21.Body);
        }

        [TestMethod]
        [Description("Guid的一般性解析测试")]
        public void GuidPickupTest()
        {
            Test21 test21 = new Test21 {ProtocolId = 1, ServiceId = 2, Identity = Guid.NewGuid()};
            test21.Bind();
            Assert.IsTrue(test21.IsBind);
            Assert.IsNotNull(test21.Body);
            Assert.IsTrue(test21.Body.Length == 31);
            PrintBytes(test21.Body);

            Test21 newObj = IntellectObjectEngine.GetObject<Test21>(test21.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(newObj.Identity == test21.Identity);
        }

        [TestMethod]
        [Description("Guid类型序列化压力测试")]
        public void GuidExtremeBindTest()
        {
            Test21 test21 = new Test21 { ProtocolId = 1, ServiceId = 2, Identity = Guid.NewGuid() };
            CodeTimer.Initialize();
            CodeTimer.Time("Guid extreme bind test: 1000000", 1000000, delegate
            {
                test21.Bind();
                Assert.IsTrue(test21.IsBind);
                Assert.IsNotNull(test21.Body);
                Assert.IsTrue(test21.Body.Length == 31);
            });
            PrintBytes(test21.Body);
        }

        [TestMethod]
        [Description("Guid类型反序列化压力测试")]
        public void GuidExtremePickupTest()
        {
            Test21 test21 = new Test21 { ProtocolId = 1, ServiceId = 2, Identity = Guid.NewGuid() };
            test21.Bind();
            Assert.IsTrue(test21.IsBind);
            Assert.IsNotNull(test21.Body);
            Assert.IsTrue(test21.Body.Length == 31);
            PrintBytes(test21.Body);
            CodeTimer.Initialize();
            CodeTimer.Time("Guid extreme pickup test: 1000000", 1000000, delegate
            {
                Test21 newObj = IntellectObjectEngine.GetObject<Test21>(test21.Body);
                Assert.IsNotNull(newObj);
                Assert.IsTrue(newObj.ProtocolId == 1);
                Assert.IsTrue(newObj.ServiceId == 2);
                Assert.IsTrue(newObj.Identity == test21.Identity);
            });
            PrintBytes(test21.Body);
        }

        [TestMethod]
        [Description("TimeSpan一般性绑定测试")]
        public void TimeSpanBindTest()
        {
            Test22 test22 = new Test22 { ProtocolId = 1, ServiceId = 2, Time = (DateTime.Now.AddMinutes(5) - DateTime.Now) };
            test22.Bind();
            Assert.IsTrue(test22.IsBind);
            Assert.IsNotNull(test22.Body);
            Assert.IsTrue(test22.Body.Length == 23);
            PrintBytes(test22.Body);
        }

        [TestMethod]
        [Description("TimeSpan一般性解析测试")]
        public void TimeSpanPickupTest()
        {
            Test22 test22 = new Test22 { ProtocolId = 1, ServiceId = 2, Time = (DateTime.Now.AddMinutes(5) - DateTime.Now) };
            test22.Bind();
            Assert.IsTrue(test22.IsBind);
            Assert.IsNotNull(test22.Body);
            Assert.IsTrue(test22.Body.Length == 23);
            PrintBytes(test22.Body);

            Test22 newObj = IntellectObjectEngine.GetObject<Test22>(test22.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsTrue(test22.Time.Equals(newObj.Time));
        }

        [TestMethod]
        [Description("对于标记了IsRequired属性字段的安全性测试")]
        public void SecurityBindCheckTest()
        {
            Test23 test23 = new Test23();
            try { test23.Bind(); }
            catch { }
            Assert.IsFalse(test23.IsBind);
            Assert.IsNull(test23.Body);
            test23.Name = "Would you like to know more?";
            test23.Bind();
            Assert.IsTrue(test23.IsBind);
            Assert.IsNotNull(test23.Body);
            PrintBytes(test23.Body);
            test23.Name = null;
            try { test23.Bind(); }
            catch { }
            Assert.IsFalse(test23.IsBind);
            Assert.IsNull(test23.Body);
        }

        [TestMethod]
        [Description("int类型数组的优化绑定测试")]
        public void IntArrayOptimizeBindTest()
        {
            Test24 test24 = new Test24 {Values = new[] {1, 2, 3, 4, 5}};
            test24.Bind();
            Assert.IsTrue(test24.IsBind);
            Assert.IsNotNull(test24.Body);
            Assert.IsTrue(test24.Body.Length == 33);
            PrintBytes(test24.Body);
            test24 = new Test24 {Values = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0}};
            test24.Bind();
            Assert.IsTrue(test24.IsBind);
            Assert.IsNotNull(test24.Body);
            Assert.IsTrue(test24.Body.Length == 53);
            PrintBytes(test24.Body);
            //parse test.
            Test24 newObj = IntellectObjectEngine.GetObject<Test24>(test24.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Values.Length == 10);
            Console.WriteLine(newObj.ToString());
        }

        [TestMethod]
        [Description("double类型数组的优化绑定测试")]
        public void DoubleArrayOptimizeBindTest()
        {
            Test29 test29 = new Test29 { Values = new double[] { 1, 2, 3, 4, 5 } };
            test29.Bind();
            Assert.IsTrue(test29.IsBind);
            Assert.IsNotNull(test29.Body);
            Assert.IsTrue(test29.Body.Length == 53);
            PrintBytes(test29.Body);
            test29 = new Test29 { Values = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 } };
            test29.Bind();
            Assert.IsTrue(test29.IsBind);
            Assert.IsNotNull(test29.Body);
            Assert.IsTrue(test29.Body.Length == 93);
            PrintBytes(test29.Body);
            //parse test.
            Test29 newObj = IntellectObjectEngine.GetObject<Test29>(test29.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Values.Length == 10);
            Console.WriteLine(newObj.ToString());
        }

        [TestMethod]
        [Description("float类型数组的优化绑定测试")]
        public void FloatArrayOptimizeBindTest()
        {
            Test26 test26 = new Test26 { Values = new float[] { 1, 2, 3, 4, 5 } };
            test26.Bind();
            Assert.IsTrue(test26.IsBind);
            Assert.IsNotNull(test26.Body);
            Assert.IsTrue(test26.Body.Length == 33);
            PrintBytes(test26.Body);
            test26 = new Test26 { Values = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 } };
            test26.Bind();
            Assert.IsTrue(test26.IsBind);
            Assert.IsNotNull(test26.Body);
            Assert.IsTrue(test26.Body.Length == 53);
            PrintBytes(test26.Body);
            //parse test.
            Test26 newObj = IntellectObjectEngine.GetObject<Test26>(test26.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Values.Length == 10);
            Console.WriteLine(newObj.ToString());
        }

        [TestMethod]
        [Description("long类型数组的优化绑定测试")]
        public void LongArrayOptimizeBindTest()
        {
            Test31 test31 = new Test31 { Values = new long[] { 1, 2, 3, 4, 5 } };
            test31.Bind();
            Assert.IsTrue(test31.IsBind);
            Assert.IsNotNull(test31.Body);
            Assert.IsTrue(test31.Body.Length == 53);
            PrintBytes(test31.Body);
            test31 = new Test31 { Values = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 } };
            test31.Bind();
            Assert.IsTrue(test31.IsBind);
            Assert.IsNotNull(test31.Body);
            Assert.IsTrue(test31.Body.Length == 93);
            PrintBytes(test31.Body);
            //parse test.
            Test31 newObj = IntellectObjectEngine.GetObject<Test31>(test31.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Values.Length == 10);
            Console.WriteLine(newObj.ToString());
        }

        [TestMethod]
        [Description("short类型数组的优化绑定测试")]
        public void ShortArrayOptimizeBindTest()
        {
            Test25 test25 = new Test25 { Values = new short[] { 1, 2, 3, 4, 5 } };
            test25.Bind();
            Assert.IsTrue(test25.IsBind);
            Assert.IsNotNull(test25.Body);
            Assert.IsTrue(test25.Body.Length == 23);
            PrintBytes(test25.Body);
            test25 = new Test25 { Values = new short[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 } };
            test25.Bind();
            Assert.IsTrue(test25.IsBind);
            Assert.IsNotNull(test25.Body);
            Assert.IsTrue(test25.Body.Length == 33);
            PrintBytes(test25.Body);
            //parse test.
            Test25 newObj = IntellectObjectEngine.GetObject<Test25>(test25.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Values.Length == 10);
            Console.WriteLine(newObj.ToString());
        }

        [TestMethod]
        [Description("byte类型数组的优化绑定测试")]
        public void ByteArrayOptimizeBindTest()
        {
            Test30 test30 = new Test30 {Values = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05}};
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            Assert.IsTrue(test30.Body.Length == 18);
            PrintBytes(test30.Body);
            test30 = new Test30();
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            test30 = new Test30 {Values = new byte[] {}};
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            Assert.IsTrue(test30.Body.Length == 13);
            PrintBytes(test30.Body);
            //parse test.
            Test30 newObj1 = IntellectObjectEngine.GetObject<Test30>(test30.Body);
            Assert.IsNotNull(newObj1);
            Assert.IsTrue(newObj1.Values.Length == 0);
            Console.WriteLine(newObj1.ToString());
            test30 = new Test30 {Values = new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A}};
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            Assert.IsTrue(test30.Body.Length == 23);
            PrintBytes(test30.Body);
            //parse test.
            Test30 newObj = IntellectObjectEngine.GetObject<Test30>(test30.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Values.Length == 10);
            Console.WriteLine(newObj.ToString());
        }

        [TestMethod]
        [Description("数组序列化优化方案研究测试")]
        public unsafe void ArrayOptimizeTotalTest()
        {
            int[] arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            byte[] data = new byte[arr.Length * 4];

            #region Method 1.

            CodeTimer.Initialize();
            CodeTimer.Time(string.Format("#element({0}) -> Marshal copy, count: 1", arr.Length), 1, delegate
            {
                fixed (int* pArr = arr)
                    Marshal.Copy((IntPtr)pArr, data, 0, data.Length);
            });
            CodeTimer.Time(string.Format("#element({0}) -> Marshal copy, count: 1000000", arr.Length), 1000000, delegate
            {
                fixed (int* pArr = arr)
                    Marshal.Copy((IntPtr)pArr, data, 0, data.Length);
            });

            #endregion

            #region Method 2.

            CodeTimer.Initialize();
            CodeTimer.Time(string.Format("#element({0}) -> Unmanaged copy, count: 1", arr.Length), 1, delegate
            {
                fixed (byte* pData = data)
                {
                    for (int i = 0; i < arr.Length; i++)
                        *(int*)(pData + i) = arr[i];
                }
            });
            CodeTimer.Time(string.Format("#element({0}) -> Unmanaged copy, count: 1000000", arr.Length), 1000000, delegate
            {
                fixed (byte* pData = data)
                {
                    for (int i = 0; i < arr.Length; i++)
                        *(int*)(pData + i) = arr[i];
                }
            });

            #endregion
        }

        [TestMethod]
        [Description("char数组序列化测试")]
        public void CharArrayBindTest()
        {
            Test32 test32 = new Test32 {Values = new[] {'a', 'b', 'c'}};
            test32.Bind();
            Assert.IsTrue(test32.IsBind);
            Assert.IsNotNull(test32.Body);
            Assert.IsTrue(test32.Body.Length == 19);
            PrintBytes(test32.Body);
            Test32 newObj = IntellectObjectEngine.GetObject<Test32>(test32.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 3);
            Assert.IsTrue(newObj.Values[0] == 'a');
            Assert.IsTrue(newObj.Values[1] == 'b');
            Assert.IsTrue(newObj.Values[2] == 'c');
            //empty array test.
            test32 = new Test32 { Values = new char[0] };
            test32.Bind();
            Assert.IsTrue(test32.IsBind);
            Assert.IsNotNull(test32.Body);
            Assert.IsTrue(test32.Body.Length == 13);
            PrintBytes(test32.Body);
            newObj = IntellectObjectEngine.GetObject<Test32>(test32.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            //large elements.
            string content = new string('*', 100);
            test32 = new Test32 {Values = content.ToCharArray()};
            test32.Bind();
            Assert.IsTrue(test32.IsBind);
            Assert.IsNotNull(test32.Body);
            Assert.IsTrue(test32.Body.Length == 213);
            PrintBytes(test32.Body);
            newObj = IntellectObjectEngine.GetObject<Test32>(test32.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
            Assert.IsTrue(new string(newObj.Values) == content);
        }

        [TestMethod]
        [Description("int32数组序列化测试")]
        public void Int32ArrayBindTest()
        {
            Test24 test24 = new Test24 {Values = new[] {1, 2, 3, 4, 5}};
            test24.Bind();
            Assert.IsTrue(test24.IsBind);
            Assert.IsNotNull(test24.Body);
            Assert.IsTrue(test24.Body.Length == 33);
            PrintBytes(test24.Body);
            Test24 newObj = IntellectObjectEngine.GetObject<Test24>(test24.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test24 = new Test24 { Values = new int[0] };
            test24.Bind();
            Assert.IsTrue(test24.IsBind);
            Assert.IsNotNull(test24.Body);
            Assert.IsTrue(test24.Body.Length == 13);
            PrintBytes(test24.Body);
            newObj = IntellectObjectEngine.GetObject<Test24>(test24.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test24 = new Test24();
            test24.Values = new int[100];
            for (int i = 0; i < 100; i++) test24.Values[i] = i;
            test24.Bind();
            Assert.IsTrue(test24.IsBind);
            Assert.IsNotNull(test24.Body);
            Assert.IsTrue(test24.Body.Length == 413);
            PrintBytes(test24.Body);
            newObj = IntellectObjectEngine.GetObject<Test24>(test24.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("bool数组序列化测试")]
        public void BoolArrayBindTest()
        {
            Test27 test27 = new Test27 {Values = new[] {true, true, true, true, true}};
            test27.Bind();
            Assert.IsTrue(test27.IsBind);
            Assert.IsNotNull(test27.Body);
            Assert.IsTrue(test27.Body.Length == 18);
            PrintBytes(test27.Body);
            Test27 newObj = IntellectObjectEngine.GetObject<Test27>(test27.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0]);
            Assert.IsTrue(newObj.Values[1]);
            Assert.IsTrue(newObj.Values[2]);
            Assert.IsTrue(newObj.Values[3]);
            Assert.IsTrue(newObj.Values[4]);
            //empty array test.
            test27 = new Test27 { Values = new bool[0] };
            test27.Bind();
            Assert.IsTrue(test27.IsBind);
            Assert.IsNotNull(test27.Body);
            Assert.IsTrue(test27.Body.Length == 13);
            PrintBytes(test27.Body);
            newObj = IntellectObjectEngine.GetObject<Test27>(test27.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test27 = new Test27();
            test27.Values = new bool[100];
            for (int i = 0; i < 100; i++) test27.Values[i] = true;
            test27.Bind();
            Assert.IsTrue(test27.IsBind);
            Assert.IsNotNull(test27.Body);
            Assert.IsTrue(test27.Body.Length == 113);
            PrintBytes(test27.Body);
            newObj = IntellectObjectEngine.GetObject<Test27>(test27.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("byte数组序列化测试")]
        public void ByteArrayBindTest()
        {
            Test30 test30 = new Test30 {Values = new byte[] {0x0a, 0x0a, 0x0a, 0x0a, 0x0a}};
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            Assert.IsTrue(test30.Body.Length == 18);
            PrintBytes(test30.Body);
            Test30 newObj = IntellectObjectEngine.GetObject<Test30>(test30.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 0x0a);
            Assert.IsTrue(newObj.Values[1] == 0x0a);
            Assert.IsTrue(newObj.Values[2] == 0x0a);
            Assert.IsTrue(newObj.Values[3] == 0x0a);
            Assert.IsTrue(newObj.Values[4] == 0x0a);
            //empty array test.
            test30 = new Test30 { Values = new byte[0] };
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            Assert.IsTrue(test30.Body.Length == 13);
            PrintBytes(test30.Body);
            newObj = IntellectObjectEngine.GetObject<Test30>(test30.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test30 = new Test30();
            test30.Values = new byte[100];
            for (int i = 0; i < 100; i++) test30.Values[i] =  0x0a;
            test30.Bind();
            Assert.IsTrue(test30.IsBind);
            Assert.IsNotNull(test30.Body);
            Assert.IsTrue(test30.Body.Length == 113);
            PrintBytes(test30.Body);
            newObj = IntellectObjectEngine.GetObject<Test30>(test30.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("double数组序列化测试")]
        public void DoubleArrayBindTest()
        {
            Test29 test29 = new Test29 {Values = new double[] {1, 2, 3, 4, 5}};
            test29.Bind();
            Assert.IsTrue(test29.IsBind);
            Assert.IsNotNull(test29.Body);
            Assert.IsTrue(test29.Body.Length == 53);
            PrintBytes(test29.Body);
            Test29 newObj = IntellectObjectEngine.GetObject<Test29>(test29.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test29 = new Test29 { Values = new double[0] };
            test29.Bind();
            Assert.IsTrue(test29.IsBind);
            Assert.IsNotNull(test29.Body);
            Assert.IsTrue(test29.Body.Length == 13);
            PrintBytes(test29.Body);
            newObj = IntellectObjectEngine.GetObject<Test29>(test29.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test29 = new Test29();
            test29.Values = new double[100];
            for (int i = 0; i < 100; i++) test29.Values[i] = 5L;
            test29.Bind();
            Assert.IsTrue(test29.IsBind);
            Assert.IsNotNull(test29.Body);
            Assert.IsTrue(test29.Body.Length == 813);
            PrintBytes(test29.Body);
            newObj = IntellectObjectEngine.GetObject<Test29>(test29.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("float数组序列化测试")]
        public void FloatArrayBindTest()
        {
            Test26 test26 = new Test26 { Values = new float[] { 1, 2, 3, 4, 5 } };
            test26.Bind();
            Assert.IsTrue(test26.IsBind);
            Assert.IsNotNull(test26.Body);
            Assert.IsTrue(test26.Body.Length == 33);
            PrintBytes(test26.Body);
            Test26 newObj = IntellectObjectEngine.GetObject<Test26>(test26.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test26 = new Test26 {Values = new float[0]};
            test26.Bind();
            Assert.IsTrue(test26.IsBind);
            Assert.IsNotNull(test26.Body);
            Assert.IsTrue(test26.Body.Length == 13);
            PrintBytes(test26.Body);
            newObj = IntellectObjectEngine.GetObject<Test26>(test26.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test26 = new Test26();
            test26.Values = new float[100];
            for (int i = 0; i < 100; i++) test26.Values[i] = 5;
            test26.Bind();
            Assert.IsTrue(test26.IsBind);
            Assert.IsNotNull(test26.Body);
            Assert.IsTrue(test26.Body.Length == 413);
            PrintBytes(test26.Body);
            newObj = IntellectObjectEngine.GetObject<Test26>(test26.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("guid数组序列化测试")]
        public void GuidArrayBindTest()
        {
            Guid newGuid = Guid.NewGuid();
            Test28 test28 = new Test28 {Values = new[] {newGuid, newGuid, newGuid, newGuid, newGuid}};
            test28.Bind();
            Assert.IsTrue(test28.IsBind);
            Assert.IsNotNull(test28.Body);
            Assert.IsTrue(test28.Body.Length == 93);
            PrintBytes(test28.Body);
            Test28 newObj = IntellectObjectEngine.GetObject<Test28>(test28.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0].Equals(newGuid));
            Assert.IsTrue(newObj.Values[1].Equals(newGuid));
            Assert.IsTrue(newObj.Values[2].Equals(newGuid));
            Assert.IsTrue(newObj.Values[3].Equals(newGuid));
            Assert.IsTrue(newObj.Values[4].Equals(newGuid));
            //empty array test.
            test28 = new Test28 { Values = new Guid[0] };
            test28.Bind();
            Assert.IsTrue(test28.IsBind);
            Assert.IsNotNull(test28.Body);
            Assert.IsTrue(test28.Body.Length == 13);
            PrintBytes(test28.Body);
            newObj = IntellectObjectEngine.GetObject<Test28>(test28.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test28 = new Test28();
            test28.Values = new Guid[100];
            for (int i = 0; i < 100; i++) test28.Values[i] = newGuid;
            test28.Bind();
            Assert.IsTrue(test28.IsBind);
            Assert.IsNotNull(test28.Body);
            Assert.IsTrue(test28.Body.Length == 1613);
            PrintBytes(test28.Body);
            newObj = IntellectObjectEngine.GetObject<Test28>(test28.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("short数组序列化测试")]
        public void ShortArrayBindTest()
        {
            Test25 test25 = new Test25 {Values = new short[] {1, 2, 3, 4, 5}};
            test25.Bind();
            Assert.IsTrue(test25.IsBind);
            Assert.IsNotNull(test25.Body);
            Assert.IsTrue(test25.Body.Length == 23);
            PrintBytes(test25.Body);
            Test25 newObj = IntellectObjectEngine.GetObject<Test25>(test25.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test25 = new Test25 { Values = new short[0] };
            test25.Bind();
            Assert.IsTrue(test25.IsBind);
            Assert.IsNotNull(test25.Body);
            Assert.IsTrue(test25.Body.Length == 13);
            PrintBytes(test25.Body);
            newObj = IntellectObjectEngine.GetObject<Test25>(test25.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test25 = new Test25();
            test25.Values = new short[100];
            for (int i = 0; i < 100; i++) test25.Values[i] = 5;
            test25.Bind();
            Assert.IsTrue(test25.IsBind);
            Assert.IsNotNull(test25.Body);
            Assert.IsTrue(test25.Body.Length == 213);
            PrintBytes(test25.Body);
            newObj = IntellectObjectEngine.GetObject<Test25>(test25.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("long数组序列化测试")]
        public void LongArrayBindTest()
        {
            Test31 test31 = new Test31 { Values = new long[] { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 51, 2, 3, 4, 5 } };
            test31.Bind();
            Assert.IsTrue(test31.IsBind);
            Assert.IsNotNull(test31.Body);
            PrintBytes(test31.Body);
            Test31 newObj = IntellectObjectEngine.GetObject<Test31>(test31.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == test31.Values.Length);
            for (int i = 0; i < test31.Values.Length; i++)
                Assert.IsTrue(newObj.Values[i] == test31.Values[i]);
            //empty array test.
            test31 = new Test31 { Values = new long[0] };
            test31.Bind();
            Assert.IsTrue(test31.IsBind);
            Assert.IsNotNull(test31.Body);
            Assert.IsTrue(test31.Body.Length == 13);
            PrintBytes(test31.Body);
            newObj = IntellectObjectEngine.GetObject<Test31>(test31.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test31 = new Test31();
            test31.Values = new long[100];
            for (int i = 0; i < 100; i++) test31.Values[i] = 5;
            test31.Bind();
            Assert.IsTrue(test31.IsBind);
            Assert.IsNotNull(test31.Body);
            Assert.IsTrue(test31.Body.Length == 813);
            PrintBytes(test31.Body);
            newObj = IntellectObjectEngine.GetObject<Test31>(test31.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("datetime数组序列化测试")]
        public void DateTimeArrayBindTest()
        {
            DateTime now = DateTime.Now;
            Test33 test33 = new Test33 {Values = new[] {now, now, now, now, now}};
            test33.Bind();
            Assert.IsTrue(test33.IsBind);
            Assert.IsNotNull(test33.Body);
            Assert.IsTrue(test33.Body.Length == 53);
            PrintBytes(test33.Body);
            Test33 newObj = IntellectObjectEngine.GetObject<Test33>(test33.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == now);
            Assert.IsTrue(newObj.Values[1] == now);
            Assert.IsTrue(newObj.Values[2] == now);
            Assert.IsTrue(newObj.Values[3] == now);
            Assert.IsTrue(newObj.Values[4] == now);
            //empty array test.
            test33 = new Test33 { Values = new DateTime[0] };
            test33.Bind();
            Assert.IsTrue(test33.IsBind);
            Assert.IsNotNull(test33.Body);
            Assert.IsTrue(test33.Body.Length == 13);
            PrintBytes(test33.Body);
            newObj = IntellectObjectEngine.GetObject<Test33>(test33.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test33 = new Test33();
            test33.Values = new DateTime[100];
            for (int i = 0; i < 100; i++) test33.Values[i] = now;
            test33.Bind();
            Assert.IsTrue(test33.IsBind);
            Assert.IsNotNull(test33.Body);
            Assert.IsTrue(test33.Body.Length == 813);
            PrintBytes(test33.Body);
            newObj = IntellectObjectEngine.GetObject<Test33>(test33.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("decimal数组序列化测试")]
        public void DecimalArrayBindTest()
        {
            Test34 test34 = new Test34 {Values = new decimal[] {1, 2, 3, 4, 5}};
            test34.Bind();
            Assert.IsTrue(test34.IsBind);
            Assert.IsNotNull(test34.Body);
            Assert.IsTrue(test34.Body.Length == 93);
            PrintBytes(test34.Body);
            Test34 newObj = IntellectObjectEngine.GetObject<Test34>(test34.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test34 = new Test34 { Values = new decimal[0] };
            test34.Bind();
            Assert.IsTrue(test34.IsBind);
            Assert.IsNotNull(test34.Body);
            Assert.IsTrue(test34.Body.Length == 13);
            PrintBytes(test34.Body);
            newObj = IntellectObjectEngine.GetObject<Test34>(test34.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test34 = new Test34();
            test34.Values = new decimal[100];
            for (int i = 0; i < 100; i++) test34.Values[i] = 6;
            test34.Bind();
            Assert.IsTrue(test34.IsBind);
            Assert.IsNotNull(test34.Body);
            Assert.IsTrue(test34.Body.Length == 1613);
            PrintBytes(test34.Body);
            newObj = IntellectObjectEngine.GetObject<Test34>(test34.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("intptr数组序列化测试")]
        public void IntPtrArrayBindTest()
        {
            Test35 test35 = new Test35 { Values = new[] { new IntPtr(1), new IntPtr(1), new IntPtr(1), new IntPtr(1), new IntPtr(1) } };
            test35.Bind();
            Assert.IsTrue(test35.IsBind);
            Assert.IsNotNull(test35.Body);
            Assert.IsTrue(test35.Body.Length == 33);
            PrintBytes(test35.Body);
            Test35 newObj = IntellectObjectEngine.GetObject<Test35>(test35.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == new IntPtr(1));
            Assert.IsTrue(newObj.Values[1] == new IntPtr(1));
            Assert.IsTrue(newObj.Values[2] == new IntPtr(1));
            Assert.IsTrue(newObj.Values[3] == new IntPtr(1));
            Assert.IsTrue(newObj.Values[4] == new IntPtr(1));
            //empty array test.
            test35 = new Test35 { Values = new IntPtr[0] };
            test35.Bind();
            Assert.IsTrue(test35.IsBind);
            Assert.IsNotNull(test35.Body);
            Assert.IsTrue(test35.Body.Length == 13);
            PrintBytes(test35.Body);
            newObj = IntellectObjectEngine.GetObject<Test35>(test35.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test35 = new Test35();
            test35.Values = new IntPtr[100];
            for (int i = 0; i < 100; i++) test35.Values[i] = new IntPtr(1);
            test35.Bind();
            Assert.IsTrue(test35.IsBind);
            Assert.IsNotNull(test35.Body);
            Assert.IsTrue(test35.Body.Length == 413);
            PrintBytes(test35.Body);
            newObj = IntellectObjectEngine.GetObject<Test35>(test35.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("sbyte数组序列化测试")]
        public void SByteArrayBindTest()
        {
            Test36 test36 = new Test36 {Values = new sbyte[] {1, 2, 3, 4, 5}};
            test36.Bind();
            Assert.IsTrue(test36.IsBind);
            Assert.IsNotNull(test36.Body);
            Assert.IsTrue(test36.Body.Length == 18);
            PrintBytes(test36.Body);
            Test36 newObj = IntellectObjectEngine.GetObject<Test36>(test36.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test36 = new Test36 { Values = new sbyte[0] };
            test36.Bind();
            Assert.IsTrue(test36.IsBind);
            Assert.IsNotNull(test36.Body);
            Assert.IsTrue(test36.Body.Length == 13);
            PrintBytes(test36.Body);
            newObj = IntellectObjectEngine.GetObject<Test36>(test36.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test36 = new Test36();
            test36.Values = new sbyte[100];
            for (int i = 0; i < 100; i++) test36.Values[i] = 1;
            test36.Bind();
            Assert.IsTrue(test36.IsBind);
            Assert.IsNotNull(test36.Body);
            Assert.IsTrue(test36.Body.Length == 113);
            PrintBytes(test36.Body);
            newObj = IntellectObjectEngine.GetObject<Test36>(test36.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("timespan数组序列化测试")]
        public void TimeSpanArrayBindTest()
        {
            TimeSpan span = new TimeSpan(0, 1, 0);
            Test37 test37 = new Test37 {Values = new[] {span, span, span, span, span}};
            test37.Bind();
            Assert.IsTrue(test37.IsBind);
            Assert.IsNotNull(test37.Body);
            Assert.IsTrue(test37.Body.Length == 53);
            PrintBytes(test37.Body);
            Test37 newObj = IntellectObjectEngine.GetObject<Test37>(test37.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == span);
            Assert.IsTrue(newObj.Values[1] == span);
            Assert.IsTrue(newObj.Values[2] == span);
            Assert.IsTrue(newObj.Values[3] == span);
            Assert.IsTrue(newObj.Values[4] == span);
            //empty array test.
            test37 = new Test37 { Values = new TimeSpan[0] };
            test37.Bind();
            Assert.IsTrue(test37.IsBind);
            Assert.IsNotNull(test37.Body);
            Assert.IsTrue(test37.Body.Length == 13);
            PrintBytes(test37.Body);
            newObj = IntellectObjectEngine.GetObject<Test37>(test37.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test37 = new Test37();
            test37.Values = new TimeSpan[100];
            for (int i = 0; i < 100; i++) test37.Values[i] = span;
            test37.Bind();
            Assert.IsTrue(test37.IsBind);
            Assert.IsNotNull(test37.Body);
            Assert.IsTrue(test37.Body.Length == 813);
            PrintBytes(test37.Body);
            newObj = IntellectObjectEngine.GetObject<Test37>(test37.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("ushort数组序列化测试")]
        public void UShortArrayBindTest()
        {
            Test38 test38 = new Test38 {Values = new ushort[] {1, 2, 3, 4, 5}};
            test38.Bind();
            Assert.IsTrue(test38.IsBind);
            Assert.IsNotNull(test38.Body);
            Assert.IsTrue(test38.Body.Length == 23);
            PrintBytes(test38.Body);
            Test38 newObj = IntellectObjectEngine.GetObject<Test38>(test38.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test38 = new Test38 { Values = new ushort[0] };
            test38.Bind();
            Assert.IsTrue(test38.IsBind);
            Assert.IsNotNull(test38.Body);
            Assert.IsTrue(test38.Body.Length == 13);
            PrintBytes(test38.Body);
            newObj = IntellectObjectEngine.GetObject<Test38>(test38.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test38 = new Test38();
            test38.Values = new ushort[100];
            for (int i = 0; i < 100; i++) test38.Values[i] = 1;
            test38.Bind();
            Assert.IsTrue(test38.IsBind);
            Assert.IsNotNull(test38.Body);
            Assert.IsTrue(test38.Body.Length == 213);
            PrintBytes(test38.Body);
            newObj = IntellectObjectEngine.GetObject<Test38>(test38.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("uint数组序列化测试")]
        public void UIntArrayBindTest()
        {
            Test39 test39 = new Test39 { Values = new uint[] { 1, 2, 3, 4, 5 } };
            test39.Bind();
            Assert.IsTrue(test39.IsBind);
            Assert.IsNotNull(test39.Body);
            Assert.IsTrue(test39.Body.Length == 33);
            PrintBytes(test39.Body);
            Test39 newObj = IntellectObjectEngine.GetObject<Test39>(test39.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test39 = new Test39 { Values = new uint[0] };
            test39.Bind();
            Assert.IsTrue(test39.IsBind);
            Assert.IsNotNull(test39.Body);
            Assert.IsTrue(test39.Body.Length == 13);
            PrintBytes(test39.Body);
            newObj = IntellectObjectEngine.GetObject<Test39>(test39.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test39 = new Test39();
            test39.Values = new uint[100];
            for (int i = 0; i < 100; i++) test39.Values[i] = 1;
            test39.Bind();
            Assert.IsTrue(test39.IsBind);
            Assert.IsNotNull(test39.Body);
            Assert.IsTrue(test39.Body.Length == 413);
            PrintBytes(test39.Body);
            newObj = IntellectObjectEngine.GetObject<Test39>(test39.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("ulong数组序列化测试")]
        public void ULongArrayBindTest()
        {
            Test40 test40 = new Test40 {Values = new ulong[] {1, 2, 3, 4, 5}};
            test40.Bind();
            Assert.IsTrue(test40.IsBind);
            Assert.IsNotNull(test40.Body);
            Assert.IsTrue(test40.Body.Length == 53);
            PrintBytes(test40.Body);
            Test40 newObj = IntellectObjectEngine.GetObject<Test40>(test40.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 5);
            Assert.IsTrue(newObj.Values[0] == 1);
            Assert.IsTrue(newObj.Values[1] == 2);
            Assert.IsTrue(newObj.Values[2] == 3);
            Assert.IsTrue(newObj.Values[3] == 4);
            Assert.IsTrue(newObj.Values[4] == 5);
            //empty array test.
            test40 = new Test40 { Values = new ulong[0] };
            test40.Bind();
            Assert.IsTrue(test40.IsBind);
            Assert.IsNotNull(test40.Body);
            Assert.IsTrue(test40.Body.Length == 13);
            PrintBytes(test40.Body);
            newObj = IntellectObjectEngine.GetObject<Test40>(test40.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 0);
            test40 = new Test40();
            test40.Values = new ulong[100];
            for (int i = 0; i < 100; i++) test40.Values[i] = 1;
            test40.Bind();
            Assert.IsTrue(test40.IsBind);
            Assert.IsNotNull(test40.Body);
            Assert.IsTrue(test40.Body.Length == 813);
            PrintBytes(test40.Body);
            newObj = IntellectObjectEngine.GetObject<Test40>(test40.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 100);
        }

        [TestMethod]
        [Description("字符串数组一般性测试")]
        public void StringArrayNormalTest()
        {
            Test41 test41 = new Test41();
            test41.Bind();
            Assert.IsTrue(test41.IsBind);
            Assert.IsNotNull(test41.Body);
            Assert.IsTrue(test41.Body.Length == 4);
            PrintBytes(test41.Body);
            test41 = new Test41 {Values = new string[3]};
            test41.Bind();
            Assert.IsTrue(test41.IsBind);
            Assert.IsNotNull(test41.Body);
            Assert.IsTrue(test41.Body.Length == 19);
            PrintBytes(test41.Body);
            Test41 newObj = IntellectObjectEngine.GetObject<Test41>(test41.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 3);
            Assert.IsTrue(newObj.Values[0] == null);
            Assert.IsTrue(newObj.Values[1] == null);
            Assert.IsTrue(newObj.Values[2] == null);
            test41 = new Test41 {Values = new[] {"kevin", null, "jee"}};
            test41.Bind();
            Assert.IsTrue(test41.IsBind);
            Assert.IsNotNull(test41.Body);
            Assert.IsTrue(test41.Body.Length == 27);
            PrintBytes(test41.Body);
            newObj = IntellectObjectEngine.GetObject<Test41>(test41.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 3);
            Assert.IsTrue(newObj.Values[0] == "kevin");
            Assert.IsTrue(newObj.Values[1] == null);
            Assert.IsTrue(newObj.Values[2] == "jee");
            test41 = new Test41 { Values = new[] { null, null, "jee" } };
            test41.Bind();
            Assert.IsTrue(test41.IsBind);
            Assert.IsNotNull(test41.Body);
            Assert.IsTrue(test41.Body.Length == 22);
            PrintBytes(test41.Body);
            newObj = IntellectObjectEngine.GetObject<Test41>(test41.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 3);
            Assert.IsTrue(newObj.Values[0] == null);
            Assert.IsTrue(newObj.Values[1] == null);
            Assert.IsTrue(newObj.Values[2] == "jee");
            test41 = new Test41 { Values = new[] { "kevin", null, null } };
            test41.Bind();
            Assert.IsTrue(test41.IsBind);
            Assert.IsNotNull(test41.Body);
            Assert.IsTrue(test41.Body.Length == 24);
            PrintBytes(test41.Body);
            newObj = IntellectObjectEngine.GetObject<Test41>(test41.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == 3);
            Assert.IsTrue(newObj.Values[0] == "kevin");
            Assert.IsTrue(newObj.Values[1] == null);
            Assert.IsTrue(newObj.Values[2] == null);
        }

        [TestMethod]
        [Description("内部含有的智能对象为空序列化测试")]
        public void IntellectObjNullBindTest()
        {
            Test5 test5 = new Test5 {ProtocolId = 1, ServicelId = 2};
            test5.Bind();
            Assert.IsTrue(test5.IsBind);
            Assert.IsNotNull(test5.Body);
            Assert.IsTrue(test5.Body.Length == 14);

            Test5 newObj = IntellectObjectEngine.GetObject<Test5>(test5.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServicelId == 2);
            Assert.IsNull(newObj.InnerObject);
        }

        [TestMethod]
        [Description("综合性int32数组测试")]
        public void ComplexIntArrayTest()
        {
            Test42 c = new Test42();
            c.Values = new[]
                {
                    130,
                    11,
                    167,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    187,
                    50,
                    51,
                    53,
                    54,
                    55,
                    56,
                    58,
                    59,
                    60,
                    61,
                    63,
                    64,
                    65,
                    66,
                    67,
                    68,
                    69,
                    70,
                    71,
                    72,
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79,
                    80,
                    57,
                    130,
                    11,
                    167,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    187,
                    50,
                    51,
                    53,
                    54,
                    55,
                    56,
                    58,
                    59,
                    60,
                    61,
                    63,
                    64,
                    65,
                    66,
                    67,
                    68,
                    69,
                    70,
                    71,
                    72,
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79,
                    80,
                    57,
                    82,
                    83,
                    84,
                    85,
                    86,
                    87,
                    88,
                    89,
                    90,
                    91,
                    92,
                    130,
                    11,
                    167,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    187,
                    50,
                    51,
                    53,
                    54,
                    55,
                    56,
                    58,
                    59,
                    60,
                    61,
                    63,
                    64,
                    65,
                    66,
                    67,
                    68,
                    69,
                    70,
                    71,
                    72,
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79,
                    80,
                    57,
                    130,
                    11,
                    167,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    187,
                    50,
                    51,
                    53,
                    54,
                    55,
                    56,
                    58,
                    59,
                    60,
                    61,
                    63,
                    64,
                    65,
                    66,
                    67,
                    68,
                    69,
                    70,
                    71,
                    72,
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79,
                    80,
                    57,
                    82,
                    83,
                    84,
                    85,
                    86,
                    87,
                    88,
                    89,
                    90,
                    91,
                    92,
                    130,
                    11,
                    167,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    187,
                    50,
                    51,
                    53,
                    54,
                    55,
                    56,
                    58,
                    59,
                    60,
                    61,
                    63,
                    64,
                    65,
                    66,
                    67,
                    68,
                    69,
                    70,
                    71,
                    72,
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79,
                    80,
                    57,
                    130,
                    11,
                    167,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    187,
                    50,
                    51,
                    53,
                    54,
                    55,
                    56,
                    58,
                    59,
                    60,
                    61,
                    63,
                    64,
                    65,
                    66,
                    67,
                    68,
                    69,
                    70,
                    71,
                    72,
                    73,
                    74,
                    75,
                    76,
                    77,
                    78,
                    79,
                    80,
                    57,
                    82,
                    83,
                    84,
                    85,
                    86,
                    87,
                    88,
                    89,
                    90,
                    91,
                    92
                };
            c.Bind();
            Test42 newObj = IntellectObjectEngine.GetObject<Test42>(c.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Values);
            Assert.IsTrue(newObj.Values.Length == c.Values.Length);
            Console.WriteLine(newObj);
            for (int i = 0; i < newObj.Values.Length; i++)
                Assert.IsTrue(newObj.Values[i] == c.Values[i]);
        }

        [TestMethod]
        [Description("综合性string测试")]
        public void ComplexStringTest()
        {
            Test2 test2 = new Test2 {Name = (new string('*', 5000) + new string('&', 5000))};
            test2.Bind();
            Assert.IsTrue(test2.IsBind);
            Assert.IsNotNull(test2.Body);
            Test2 newObj = IntellectObjectEngine.GetObject<Test2>(test2.Body);
            Assert.IsNotNull(newObj);
            Assert.IsNotNull(newObj.Name);
            Console.WriteLine(newObj);
            Assert.IsTrue(test2.Name == newObj.Name);
        }

        [TestMethod]
        [Description("兼容性测试-允许")]
        public void CompatibleModeTest()
        {
            CMode2 mode2 = new CMode2 {X = 1, Y = 2, Z = 3};
            mode2.Bind();
            Assert.IsTrue(mode2.IsBind);
            Assert.IsNotNull(mode2.Body);
            PrintBytes(mode2.Body);
            CMode mode = IntellectObjectEngine.GetObject<CMode>(mode2.Body);
            Assert.IsNotNull(mode);
            Assert.IsTrue(mode.IsPickup);
            Assert.IsTrue(mode.CompatibleMode);
            Console.WriteLine(mode);
        }

        [TestMethod]
        [Description("兼容性测试-不允许")]
        public void CompatibleModeTest2()
        {
            MemoryAllotter.AllowCompatibleMode = false;
            CMode2 mode2 = new CMode2 { X = 1, Y = 2, Z = 3 };
            mode2.Bind();
            Assert.IsTrue(mode2.IsBind);
            Assert.IsNotNull(mode2.Body);
            PrintBytes(mode2.Body);
            System.Exception e = null;
            try { IntellectObjectEngine.GetObject<CMode>(mode2.Body); }
            catch (System.Exception ex) { e = ex; }
            Assert.IsNotNull(e);
            Console.WriteLine(e.Message);
        }

        [TestMethod]
        [Description("无意义的字段Attribute值测试")]
        public void NoMeaningTest1()
        {
            Test44 test44 = new Test44();
            test44.Bind();
            Assert.IsFalse(test44.IsBind);
        }

        [TestMethod]
        [Description("无意义的字段Attribute值测试")]
        public void NoMeaningTest2()
        {
            Test46 test44 = new Test46();
            test44.Bind();
            Assert.IsFalse(test44.IsBind);
        }

        [TestMethod]
        [Description("Int32类型默认值不参与传输测试")]
        public void DefaultNull_Int32_Test()
        {
            Test45 test45 = new Test45 {X = 3};
            test45.Bind();
            Assert.IsTrue(test45.IsBind);
            Assert.IsTrue(test45.Body.Length == 9);
            PrintBytes(test45.Body);
           
            Test45 newObj = IntellectObjectEngine.GetObject<Test45>(test45.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("UInt32类型默认值不参与传输测试")]
        public void DefaultNull_UInt32_Test()
        {
            Test47 test47 = new Test47 { X = 3 };
            test47.Bind();
            Assert.IsTrue(test47.IsBind);
            Assert.IsTrue(test47.Body.Length == 9);
            PrintBytes(test47.Body);

            Test47 newObj = IntellectObjectEngine.GetObject<Test47>(test47.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Int16类型默认值不参与传输测试")]
        public void DefaultNull_Int16_Test()
        {
            Test48 test48 = new Test48 { X = 3 };
            test48.Bind();
            Assert.IsTrue(test48.IsBind);
            Assert.IsTrue(test48.Body.Length == 9);
            PrintBytes(test48.Body);

            Test48 newObj = IntellectObjectEngine.GetObject<Test48>(test48.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("UInt16类型默认值不参与传输测试")]
        public void DefaultNull_UInt16_Test()
        {
            Test49 test49 = new Test49 { X = 3 };
            test49.Bind();
            Assert.IsTrue(test49.IsBind);
            Assert.IsTrue(test49.Body.Length == 9);
            PrintBytes(test49.Body);

            Test49 newObj = IntellectObjectEngine.GetObject<Test49>(test49.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Guid类型默认值不参与传输测试")]
        public void DefaultNull_Guid_Test()
        {
            Test50 test50 = new Test50 { X = 3 };
            test50.Bind();
            Assert.IsTrue(test50.IsBind);
            Assert.IsTrue(test50.Body.Length == 9);
            PrintBytes(test50.Body);

            Test50 newObj = IntellectObjectEngine.GetObject<Test50>(test50.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == Guid.Empty);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("DateTime类型默认值不参与传输测试")]
        public void DefaultNull_DateTime_Test()
        {
            Test51 test51 = new Test51 { X = 3 };
            test51.Bind();
            Assert.IsTrue(test51.IsBind);
            Assert.IsTrue(test51.Body.Length == 9);
            PrintBytes(test51.Body);

            Test51 newObj = IntellectObjectEngine.GetObject<Test51>(test51.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == DateTime.MinValue);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("TimeSpan类型默认值不参与传输测试")]
        public void DefaultNull_TimeSpan_Test()
        {
            Test52 test52 = new Test52 { X = 3 };
            test52.Bind();
            Assert.IsTrue(test52.IsBind);
            Assert.IsTrue(test52.Body.Length == 9);
            PrintBytes(test52.Body);

            Test52 newObj = IntellectObjectEngine.GetObject<Test52>(test52.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == DefaultValue.TimeSpan);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("IntPtr类型默认值不参与传输测试")]
        public void DefaultNull_IntPtr_Test()
        {
            Test53 test53 = new Test53 { X = 3 };
            test53.Bind();
            Assert.IsTrue(test53.IsBind);
            Assert.IsTrue(test53.Body.Length == 9);
            PrintBytes(test53.Body);

            Test53 newObj = IntellectObjectEngine.GetObject<Test53>(test53.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == IntPtr.Zero);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Double类型默认值不参与传输测试")]
        public void DefaultNull_Double_Test()
        {
            Test54 test54 = new Test54 { X = 3 };
            test54.Bind();
            Assert.IsTrue(test54.IsBind);
            Assert.IsTrue(test54.Body.Length == 9);
            PrintBytes(test54.Body);

            Test54 newObj = IntellectObjectEngine.GetObject<Test54>(test54.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0D);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Float类型默认值不参与传输测试")]
        public void DefaultNull_Float_Test()
        {
            Test55 test55 = new Test55 { X = 3 };
            test55.Bind();
            Assert.IsTrue(test55.IsBind);
            Assert.IsTrue(test55.Body.Length == 9);
            PrintBytes(test55.Body);

            Test55 newObj = IntellectObjectEngine.GetObject<Test55>(test55.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0F);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Long类型默认值不参与传输测试")]
        public void DefaultNull_Long_Test()
        {
            Test56 test56 = new Test56 { X = 3 };
            test56.Bind();
            Assert.IsTrue(test56.IsBind);
            Assert.IsTrue(test56.Body.Length == 9);
            PrintBytes(test56.Body);

            Test56 newObj = IntellectObjectEngine.GetObject<Test56>(test56.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0L);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("ULong类型默认值不参与传输测试")]
        public void DefaultNull_ULong_Test()
        {
            Test57 test57 = new Test57 { X = 3 };
            test57.Bind();
            Assert.IsTrue(test57.IsBind);
            Assert.IsTrue(test57.Body.Length == 9);
            PrintBytes(test57.Body);

            Test57 newObj = IntellectObjectEngine.GetObject<Test57>(test57.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0UL);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Decimal类型默认值不参与传输测试")]
        public void DefaultNull_Decimal_Test()
        {
            Test58 test58 = new Test58 { X = 3 };
            test58.Bind();
            Assert.IsTrue(test58.IsBind);
            Assert.IsTrue(test58.Body.Length == 9);
            PrintBytes(test58.Body);

            Test58 newObj = IntellectObjectEngine.GetObject<Test58>(test58.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Char类型默认值不参与传输测试")]
        public void DefaultNull_Char_Test()
        {
            Test59 test59 = new Test59 { X = 3 };
            test59.Bind();
            Assert.IsTrue(test59.IsBind);
            Assert.IsTrue(test59.Body.Length == 9);
            PrintBytes(test59.Body);

            Test59 newObj = IntellectObjectEngine.GetObject<Test59>(test59.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == '\0');
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Boolean类型默认值不参与传输测试")]
        public void DefaultNull_Boolean_Test()
        {
            Test60 test60 = new Test60 { X = 3 };
            test60.Bind();
            Assert.IsTrue(test60.IsBind);
            Assert.IsTrue(test60.Body.Length == 9);
            PrintBytes(test60.Body);

            Test60 newObj = IntellectObjectEngine.GetObject<Test60>(test60.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsFalse(newObj.Y);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("Byte类型默认值不参与传输测试")]
        public void DefaultNull_Byte_Test()
        {
            Test61 test61 = new Test61 { X = 3 };
            test61.Bind();
            Assert.IsTrue(test61.IsBind);
            Assert.IsTrue(test61.Body.Length == 9);
            PrintBytes(test61.Body);

            Test61 newObj = IntellectObjectEngine.GetObject<Test61>(test61.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0x00);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("SByte类型默认值不参与传输测试")]
        public void DefaultNull_SByte_Test()
        {
            Test62 test62 = new Test62 { X = 3 };
            test62.Bind();
            Assert.IsTrue(test62.IsBind);
            Assert.IsTrue(test62.Body.Length == 9);
            PrintBytes(test62.Body);

            Test62 newObj = IntellectObjectEngine.GetObject<Test62>(test62.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0x00);
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("类型默认值不参与传输综合测试1")]
        public void DefaultNullTest1()
        {
            Test63 test63 = new Test63 {X = 3, Z = 6, M = new TimeSpan(1, 0, 0)};
            test63.Bind();
            Assert.IsTrue(test63.IsBind);
            Assert.IsTrue(test63.Body.Length == 23);
            PrintBytes(test63.Body);

            Test63 newObj = IntellectObjectEngine.GetObject<Test63>(test63.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.X == 3);
            Assert.IsTrue(newObj.Y == 0);
            Assert.IsTrue(newObj.Z == 6);
            Assert.IsTrue(newObj.N == Guid.Empty);
            Assert.IsTrue(newObj.M == new TimeSpan(1, 0, 0));
            Console.WriteLine(newObj);
        }

        [TestMethod]
        [Description("将一个定义为非公共开类转换为二进制数据测试")]
        public void BindUnpublicClassTest()
        {
            Test64 test64 = new Test64 {X = 3};
            System.Exception throwEx = null;
            try
            {
                test64.Bind();
            }
            catch (System.Exception ex)
            {
                throwEx = ex;
            }
            Assert.IsFalse(test64.IsBind);
            Assert.IsNotNull(throwEx);
            Console.WriteLine(throwEx.Message);
            Assert.IsTrue(throwEx is MethodAccessException);
        }

        [TestMethod]
        [Description("将一个定义为非公共开SET可序列化字段的类转换为二进制数据测试")]
        public void BindUnpublicPropertyTest()
        {
            Test65 test65 = new Test65("kevin");
            test65.Bind();
            Assert.IsTrue(test65.IsBind);
            PrintBytes(test65.Body);
        }

        [TestMethod]
        [Description("将一个定义为非公共开SET可序列化字段的类反序列化测试")]
        public void PickupUnpublicPropertyTest()
        {
            Test65 test65 = new Test65("kevin");
            test65.Bind();
            Assert.IsTrue(test65.IsBind);
            PrintBytes(test65.Body);

            Test65 newObj = IntellectObjectEngine.GetObject<Test65>(test65.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.Name == "kevin");
            Console.WriteLine(newObj);
        }
        
        [TestMethod]
        [Description("拥有字节数组集合的智能对象，打印描述信息的测试方法")]
        public void ByteArrayToStringTest()
        {
            TestObjectByteArray testObject = new TestObjectByteArray();
            testObject.Array = Encoding.Default.GetBytes("拥有222dfs~~~d象，打印描sdfsdfsdffsdf拥有字节数组集合的智能对象，打印描述信息的测试方法sdfewj90382-sjf lisf ssdf325yhrtgt htyj uy");
            Console.WriteLine(testObject);
        }

        [TestMethod]
        [Description("用于测试内部字段为智能对象数组形式的时候，依旧可以反序列化")]
        public void MultiIntellectObjectPickupTest()
        {
            Test67 test67 = new Test67 { ProtocolId = 1, ServiceId = 2, Objs = new[] { new Test1 { ProtocolId = 3, ServicelId = 4 }, new Test1 { ProtocolId = 3, ServicelId = 4 } } };
            test67.Bind();
            Assert.IsTrue(test67.IsBind);

            Test67 newObj = (Test67)IntellectObjectEngine.GetObject<Test67>(test67.Body);
            Assert.IsNotNull(newObj);
        }

        [TestMethod]
        [Description("拥有Blob的智能对象序列化测试方法")]
        public void BlobBindTest()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();

            #endregion
            Test68 test68 = new Test68 { ProtocolId = 1, ServiceId = 2, Blob = new Blob(CompressionTypes.GZip, data) };
            test68.Bind();
            Assert.IsTrue(test68.IsBind);
            Assert.IsNotNull(test68.Body);
            Console.WriteLine(test68);
            Console.WriteLine();
            PrintBytes(test68.Body);
        }

        [TestMethod]
        [Description("拥有空值Blob的智能对象序列化测试方法")]
        public void BlobNullValueBindTest()
        {
            Test68 test68 = new Test68 { ProtocolId = 1, ServiceId = 2 };
            test68.Bind();
            Assert.IsTrue(test68.IsBind);
            Assert.IsNotNull(test68.Body);
            Assert.IsTrue(test68.Body.Length == 14);
            Console.WriteLine(test68);
            Console.WriteLine();
            PrintBytes(test68.Body);
        }

        [TestMethod]
        [Description("拥有Blob的智能对象反序列化测试方法")]
        public void BlobPickupTest()
        {
            #region Prepare data from internet.

            WebRequest request = HttpWebRequest.Create("http://www.163.com");
            request.Method = "GET";
            Stream responseStream = request.GetResponse().GetResponseStream();
            byte[] data;
            using (StreamReader reader = new StreamReader(responseStream))
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            responseStream.Close();

            #endregion
            Test68 test68 = new Test68 { ProtocolId = 1, ServiceId = 2, Blob = new Blob(CompressionTypes.BZip2, data) };
            test68.Bind();
            Assert.IsTrue(test68.IsBind);
            Assert.IsNotNull(test68.Body);
            Console.WriteLine(test68);
            Console.WriteLine();

            Test68 newObj = IntellectObjectEngine.GetObject<Test68>(test68.Body);
            Assert.IsNotNull(newObj);
            Assert.IsTrue(newObj.ProtocolId == 1);
            Assert.IsTrue(newObj.ServiceId == 2);
            Assert.IsNotNull(newObj.Blob);
            Assert.IsTrue(newObj.Blob.CompressionType == CompressionTypes.BZip2);
            Console.WriteLine(newObj);
            byte[] newData = newObj.Blob.Decompress();
            Assert.IsNotNull(newData);
            Assert.IsTrue(newData.Length == data.Length);
            for (int i = 0; i < newData.Length; i++)
                Assert.IsTrue(newData[i] == data[i]);
            Console.WriteLine(newObj);
            Console.WriteLine();
        }

        [TestMethod]
        [Description("拥有OneField的智能对象反序列化测试方法")]
        public void OneFieldPickupTest()
        {
            OneFieldMessage msg = new OneFieldMessage { X = 10 };
            msg.Bind();
            OneFieldMessage ok = IntellectObjectEngine.GetObject<OneFieldMessage>(msg.Body);
            Assert.IsNotNull(ok);
        }


        public static void PrintBytes(byte[] data)
        {
            byte[] array = data;
            string spc = string.Empty;
            string nextSpace = spc + "  ";
            string nxtSpace = spc + "  ";
            int round = array.Length / 8 + (array.Length % 8 > 0 ? 1 : 0);
            int currentOffset, remainningLen;
            StringBuilder s = new StringBuilder();
            for (int j = 0; j < round; j++)
            {
                currentOffset = j * 8;
                remainningLen = ((array.Length - currentOffset) >= 8 ? 8 : (array.Length - currentOffset));
                StringBuilder rawByteBuilder = new StringBuilder();
                rawByteBuilder.Append(nextSpace);
                for (int k = 0; k < remainningLen; k++)
                {
                    rawByteBuilder.AppendFormat("0x{0}", array[currentOffset + k].ToString("X2"));
                    if (k != remainningLen - 1) rawByteBuilder.Append(", ");
                }
                rawByteBuilder.Append(new string(' ', (remainningLen == 8 ? 5 : (8 - remainningLen) * 4 + (((8 - remainningLen) - 1) * 2) + 7)));
                for (int k = 0; k < remainningLen; k++)
                {
                    if ((char)array[currentOffset + k] > 126 || (char)array[currentOffset + k] < 32) rawByteBuilder.Append('.');
                    else rawByteBuilder.Append((char)array[currentOffset + k]);
                }
                s.AppendLine(string.Format("{0}{1}", nxtSpace, rawByteBuilder));
            }
            Console.WriteLine(s.ToString());
        }
    }
}