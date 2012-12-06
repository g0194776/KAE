using System;
using System.Reflection;
using System.Threading;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.Data.Synchronization.Policies;
using KJFramework.EventArgs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Data.Synchronization.UnitTest
{
    [TestClass]
    public class DataPublisherTest
    {
        #region Methods

        /// <summary>
        ///     创建一个数据发布者的测试
        /// </summary>
        [TestMethod]
        public void CreateTest()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("CreateTest", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
        }

        /// <summary>
        ///     创建2个数据发布者，并且后一个发布者使用第一个数据发布者的CATALOG和网络资源
        /// </summary>
        [TestMethod]
        public void CreateTestUseSameResource()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("CreateTest_UseSameResource", new NetworkResource(9897));
            FieldInfo stubField = publisher.GetType().GetField("_stub", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            object stub = stubField.GetValue(publisher);
            PropertyInfo usecountProperty = stub.GetType().GetProperty("UseCount");
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "1");
            IDataPublisher<string, string> publisher1 = DataPublisherFactory.Instance.Create<string, string>("CreateTest_UseSameResource1", new NetworkResource(9897));
            Assert.IsNotNull(publisher1);
            stub = stubField.GetValue(publisher1);
            Assert.IsTrue(publisher1.Policy == PublisherPolicy.Default);
            //already existed.
            Assert.IsTrue(publisher1.State == PublisherState.Prepared);
            Assert.IsTrue(publisher1.Open() == PublisherState.Open);
            Assert.IsTrue(publisher1.State == PublisherState.Open);
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "2");
        }

        /// <summary>
        ///     创建并关闭一个数据发布者的测试
        /// </summary>
        [TestMethod]
        public void CloseTest()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("CloseTest", new NetworkResource(9899));
            FieldInfo stubField = publisher.GetType().GetField("_stub", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            object stub = stubField.GetValue(publisher);
            PropertyInfo usecountProperty = stub.GetType().GetProperty("UseCount");
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "1");
            publisher.Close();
            Assert.IsTrue(publisher.State == PublisherState.Close);
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "0");
        }

        /// <summary>
        ///     创建并关闭2个数据发布者，并且后一个发布者使用第一个数据发布者的CATALOG和网络资源
        /// </summary>
        [TestMethod]
        public void CloseTestUseSameResource()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("CloseTest_UseSameResource", new NetworkResource(9890));
            FieldInfo stubField = publisher.GetType().GetField("_stub", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            object stub = stubField.GetValue(publisher);
            PropertyInfo usecountProperty = stub.GetType().GetProperty("UseCount");
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "1");
            IDataPublisher<string, string> publisher1 = DataPublisherFactory.Instance.Create<string, string>("CloseTest_UseSameResource1", new NetworkResource(9890));
            Assert.IsNotNull(publisher1);
            stub = stubField.GetValue(publisher1);
            Assert.IsTrue(publisher1.Policy == PublisherPolicy.Default);
            //already existed.
            Assert.IsTrue(publisher1.State == PublisherState.Prepared);
            Assert.IsTrue(publisher1.Open() == PublisherState.Open);
            Assert.IsTrue(publisher1.State == PublisherState.Open);
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "2");
            publisher.Close();
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "1");
            Assert.IsTrue(publisher.State == PublisherState.Close);
            publisher1.Close();
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "0");
            Assert.IsTrue(publisher1.State == PublisherState.Close);
        }

        /// <summary>
        ///     创建并关闭2个数据发布者，并且后一个发布者使用第一个数据发布者的CATALOG和网络资源
        /// </summary>
        [TestMethod]
        public void CloseTestUseSameResource2()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("CloseTestUseSameResource_2", new NetworkResource(9890));
            FieldInfo stubField = publisher.GetType().GetField("_stub", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Console.WriteLine(publisher.State);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            object stub = stubField.GetValue(publisher);
            PropertyInfo usecountProperty = stub.GetType().GetProperty("UseCount");
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "1");
            IDataPublisher<string, string> publisher1 = DataPublisherFactory.Instance.Create<string, string>("CloseTestUseSameResource_3", new NetworkResource(9890));
            Assert.IsNotNull(publisher1);
            stub = stubField.GetValue(publisher1);
            Assert.IsTrue(publisher1.Policy == PublisherPolicy.Default);
            //already existed.
            Assert.IsTrue(publisher1.State == PublisherState.Prepared);
            Assert.IsTrue(publisher1.Open() == PublisherState.Open);
            Assert.IsTrue(publisher1.State == PublisherState.Open);
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "2");

            #region Create 1 subscriber.

            IRemoteDataSubscriber<string, string> subscriber = DataSubscriberFactory.Instance.Create<string, string>("CloseTestUseSameResource_2", new NetworkResource("127.0.0.1:9890"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            Assert.IsTrue(subscriber.Open() == SubscriberState.Subscribed);
            Assert.IsTrue(publisher.SubscriberCount == 1);
            Assert.IsTrue(publisher1.SubscriberCount == 0);

            #endregion

            publisher.Close();
            Assert.IsTrue(publisher.SubscriberCount == 0);
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "1");
            Assert.IsTrue(publisher.State == PublisherState.Close);
            publisher1.Close();
            Assert.IsTrue(usecountProperty.GetValue(stub, null).ToString() == "0");
            Assert.IsTrue(publisher1.State == PublisherState.Close);
        }

        /// <summary>
        ///     发布者发布数据测试
        /// </summary>
        [TestMethod]
        public void PublishTest()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("PublishTest", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            IRemoteDataSubscriber<string, string> subscriber = DataSubscriberFactory.Instance.Create<string, string>("PublishTest", new NetworkResource("127.0.0.1:9898"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            subscriber.Open();
            Assert.IsTrue(subscriber.State == SubscriberState.Subscribed);
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            subscriber.MessageRecv += delegate (object sender, LightSingleArgEventArgs<DataRecvEventArgs<string, string>> e)
            {
                Console.WriteLine("Key: " + e.Target.Key);
                Console.WriteLine("Value: " + e.Target.Value);
                resetEvent.Set();
            };
            publisher.Publish("K_user", "V_pass");
            if (!resetEvent.WaitOne(10000)) throw new System.Exception("Wait publish message timeout!");
            Thread.Sleep(5000);
        }

        #endregion
    }
}