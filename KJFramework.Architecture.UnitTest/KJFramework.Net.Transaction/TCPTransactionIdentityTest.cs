using System;
using System.Net;
using System.Text;
using KJFramework.Messages.Engine;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.TypeProcessors.Maps;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ValueStored;
using NUnit.Framework;

namespace KJFramework.Net.Transaction.UnitTest
{
    public class TCPTransactionIdentityTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            MemoryAllotter.Instance.Initialize();
        }

        [Test]
        public void SerializeAsMetadataTest()
        {
            TransactionIdentity identity = new TCPTransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26666);
            TransactionIdentityValueStored stored = new TransactionIdentityValueStored(identity);
            MemorySegmentProxy proxy = new MemorySegmentProxy();
            stored.ToBytes(proxy);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 19);
            PrintBytes(data);
        }

        [Test]
        public void SerializeAsIntellectObjectTes()
        {
            TransactionIdentity identity = new TCPTransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26666);
            TransactionIdentityProcessor processor = new TransactionIdentityProcessor();
            MemorySegmentProxy proxy = new MemorySegmentProxy();
            processor.Process(proxy, identity, false, false);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 24);
            PrintBytes(data);
        }

        [Test]
        public void DeserializeTest()
        {
            BaseMessage baseMessage = new BaseMessage { MessageIdentity = new MessageIdentity { ProtocolId = 1, ServiceId = 2, DetailsId = 3 } };
            FixedTypeManager.Add(typeof(MessageIdentity), 5);
            IntellectTypeProcessorMapping.Instance.Regist(new TransactionIdentityProcessor());
            IntellectTypeProcessorMapping.Instance.Regist(new MessageIdentityProcessor());
            TransactionIdentity identity = new TCPTransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26666);
            baseMessage.TransactionIdentity = identity;
            baseMessage.Bind();
            Assert.IsTrue(baseMessage.IsBind);
            byte[] data = baseMessage.Body;
            Assert.IsTrue(data.Length > 0);
            Assert.IsTrue(data.Length == 34);


            BaseMessage newMessage = IntellectObjectEngine.GetObject<BaseMessage>(data);
            Assert.IsNotNull(newMessage);
            Assert.IsTrue(newMessage.MessageIdentity.ProtocolId == 1);
            Assert.IsTrue(newMessage.MessageIdentity.ServiceId == 2);
            Assert.IsTrue(newMessage.MessageIdentity.DetailsId == 3);
            Assert.IsNotNull(newMessage.TransactionIdentity);
            Assert.IsTrue(newMessage.TransactionIdentity.IsRequest);
            Assert.IsTrue(newMessage.TransactionIdentity.MessageId == 3);
            Assert.IsNotNull(newMessage.TransactionIdentity.EndPoint);
            Assert.IsTrue(((IPEndPoint)newMessage.TransactionIdentity.EndPoint).Port == 26666);
            Assert.IsTrue(((IPEndPoint)newMessage.TransactionIdentity.EndPoint).Address.ToString() == "127.0.0.1");
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

        #endregion
    }
}