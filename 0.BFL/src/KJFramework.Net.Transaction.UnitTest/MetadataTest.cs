using System;
using System.Collections.Generic;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace KJFramework.Net.Transaction.UnitTest
{
    public class MetadataTest : MetadataProtocolStack
    {
        #region Methods

        [SetUp]
        public void Setup()
        {
            ExtensionTypeMapping.Regist(typeof (MessageIdentityValueStored));
        }

        [Test]
        public void MetaOneMessageToBytes()
        {
            MetadataProtocolStack protocolStack = new MetadataTest();
            MessageIdentity messageIdentity1 = new MessageIdentity
            {
                DetailsId = 1,
                ProtocolId = 2,
                ServiceId = 3,
                Tid = 4
            };

            MetadataContainer metadata1 = new MetadataContainer();
            metadata1.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity1));
            byte[] data1 = protocolStack.ConvertToBytes(metadata1);
            Assert.IsNotNull(data1);
            Assert.IsTrue(data1.Length == 16);
            byte[] totalData = new byte[data1.Length];
            System.Buffer.BlockCopy(data1, 0, totalData, 0, data1.Length);
            Assert.IsNotNull(totalData);
            Assert.IsTrue(totalData.Length == 16);

            List<MetadataContainer> list = protocolStack.Parse(totalData);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().DetailsId == 1);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ProtocolId == 2);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ServiceId == 3);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().Tid == 4);
            Console.WriteLine(list[0].GetAttribute(0x00).GetValue<MessageIdentity>());
        }

        [Test]
        public void MetaOneMessageBetweenTwoNullSegmentToBytes()
        {
            MetadataProtocolStack protocolStack = new MetadataTest();
            MessageIdentity messageIdentity1 = new MessageIdentity
            {
                DetailsId = 1,
                ProtocolId = 2,
                ServiceId = 3,
                Tid = 4
            };

            MetadataContainer metadata1 = new MetadataContainer();
            metadata1.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity1));
            byte[] data1 = protocolStack.ConvertToBytes(metadata1);
            Assert.IsNotNull(data1);
            Assert.IsTrue(data1.Length == 16);
            byte[] totalData = new byte[data1.Length+21];
            System.Buffer.BlockCopy(data1, 0, totalData, 10, data1.Length);
            Assert.IsNotNull(totalData);
            Assert.IsTrue(totalData.Length == 37);

            List<MetadataContainer> list = protocolStack.Parse(totalData, 10, data1.Length);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().DetailsId == 1);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ProtocolId == 2);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ServiceId == 3);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().Tid == 4);
            Console.WriteLine(list[0].GetAttribute(0x00).GetValue<MessageIdentity>());
        }

        [Test]
        public void MetaOneMessageAfterNullSegmentToBytes()
        {
            MetadataProtocolStack protocolStack = new MetadataTest();
            MessageIdentity messageIdentity1 = new MessageIdentity
            {
                DetailsId = 1,
                ProtocolId = 2,
                ServiceId = 3,
                Tid = 4
            };
            MessageIdentity messageIdentity2 = new MessageIdentity
            {
                DetailsId = 2,
                ProtocolId = 3,
                ServiceId = 4,
                Tid = 5
            };

            MetadataContainer metadata1 = new MetadataContainer();
            MetadataContainer metadata2 = new MetadataContainer();
            metadata1.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity1));
            byte[] data1 = protocolStack.ConvertToBytes(metadata1);
            Assert.IsNotNull(data1);
            Assert.IsTrue(data1.Length == 16);
            byte[] data2 = protocolStack.ConvertToBytes(metadata2);
            Assert.IsNotNull(data2);
            Assert.IsTrue(data1.Length == 16);
            byte[] totalData = new byte[data1.Length + 11 + data2.Length];
            System.Buffer.BlockCopy(data1, 0, totalData, 10, data1.Length);
            System.Buffer.BlockCopy(data2,0,totalData,data1.Length+10,data2.Length);
            Assert.IsNotNull(totalData);
            Assert.IsTrue(totalData.Length == 33);

            List<MetadataContainer> list = protocolStack.Parse(totalData, 10, data1.Length);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().DetailsId == 1);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ProtocolId == 2);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ServiceId == 3);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().Tid == 4);
            Console.WriteLine(list[0].GetAttribute(0x00).GetValue<MessageIdentity>());
        }

        [Test]
        public void MetaManyMessageToBytes()
        {
            MetadataProtocolStack protocolStack = new MetadataTest();
            MessageIdentity messageIdentity1 = new MessageIdentity
            {
                DetailsId = 1,
                ProtocolId = 2,
                ServiceId = 3,
                Tid = 4
            };
            MessageIdentity messageIdentity2 = new MessageIdentity
            {
                DetailsId = 2,
                ProtocolId = 3,
                ServiceId = 4,
                Tid = 5
            };
            MessageIdentity messageIdentity3 = new MessageIdentity
            {
                DetailsId = 3,
                ProtocolId = 4,
                ServiceId = 5,
                Tid = 6
            };
            MetadataContainer metadata1 = new MetadataContainer();
            MetadataContainer metadata2 = new MetadataContainer();
            MetadataContainer metadata3 = new MetadataContainer();
            metadata1.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity1));
            byte[] data1 = protocolStack.ConvertToBytes(metadata1);
            Assert.IsNotNull(data1);
            Assert.IsTrue(data1.Length == 16);
            metadata2.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity2));
            byte[] data2 = protocolStack.ConvertToBytes(metadata2);
            Assert.IsNotNull(data2);
            Assert.IsTrue(data1.Length == 16);
            metadata3.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity3));
            byte[] data3 = protocolStack.ConvertToBytes(metadata3);
            Assert.IsNotNull(data3);
            Assert.IsTrue(data1.Length == 16);
            byte[] totalData = new byte[data1.Length+data2.Length+data3.Length];
            System.Buffer.BlockCopy(data1,0,totalData,0,data1.Length);
            System.Buffer.BlockCopy(data2, 0, totalData, data1.Length, data2.Length);
            System.Buffer.BlockCopy(data3,0,totalData,data1.Length+data2.Length,data3.Length);
            Assert.IsNotNull(totalData);
            Assert.IsTrue(totalData.Length == 48);
          
            List<MetadataContainer> list = protocolStack.Parse(totalData);
            Assert.IsNotNull(list);
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().DetailsId == 1 );
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ProtocolId == 2 );
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().ServiceId == 3 );
            Assert.IsTrue(list[0].GetAttribute(0x00).GetValue<MessageIdentity>().Tid == 4 );
            Assert.IsTrue(list[1].GetAttribute(0x00).GetValue<MessageIdentity>().DetailsId == 2);
            Assert.IsTrue(list[1].GetAttribute(0x00).GetValue<MessageIdentity>().ProtocolId == 3);
            Assert.IsTrue(list[1].GetAttribute(0x00).GetValue<MessageIdentity>().ServiceId == 4);
            Assert.IsTrue(list[1].GetAttribute(0x00).GetValue<MessageIdentity>().Tid == 5);
            Assert.IsTrue(list[2].GetAttribute(0x00).GetValue<MessageIdentity>().DetailsId == 3);
            Assert.IsTrue(list[2].GetAttribute(0x00).GetValue<MessageIdentity>().ProtocolId == 4);
            Assert.IsTrue(list[2].GetAttribute(0x00).GetValue<MessageIdentity>().ServiceId == 5);
            Assert.IsTrue(list[2].GetAttribute(0x00).GetValue<MessageIdentity>().Tid == 6);
            Console.WriteLine(list[0].GetAttribute(0x00).GetValue<MessageIdentity>());
            Console.WriteLine(list[1].GetAttribute(0x00).GetValue<MessageIdentity>());
            Console.WriteLine(list[2].GetAttribute(0x00).GetValue<MessageIdentity>());
        }

        public override bool Initialize()
        {
            return true;
        }

      #endregion
    }
}
