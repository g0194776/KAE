using System;
using System.Threading;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.Data.Synchronization.Policies;
using NUnit.Framework;

namespace KJFramework.Data.Synchronization.UnitTest
{
    public class RemoteDataSubscriberTest
    {
        #region Methods

        [SetUp]
        public void Initialize()
        {
            SyncDataFramework.Initialize();
        }

        /// <summary>
        ///     创建数据订阅者测试
        /// </summary>
        [Test]
        public void CreateTest()
        {
            IDataPublisher<string, string> publisher = DataPublisherFactory.Instance.Create<string, string>("CATELOG_TEST", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            IRemoteDataSubscriber<string, string> subscriber = DataSubscriberFactory.Instance.Create<string, string>("CATELOG_TEST", new NetworkResource("127.0.0.1:9898"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            subscriber.Open();
            Assert.IsTrue(subscriber.State == SubscriberState.Subscribed);
            subscriber.Close();
            publisher.Close();
        }

        /// <summary>
        ///     创建数据订阅者测试
        /// </summary>
        [Test]
        public void RecvSubscribedContentTest_string_int32()
        {
            int testValue = 3, recvValue;
            string testKey = "age", recvKey;
            IDataPublisher<string, int> publisher = DataPublisherFactory.Instance.Create<string, int>("RecvSubscribedContentTest_string_int32", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            IRemoteDataSubscriber<string, int> subscriber = DataSubscriberFactory.Instance.Create<string, int>("RecvSubscribedContentTest_string_int32", new NetworkResource("127.0.0.1:9898"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            Assert.IsTrue(subscriber.Open() == SubscriberState.Subscribed);
            Assert.IsTrue(subscriber.State == SubscriberState.Subscribed);
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            subscriber.MessageRecv +=
            delegate(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<EventArgs.DataRecvEventArgs<string, int>> e)
                {
                    recvKey = e.Target.Key;
                    recvValue = e.Target.Value;
                    Console.WriteLine("RecvKey = {0}, Value = {1}.", recvKey, recvValue);
                    autoResetEvent.Set();
                };
            publisher.Publish(testKey, testValue);
            System.Exception exception = null;
            if (!autoResetEvent.WaitOne(30000)) exception = new System.Exception("RecvSubscribedContentTest_string_int32 timeout!");
            subscriber.Close();
            publisher.Close();
            if (exception != null) throw exception;
        }

        /// <summary>
        ///     创建数据订阅者测试
        /// </summary>
        [Test]
        public void RecvSubscribedContentTest_string_nullable_int32()
        {
            int? testValue = 3, recvValue;
            string testKey = "age", recvKey;
            IDataPublisher<string, int?> publisher = DataPublisherFactory.Instance.Create<string, int?>("RecvSubscribedContentTest_string_nullable_int32", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            IRemoteDataSubscriber<string, int?> subscriber = DataSubscriberFactory.Instance.Create<string, int?>("RecvSubscribedContentTest_string_nullable_int32", new NetworkResource("127.0.0.1:9898"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            Assert.IsTrue(subscriber.Open() == SubscriberState.Subscribed);
            Assert.IsTrue(subscriber.State == SubscriberState.Subscribed);
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            subscriber.MessageRecv +=
            delegate(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<EventArgs.DataRecvEventArgs<string, int?>> e)
            {
                recvKey = e.Target.Key;
                recvValue = e.Target.Value;
                Console.WriteLine("RecvKey = {0}, Value = {1}.", recvKey, recvValue);
                autoResetEvent.Set();
            };
            publisher.Publish(testKey, testValue);
            System.Exception exception = null;
            if (!autoResetEvent.WaitOne(30000)) exception = new System.Exception("RecvSubscribedContentTest_string_nullable_int32 timeout!");
            subscriber.Close();
            publisher.Close();
            if (exception != null) throw exception;
        }

        /// <summary>
        ///     创建数据订阅者测试
        /// </summary>
        [Test]
        public void RecvSubscribedContentTest_nullable_double_nullable_int32()
        {
            int? testValue = 3, recvValue;
            double? testKey = 61.5, recvKey;
            IDataPublisher<double?, int?> publisher = DataPublisherFactory.Instance.Create<double?, int?>("RecvSubscribedContentTest_nullable_double_nullable_int32", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            IRemoteDataSubscriber<double?, int?> subscriber = DataSubscriberFactory.Instance.Create<double?, int?>("RecvSubscribedContentTest_nullable_double_nullable_int32", new NetworkResource("127.0.0.1:9898"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            Assert.IsTrue(subscriber.Open() == SubscriberState.Subscribed);
            Assert.IsTrue(subscriber.State == SubscriberState.Subscribed);
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            subscriber.MessageRecv +=
            delegate(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<EventArgs.DataRecvEventArgs<double?, int?>> e)
            {
                recvKey = e.Target.Key;
                recvValue = e.Target.Value;
                Console.WriteLine("RecvKey = {0}, Value = {1}.", recvKey, recvValue);
                autoResetEvent.Set();
            };
            publisher.Publish(testKey, testValue);
            System.Exception exception = null;
            if (!autoResetEvent.WaitOne(30000)) exception = new System.Exception("RecvSubscribedContentTest_nullable_double_nullable_int32 timeout!");
            subscriber.Close();
            publisher.Close();
            if (exception != null) throw exception;
        }

        /// <summary>
        ///     创建数据订阅者测试
        /// </summary>
        [Test]
        public void RecvSubscribedContentTest_nullable_double_datetime()
        {
            DateTime testValue = DateTime.Now, recvValue;
            double? testKey = 61.5, recvKey;
            IDataPublisher<double?, DateTime> publisher = DataPublisherFactory.Instance.Create<double?, DateTime>("RecvSubscribedContentTest_nullable_double_datetime", new NetworkResource(9898));
            Assert.IsNotNull(publisher);
            Assert.IsTrue(publisher.Policy == PublisherPolicy.Default);
            Assert.IsTrue(publisher.State == PublisherState.Prepared);
            Assert.IsTrue(publisher.Open() == PublisherState.Open);
            Assert.IsTrue(publisher.State == PublisherState.Open);
            IRemoteDataSubscriber<double?, DateTime> subscriber = DataSubscriberFactory.Instance.Create<double?, DateTime>("RecvSubscribedContentTest_nullable_double_datetime", new NetworkResource("127.0.0.1:9898"));
            Assert.IsNotNull(subscriber);
            Assert.IsTrue(subscriber.State == SubscriberState.ToBeSubscribe);
            Assert.IsTrue(subscriber.Open() == SubscriberState.Subscribed);
            Assert.IsTrue(subscriber.State == SubscriberState.Subscribed);
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            subscriber.MessageRecv +=
            delegate(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<EventArgs.DataRecvEventArgs<double?, DateTime>> e)
            {
                recvKey = e.Target.Key;
                recvValue = e.Target.Value;
                Console.WriteLine("RecvKey = {0}, Value = {1}.", recvKey, recvValue);
                autoResetEvent.Set();
            };
            publisher.Publish(testKey, testValue);
            System.Exception exception = null;
            if (!autoResetEvent.WaitOne(30000)) exception = new System.Exception("RecvSubscribedContentTest_nullable_double_datetime timeout!");
            subscriber.Close();
            publisher.Close();
            if (exception != null) throw exception;
        }

        #endregion
    }
}