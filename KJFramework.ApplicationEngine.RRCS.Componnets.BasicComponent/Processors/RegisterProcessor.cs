using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Processors;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent.Processors
{
    /// <summary>
    ///     注册到RRCS服务的消息信令处理器
    /// </summary>
    public class RegisterProcessor : IMessageTransactionProcessor<MetadataMessageTransaction, MetadataContainer>
    {
        #region Methods.

        /*
         *  Register to RRCS message structures:
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
         *      0x0C - Remoting cached End-Points resource blocks (ARRAY)
         *      -------- Internal resource block's structure --------
         *          0x00 - application's network identity.
         *          0x01 - application's version resource blocks  (ARRAY)
         *          -------- Internal resource block's structure --------
         *              0x00 - application's version.
         *              0x01 - application's end-points (STRING ARRAY).
         */
        public void Process(MetadataMessageTransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}