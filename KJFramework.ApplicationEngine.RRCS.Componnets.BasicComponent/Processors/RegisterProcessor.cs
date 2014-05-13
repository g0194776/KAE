﻿using System;
using KJFramework.ApplicationEngine.Entities;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Helpers;
using KJFramework.Enums;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Results;
using System.Collections.Generic;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Processors
{
    /// <summary>
    ///     注册到RRCS服务的消息信令处理器
    /// </summary>
    public class RegisterProcessor : MessageTransactionProcessor
    {
        #region Methods.

        /*
         *  Registers to RRCS message structures:
         *  [REQ MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x0A - Application's Information (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's CRC.
         *          0x01 - application's network resource blocks  (ARRAY)
         *          -------- Internal resource block's structure --------
         *              0x00 - supported newtork protocol.
         *              0x01 - network end-points.  (STRING ARRAY).
         * 
         *  [RSP MESSAGE]
         *  ===========================================
         *      0x00 - Message Identity
         *      0x01 - Transaction Identity
         *      0x0A - Error Id
         *      0x0B - Error Reason
         */
        protected override void InnerProcess(MetadataMessageTransaction transaction)
        {
            MetadataContainer reqMsg = transaction.Request;
            MetadataContainer rspMsg = new MetadataContainer();
            rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity { ProtocolId = 0xFC, ServiceId = 0, DetailsId = 1 }));
            ResourceBlock[] resBlocks = reqMsg.GetAttributeAsType<ResourceBlock[]>(0x0A);
            if (resBlocks == null || resBlocks.Length == 0)
            {
                rspMsg = new MetadataContainer();
                rspMsg.SetAttribute(0x00, new MessageIdentityValueStored(new MessageIdentity{ProtocolId = 0xFC, ServiceId = 0, DetailsId = 1}));
                rspMsg.SetAttribute(0x0A, new ByteValueStored((byte) KAEErrorCodes.SpecifiedKAEHostHasNoAnyApplication));
                rspMsg.SetAttribute(0x0B, new StringValueStored("#Targeted KAE Host didn't has any allowed application, that's not allowed."));
                transaction.SendResponse(rspMsg);
                return;
            }
            //beginning analyzes.
            ApplicationInformation appInfo;
            //key = MessageIdentity + Supported Protocol + Application's Level + Application's Version
            IDictionary<string, List<string>> metadataDic = new Dictionary<string, List<string>>();
            foreach (ResourceBlock resBlock in resBlocks)
            {
                #region Preparation of supported networks.

                IDictionary<ProtocolTypes, List<string>> protocols = new Dictionary<ProtocolTypes, List<string>>();
                foreach (ResourceBlock block in resBlock.GetAttributeAsType<ResourceBlock[]>(0x01))
                {
                    ProtocolTypes protocolType = (ProtocolTypes) block.GetAttributeAsType<byte>(0x00);
                    IList<string> networks = new List<string>(block.GetAttributeAsType<string[]>(0x01) ?? new string[] {});
                    List<string> tempNetworks;
                    if (!protocols.TryGetValue(protocolType, out tempNetworks))
                        protocols.Add(protocolType, (tempNetworks = new List<string>()));
                    tempNetworks.AddRange(networks);
                }

                #endregion
                long appCRC = resBlock.GetAttributeAsType<long>(0x00);
                IExecuteResult<ApplicationInformation> result = ApplicationHelper.GetApplicationInformationByCRCAsync(appCRC);
                if (result.State != ExecuteResults.Succeed)
                {
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.CommunicaitonFailedWithAPMS));
                    rspMsg.SetAttribute(0x0B, new StringValueStored("#Communication Failed with APMS."));
                    transaction.SendResponse(rspMsg);
                    return;
                }
                if ((appInfo = result.GetResult()) == null)
                {
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.NullResultWithTargetedAppCRC));
                    rspMsg.SetAttribute(0x0B, new StringValueStored(string.Format("#RRCS couldn't gets any information by targeted CRC value: {0}.", appCRC)));
                    transaction.SendResponse(rspMsg);
                    return;
                }
                if (!AppendResults(metadataDic, protocols, appInfo))
                {
                    rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.IllegalSupportedInformation));
                    rspMsg.SetAttribute(0x0B, new StringValueStored("#Illegal network information in current committed register."));
                    transaction.SendResponse(rspMsg);
                    return;
                }
            }
            Guid guid = Guid.NewGuid();
            transaction.GetChannel().Tag = guid;
            transaction.GetChannel().Disconnected += ChannelDisconnected;
            RemotingServerManager.Register(guid, metadataDic);
            rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.OK));
            transaction.SendResponse(rspMsg);
        }

        private bool AppendResults(IDictionary<string, List<string>> dic, IDictionary<ProtocolTypes, List<string>> resources, ApplicationInformation appInfo)
        {
            //key = MessageIdentity + Supported Protocol + Application's Level + Application's Version
            foreach (KeyValuePair<ProtocolTypes, IList<MessageIdentity>> pair in appInfo.MessageIdentities)
            {
                List<string> networkResources;
                if (!resources.TryGetValue(pair.Key, out networkResources)) return false;
                foreach (MessageIdentity identity in pair.Value)
                {
                    string key = string.Format("({0}_{1}_{2})_{3}_{4}_{5}", identity.ProtocolId, identity.ServiceId, identity.DetailsId, pair.Key, appInfo.Level, appInfo.Version);
                    List<string> tempValue;
                    if(!dic.TryGetValue(key, out tempValue)) dic.Add(key, (tempValue = new List<string>()));
                    tempValue.AddRange(networkResources);
                }
            }
            return true;
        }

        #endregion

        #region Events

        void ChannelDisconnected(object sender, System.EventArgs e)
        {
            IMessageTransportChannel<MetadataContainer> msgChannel = (IMessageTransportChannel<MetadataContainer>) sender;
            msgChannel.Disconnected -= ChannelDisconnected;
            Guid guid = (Guid) msgChannel.Tag;
            RemotingServerManager.UnRegister(guid);
        }

        #endregion
    }
}