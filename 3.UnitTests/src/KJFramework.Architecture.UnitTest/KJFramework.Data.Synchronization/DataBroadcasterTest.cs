﻿using System;
using System.Threading;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.EventArgs;
using NUnit.Framework;

namespace KJFramework.Data.Synchronization.UnitTest
{
    public class DataBroadcasterTest
    {
        [SetUp]
        public void Initialize()
        {
            SyncDataFramework.Initialize();
        }

        /// <summary>
        ///     测试广播消息
        ///     <para>Key: string, Value: string</para>
        /// </summary>
        [Test]
        public void BroadcasterString()
        {
            string stringKey = "a";
            string stringValue = "b";
            string recvKey = null;
            string recvValue = null;
            IDataPublisher<string, string> dataPublisher = DataPublisherFactory.Instance.Create<string, string>("Catalog_string", new NetworkResource(8888));
            Assert.IsTrue(dataPublisher.Open() == PublisherState.Open);
            IRemoteDataSubscriber<string, string> remoteDataSubscriber = DataSubscriberFactory.Instance.Create<string, string>("Catalog_string", new NetworkResource("127.0.0.1:8888"));
            AutoResetEvent are = new AutoResetEvent(false);
            remoteDataSubscriber.MessageRecv += delegate(object sender, LightSingleArgEventArgs<DataRecvEventArgs<string, string>> e)
            {
                recvKey = e.Target.Key;
                recvValue = e.Target.Value;
                if (e.Target != null)
                {
                    Console.WriteLine("Receive Key:" + e.Target.Key);
                    Console.WriteLine("Receive Value:" + e.Target.Value);
                }
                are.Set();
            };
            remoteDataSubscriber.Open();
            IDataBroadcaster<string, string> broadcaster = DataBroadcasterFactory.Instance.Create<string, string>("Catalog_string", new NetworkResource("127.0.0.1:8888"));
            if (!broadcaster.Broadcast(stringKey, stringValue)) throw new System.Exception("Broadcaster failed!");
            Console.WriteLine("Send Key:" + stringKey);
            Console.WriteLine("Send Value:" + stringValue);
            if (!are.WaitOne(60 * 1000)) throw new System.Exception("Broadcaster Timeout!");
            broadcaster.Close();
            remoteDataSubscriber.Close();
            dataPublisher.Close();
            Assert.IsTrue(recvKey.CompareTo(stringKey) == 0);
            Assert.IsTrue(recvValue.CompareTo(stringValue) == 0);
        }

        /// <summary>
        ///     测试广播消息
        ///     <para>Key: Test, Value: Test</para>
        /// </summary>
        [Test]
        public void BroadcasterComplexObject()
        {
            Test testKey = new Test {Name = "zhangsan", Age = 18};
            Test testValue = new Test {Name = "v-dish", Age = 100};
            Test recvKey = null;
            Test recvValue = null;
            IDataPublisher<Test, Test> dataPublisher = DataPublisherFactory.Instance.Create<Test, Test>("Catalog_Test", new NetworkResource(8888));
            Assert.IsTrue(dataPublisher.Open() == PublisherState.Open);
            IRemoteDataSubscriber<Test, Test> remoteDataSubscriber = DataSubscriberFactory.Instance.Create<Test, Test>("Catalog_Test", new NetworkResource("127.0.0.1:8888"));
            AutoResetEvent are = new AutoResetEvent(false);
            remoteDataSubscriber.MessageRecv += delegate(object sender, LightSingleArgEventArgs<DataRecvEventArgs<Test, Test>> e)
            {
                recvKey = e.Target.Key;
                recvValue = e.Target.Value;
                if (e.Target != null)
                {
                    Console.WriteLine("Receive Key:" + e.Target.Key);
                    Console.WriteLine("Receive Value:" + e.Target.Value);
                }
                are.Set();
            };
            remoteDataSubscriber.Open();
            IDataBroadcaster<Test, Test> broadcaster = DataBroadcasterFactory.Instance.Create<Test, Test>("Catalog_Test", new NetworkResource("127.0.0.1:8888"));
            if (!broadcaster.Broadcast(testKey, testValue)) throw new System.Exception("Broadcaster failed!");
            Console.WriteLine("Send Key:" + testKey);
            Console.WriteLine("Send Value:" + testValue);
            if (!are.WaitOne(60 * 1000)) throw new System.Exception("Broadcaster Timeout!");
            broadcaster.Close();
            remoteDataSubscriber.Close();
            dataPublisher.Close();
            Assert.IsTrue(((IComparable<Test>)recvKey).CompareTo(testKey) == 0);
            Assert.IsTrue(((IComparable<Test>)recvValue).CompareTo(testValue) == 0);
        }


        /// <summary>
        ///     广播者自动重连测试
        ///     <para>Key: string, Value: string</para>
        /// </summary>
        [Test]
        public void BroadcasterReconnectTest()
        {
            string stringKey = "a";
            string stringValue = "b";
            string recvKey = null;
            string recvValue = null;
            IDataBroadcaster<string, string> broadcaster = DataBroadcasterFactory.Instance.Create<string, string>("Catalog_string", new NetworkResource("127.0.0.1:8888"), true);
            Assert.IsTrue(broadcaster.State == BroadcasterState.Reconnecting);
            IDataPublisher<string, string> dataPublisher = DataPublisherFactory.Instance.Create<string, string>("Catalog_string", new NetworkResource(8888));
            Assert.IsTrue(dataPublisher.Open() == PublisherState.Open);
            //10s.
            Thread.Sleep(10000);
            Assert.IsTrue(broadcaster.State == BroadcasterState.Connected);
            IRemoteDataSubscriber<string, string> remoteDataSubscriber = DataSubscriberFactory.Instance.Create<string, string>("Catalog_string", new NetworkResource("127.0.0.1:8888"));
            AutoResetEvent are = new AutoResetEvent(false);
            remoteDataSubscriber.MessageRecv += delegate(object sender, LightSingleArgEventArgs<DataRecvEventArgs<string, string>> e)
            {
                recvKey = e.Target.Key;
                recvValue = e.Target.Value;
                if (e.Target != null)
                {
                    Console.WriteLine("Receive Key:" + e.Target.Key);
                    Console.WriteLine("Receive Value:" + e.Target.Value);
                }
                are.Set();
            };
            remoteDataSubscriber.Open();
            if (!broadcaster.Broadcast(stringKey, stringValue)) throw new System.Exception("Broadcaster failed!");
            Console.WriteLine("Send Key:" + stringKey);
            Console.WriteLine("Send Value:" + stringValue);
            if (!are.WaitOne(60 * 1000)) throw new System.Exception("Broadcaster Timeout!");
            remoteDataSubscriber.Close();
            dataPublisher.Close();
            Thread.Sleep(2000);
            Assert.IsTrue(broadcaster.State == BroadcasterState.Reconnecting);
            broadcaster.Close();
            Assert.IsTrue(broadcaster.State == BroadcasterState.Disconnected);
            Assert.IsTrue(recvKey.CompareTo(stringKey) == 0);
            Assert.IsTrue(recvValue.CompareTo(stringValue) == 0);
        }
    }
}