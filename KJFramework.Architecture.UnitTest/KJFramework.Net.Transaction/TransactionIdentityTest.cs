using System;
using System.Net;
using System.Text;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ValueStored;
using NUnit.Framework;

namespace KJFramework.Net.Transaction.UnitTest
{
    public class TransactionIdentityTest
    {
        #region Methods.

        [SetUp]
        public void Initialize()
        {
            MemoryAllotter.Instance.Initialize();
        }

        [Test]
        public void SerializeTest()
        {
            TransactionIdentity identity = new TransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26666);
            TransactionIdentityValueStored stored = new TransactionIdentityValueStored(identity);
            MemorySegmentProxy proxy = new MemorySegmentProxy();
            stored.ToBytes(proxy);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 18);
            PrintBytes(data);
        }

        [Test]
        public void SerializeTest2()
        {
            TransactionIdentity identity = new TransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26666);
            TransactionIdentityProcessor processor = new TransactionIdentityProcessor();
            MemorySegmentProxy proxy = new MemorySegmentProxy();
            processor.Process(proxy, identity, false, false);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 18);
            PrintBytes(data);
        }

        [Test]
        public void DeserializeTest()
        {
            TransactionIdentity identity = new TransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26666);
            TransactionIdentityValueStored stored = new TransactionIdentityValueStored(identity);
            MemorySegmentProxy proxy = new MemorySegmentProxy();
            stored.ToBytes(proxy);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 18);
            PrintBytes(data);

            MetadataContainer container = new MetadataContainer();
            stored.ToData(container, 0, data, 0, (uint) data.Length);
            Assert.IsTrue(container.IsAttibuteExsits(0x00));
            Assert.IsNotNull(container.GetAttribute(0x00));
            Assert.IsNotNull(container.GetAttribute(0x00).GetValue<TransactionIdentity>());
            Assert.IsTrue(container.GetAttribute(0x00).GetValue<TransactionIdentity>().IsRequest);
            Assert.IsTrue(container.GetAttribute(0x00).GetValue<TransactionIdentity>().MessageId == 3);
            Assert.IsNotNull(container.GetAttribute(0x00).GetValue<TransactionIdentity>().EndPoint);
            Assert.IsTrue(container.GetAttribute(0x00).GetValue<TransactionIdentity>().EndPoint.Port == 26666);
            Assert.IsTrue(container.GetAttribute(0x00).GetValue<TransactionIdentity>().EndPoint.Address.ToString() == "127.0.0.1");
        }

        [Test]
        public void DeserializeTest2()
        {
            TransactionIdentity identity = new TransactionIdentity();
            identity.MessageId = 3;
            identity.IsRequest = true;
            identity.EndPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 26666);
            TransactionIdentityProcessor processor = new TransactionIdentityProcessor();
            MemorySegmentProxy proxy = new MemorySegmentProxy();
            processor.Process(proxy, identity, false, false);
            byte[] data = proxy.GetBytes();
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Length == 18);
            PrintBytes(data);

            TransactionIdentity identity2 = (TransactionIdentity) processor.Process(new IntellectPropertyAttribute(0), data);
            Assert.IsNotNull(identity2);
            Assert.IsTrue(identity2.IsRequest);
            Assert.IsTrue(identity2.MessageId == 3);
            Assert.IsNotNull(identity2.EndPoint);
            Assert.IsTrue(identity2.EndPoint.Port == 26666);
            Assert.IsTrue(identity2.EndPoint.Address.ToString() == "255.255.255.255");
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