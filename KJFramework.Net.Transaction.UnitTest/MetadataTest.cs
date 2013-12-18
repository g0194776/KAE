using System;
using System.Collections.Generic;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction.Identities;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KJFramework.Net.Transaction.UnitTest
{
    [TestClass]
    public class MetadataTest : MetadataProtocolStack
    {
        #region Methods

        [TestMethod]
        public void MetaMessageToBytes()
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
                ServiceId = 3,
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
            byte[] data1 = protocolStack.ConvertToBytes(metadata2);
            metadata2.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity2));
            byte[] data2 = protocolStack.ConvertToBytes(metadata2);
            metadata3.SetAttribute(0x00, new MessageIdentityValueStored(messageIdentity3));
            byte[] data3 = protocolStack.ConvertToBytes(metadata2);
            byte[] totalData = new byte[data1.Length+data2.Length+data3.Length];
            System.Buffer.BlockCopy(data1,0,totalData,0,data1.Length);
            System.Buffer.BlockCopy(data2, 0, totalData, data1.Length, data2.Length);
            System.Buffer.BlockCopy(data3,0,totalData,data1.Length+data2.Length,data3.Length);

            List<MetadataContainer> list = protocolStack.Parse(totalData);
        }

        public override bool Initialize()
        {
            throw new NotImplementedException();
        }

      #endregion
    }
}
