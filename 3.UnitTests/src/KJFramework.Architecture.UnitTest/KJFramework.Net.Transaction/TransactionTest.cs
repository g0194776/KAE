using System;
using System.Net;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net.Channels;
using KJFramework.Net.Enums;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.UnitTest.ProtocolStack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KJFramework.Net.Transaction.UnitTest
{
    public class TransactionTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            FixedTypeManager.Add(typeof(TransactionIdentity), 18);
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
        }

        [Test]
        public void TimeoutWith30S_Test()
        {
            DateTime now = DateTime.Now;
            MessageTransactionManager manager = new MessageTransactionManager(new TransactionIdentityComparer());
            BusinessMessageTransaction transaction = manager.Create(IdentityHelper.Create(new IPEndPoint(IPAddress.Parse("127.0.0.01"), 9999), TransportChannelTypes.TCP), new MessageTransportChannel<BaseMessage>(null, new TestProtocolStack()));
            Assert.IsNotNull(transaction);
            Assert.IsFalse (transaction.GetLease().IsDead);
            Assert.IsFalse(transaction.GetLease().ExpireTime == now.Add(Global.TransactionTimeout));
        }

        [Test]
        public void TimeoutWith10S_Test()
        {
            DateTime now = DateTime.Now;
            MessageTransactionManager manager = new MessageTransactionManager(new TransactionIdentityComparer());
            BusinessMessageTransaction transaction = manager.Create(IdentityHelper.Create(new IPEndPoint(IPAddress.Parse("127.0.0.01"), 9999), TransportChannelTypes.TCP), new MessageTransportChannel<BaseMessage>(null, new TestProtocolStack()), TimeSpan.Parse("00:00:10"));
            Assert.IsNotNull(transaction);
            Assert.IsFalse(transaction.GetLease().IsDead);
            Assert.IsFalse(transaction.GetLease().ExpireTime == now.Add(TimeSpan.Parse("00:00:10")));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TimeoutExceptionTest()
        {
            MessageTransactionManager manager = new MessageTransactionManager(new TransactionIdentityComparer());
            BusinessMessageTransaction transaction = manager.Create(IdentityHelper.Create(new IPEndPoint(IPAddress.Parse("127.0.0.01"), 9999), TransportChannelTypes.TCP), new MessageTransportChannel<BaseMessage>(null, new TestProtocolStack()), TimeSpan.Parse("00:00:00"));
            Assert.IsNotNull(transaction);
        }

        #endregion
    }
}