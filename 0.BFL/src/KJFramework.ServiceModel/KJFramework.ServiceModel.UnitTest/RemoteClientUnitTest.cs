using System;
using System.Threading;
using ClientTest;
using KJFramework.ServiceModel.Bussiness.Default.Proxy;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Proxy;
using KJFramework.Timer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerTest.Contract;

namespace KJFramework.ServiceModel.UnitTest
{
    [TestClass]
    public class RemoteClientUnitTest
    {
        #region Methods

        [TestMethod]
        public void CallNormal3Args__HasReturn()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string hello = instance0.Channel.Hello(1, 2, "3");
            Console.WriteLine(hello);
            Assert.IsNotNull(hello);
        }

        [TestMethod]
        public void CallNormal3Args__HasReturn_NULLABLE()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string hello = instance0.Channel.Hello(1, 2, null);
            Console.WriteLine(hello);
            Assert.IsNotNull(hello);
        }

        [TestMethod]
        public void CallInArray_HasReturnArray()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            int[] hello = instance0.Channel.Hello(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0});
            Console.WriteLine(hello);
            Assert.IsNotNull(hello);
        }

        [TestMethod]
        public void CallInRefArray_HasReturnRefArray()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string[] hello = instance0.Channel.TestStrings(new[] {"kevin", "kline"}, "1", 2);
            Assert.IsNotNull(hello);
            Assert.IsTrue(hello.Length == 2);
            Assert.IsTrue(hello[0] == "aaaa");
            Assert.IsTrue(hello[1] == "bbbb");
            Console.WriteLine(hello);
            Assert.IsNotNull(hello);
        }

        [TestMethod]
        public void Ex_CallInRefArray_HasReturnRefArray()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string[] hello1 = instance0.Channel.TestStrings(new[] { "kevin", "kline" }, "1", 2);
            CodeTimer.Initialize();
            CodeTimer.Time("Ex_CallInRefArray_HasReturnRefArray : 10000", 10000, delegate
            {
                string[] hello = instance0.Channel.TestStrings(new[] { "kevin", "kline" }, "1", 2);
                Assert.IsNotNull(hello);
                Assert.IsTrue(hello.Length == 2);
                Assert.IsTrue(hello[0] == "aaaa");
                Assert.IsTrue(hello[1] == "bbbb");
                Assert.IsNotNull(hello);
            });
        }

        [TestMethod]
        public void CallInRefArray_HasReturnRefArray_NULLABLE()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string[] hello = instance0.Channel.TestStrings(new string[] { null, null }, "1", 2);
            Assert.IsNotNull(hello);
            Assert.IsTrue(hello.Length == 2);
            Assert.IsTrue(hello[0] == "aaaa");
            Assert.IsTrue(hello[1] == "bbbb");
            Console.WriteLine(hello);
            Assert.IsNotNull(hello);
        }

        [TestMethod]
        public void CallInRefArray_HasReturnRefArray_NULLABLE_V1()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string[] hello = instance0.Channel.TestStrings(null, null, 2);
            Assert.IsNotNull(hello);
            Assert.IsTrue(hello.Length == 2);
            Assert.IsTrue(hello[0] == "aaaa");
            Assert.IsTrue(hello[1] == "bbbb");
            Console.WriteLine(hello);
            Assert.IsNotNull(hello);
        }

        [TestMethod]
        public void CallIntellectObject()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            InObject arg = new InObject
                               {
                                   Inf1 = 1,
                                   Info = "kevin",
                                   Info2 = DateTime.Now,
                                   Info3 = new[] {"1", "2"},
                                   Info4 = new[] {1, 2}
                               };
            InObject hello = instance0.Channel.CallIntellectObject(arg);
            Assert.IsNotNull(hello);
            Assert.IsTrue(hello.Inf1 == arg.Inf1);
            Assert.IsTrue(hello.Info == arg.Info);
            Assert.IsNotNull(hello.Info3);
            Assert.IsNotNull(hello.Info4);
            Assert.IsTrue(hello.Info2 == arg.Info2);
            Assert.IsTrue(hello.Info3.Length == 2);
            Assert.IsTrue(hello.Info3[0] == arg.Info3[0]);
            Assert.IsTrue(hello.Info3[1] == arg.Info3[1]);
            Assert.IsTrue(hello.Info4.Length == 2);
            Assert.IsTrue(hello.Info4[0] == arg.Info4[0]);
            Assert.IsTrue(hello.Info4[1] == arg.Info4[1]);
            Assert.IsTrue(hello.Info5 == arg.Info5);
        }

        [TestMethod]
        public void CallIntellectObjectArray()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            InObject arg = new InObject
            {
                Inf1 = 1,
                Info = "kevin",
                Info2 = DateTime.Now,
                Info3 = new[] { "1", "2" },
                Info4 = new[] { 1, 2 }
            };
            InObject[] results = instance0.Channel.CallIntellectObjectArray(new[] {arg, arg});
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Length == 2);
            for (int i = 0; i < 2; i++)
            {
                Assert.IsNotNull(results[i]);
                Assert.IsTrue(results[i].Inf1 == arg.Inf1);
                Assert.IsTrue(results[i].Info == arg.Info);
                Assert.IsNotNull(results[i].Info3);
                Assert.IsNotNull(results[i].Info4);
                Assert.IsTrue(results[i].Info2 == arg.Info2);
                Assert.IsTrue(results[i].Info3.Length == 2);
                Assert.IsTrue(results[i].Info3[0] == arg.Info3[0]);
                Assert.IsTrue(results[i].Info3[1] == arg.Info3[1]);
                Assert.IsTrue(results[i].Info4.Length == 2);
                Assert.IsTrue(results[i].Info4[0] == arg.Info4[0]);
                Assert.IsTrue(results[i].Info4[1] == arg.Info4[1]);
                Assert.IsTrue(results[i].Info5 == arg.Info5);
            }
        }

        [TestMethod]
        public void CallReturnNull()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            string returnNull = instance0.Channel.CallReturnNull();
            Assert.IsNull(returnNull);
        }

        [TestMethod]
        public void CallOneway()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            instance0.Channel.CallOneway();
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void Call()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            instance0.Channel.Call();
        }
        
        [TestMethod]
        public void ThrowException()
        {
            IClientProxy<IHelloWorld> instance0 = new DefaultClientProxy<IHelloWorld>(new TcpBinding("tcp://192.168.20.124:9999/Test"));
            System.Exception ex = null;
            try { instance0.Channel.ThrowException(); }
            catch (System.Exception e) { ex = e; }
            Assert.IsNotNull(ex);
        }

        #endregion
    }
}