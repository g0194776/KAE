using System;
using KJFramework.ApplicationEngine.Entities;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Helpers;
using KJFramework.Enums;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Types;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Results;
using System.Collections.Generic;
using KJFramework.Net;
using KJFramework.Net.Identities;

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
         *      0x02 - KAE-Host's Default Communication Port
         *      0x03 - Register Source Type
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
         *      0x0C - Remoting cached End-Points resource blocks (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's level.
         *          0x01 - application's version.
         *          0x02 - targeted application's network resource uri (STRING)
         *          0x01 - targeted application supported all network abilities  (ARRAY)
         *          -------- Internal resource block's structure --------
         *              0x00 - Message Identity
         *              0x01 - Supported Protocol
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
            KAEHostRegisterSourceTypes registerType = (KAEHostRegisterSourceTypes) reqMsg.GetAttributeAsType<byte>(0x03);
            Guid guid = Guid.NewGuid();
            transaction.GetChannel().Tag = new Tuple<KAEHostRegisterSourceTypes, Guid>(registerType, guid);
            transaction.GetChannel().Disconnected += ChannelDisconnected;
            RemotingServerManager.Register(guid, metadataDic);
            IDictionary<string, List<string>> information = RemotingServerManager.GetAllInformation();
            rspMsg.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.OK));
            rspMsg.SetAttribute(0x0C, new ResourceBlockArrayStored(ConvertReturnMessage(information)));
            rspMsg.SetAttribute(0x0D, new GuidValueStored(guid));
            rspMsg.SetAttribute(0x0E, new StringValueStored(RemotingServerManager.RemotingPublisherUri.ToString()));
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
                    string key = string.Format("({0},{1},{2})_{3}_{4}_{5}", identity.ProtocolId, identity.ServiceId, identity.DetailsId, pair.Key, appInfo.Level, appInfo.Version);
                    List<string> tempValue;
                    if(!dic.TryGetValue(key, out tempValue)) dic.Add(key, (tempValue = new List<string>()));
                    tempValue.AddRange(networkResources);
                }
            }
            return true;
        }

        private ResourceBlock[] ConvertReturnMessage(IDictionary<string, List<string>> dic)
        {
            ResourceBlock[] msgs = new ResourceBlock[dic.Count];
            int offset = 0;
            foreach (KeyValuePair<string, List<string>> pair in dic)
            {
                ResourceBlock msg = new ResourceBlock();
                msg.SetAttribute(0x00, new StringValueStored(pair.Key));
                msg.SetAttribute(0x01, new StringArrayValueStored(pair.Value.ToArray()));
                msgs[offset++] = msg;
            }
            return msgs;
        }

        #endregion

        #region Events

        void ChannelDisconnected(object sender, System.EventArgs e)
        {
            IMessageTransportChannel<MetadataContainer> msgChannel = (IMessageTransportChannel<MetadataContainer>) sender;
            msgChannel.Disconnected -= ChannelDisconnected;
            Tuple<KAEHostRegisterSourceTypes, Guid> tuple = (Tuple<KAEHostRegisterSourceTypes, Guid>)msgChannel.Tag;
            if (tuple.Item1 == KAEHostRegisterSourceTypes.Service) RemotingServerManager.UnRegister(tuple.Item2);
        }

        #endregion
    }
}