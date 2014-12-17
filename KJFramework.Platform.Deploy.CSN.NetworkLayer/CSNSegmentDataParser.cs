using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Events;
using KJFramework.Net.Channels.Objects;
using KJFramework.Net.Channels.Parsers;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     数据段解析器元接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class CSNSegmentDataParser<T> : ICSNSegmentDataParser<T>
    {
        #region Constructor

        /// <summary>
        ///     数据段解析器元接口
        /// </summary>
        /// <param name="protocolStack">协议栈</param>
        public CSNSegmentDataParser(IProtocolStack protocolStack)
        {
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            _protocolStack = protocolStack;
        }

        #endregion

        #region Members

        private CSNSegmentNode _head;
        private CSNSegmentNode _tail;
        private readonly IProtocolStack _protocolStack;

        #endregion

        #region Implementation of ICSNSegmentDataParser<T>

        /// <summary>
        ///     追加一个新的数据段
        /// </summary>
        /// <param name="args">数据段接受参数</param>
        public void Append(CSNSegmentReceiveEventArgs args)
        {
            #region Step 1, check-on args.

            if (_head == null) _head = _tail = new CSNSegmentNode(args);
            else
            {
                _tail.Next = new CSNSegmentNode(args);
                _tail = _tail.Next;
            }

            #endregion

            #region Step 2, check bytes enough & pickup data.

            int msgSize;
            //RESET next node for current expression.
            CSNSegmentNode nextNode = _head;
            List<T> msgs = new List<T>();
            //check whatever bytes can be parse.
            while (nextNode != null && (msgSize = CheckBytes(nextNode)) > 0)
            {
                //direct parse.
                if (nextNode.RemainingSize >= msgSize)
                {
                    List<T> list = _protocolStack.Parse<T>(nextNode.Args.Stub.Cache.Segment.Segment.Array, nextNode.Args.Stub.Cache.Segment.UsedOffset, msgSize);
                    if (list != null) msgs.AddRange(list);
                    nextNode.Args.Stub.Cache.Segment.UsedBytes += msgSize;
                    CSNChannelCounter.Instance.RateOfDirectParse.Increment();
                    if (nextNode.RemainingSize > 0) continue;
                    //giveup current fixed stub.
                    CSNChannelConst.BuffAsyncStubPool.Giveback(nextNode.Args.Stub);
                    CSNChannelCounter.Instance.RateOfFixedBufferStubGiveback.Increment();
                    _head = nextNode = nextNode.Next;
                    if (_head != null) continue;
                    //Tail node must be null, if the head node has no value.
                    _tail = null;
                    //publish messages.
                    if (msgs.Count > 0) ParseSucceedHandler(new LightSingleArgEventArgs<List<T>>(msgs));
                    return;
                }
                //create sub-list for all ava-data segment path.
                int remainingSize = msgSize - nextNode.RemainingSize;
                CSNSegmentNode childHead = (CSNSegmentNode)nextNode.Clone();
                CSNSegmentNode childTail = (CSNSegmentNode)nextNode.Clone();
                CSNSegmentNode tempNode = nextNode;
                CSNSegmentNode lastRealNode = null;
                CSNSegmentNode cloneLastRealNode = null;
                do
                {
                    tempNode = tempNode.Next;
                    if (tempNode == null) break;
                    remainingSize -= tempNode.RemainingSize;
                    //clone target node for child-list.
                    lastRealNode = tempNode;
                    CSNSegmentNode cloneNode = (CSNSegmentNode)tempNode.Clone();
                    cloneNode.Next = null;
                    childTail.Next = cloneNode;
                    childTail = childTail.Next;
                } while (remainingSize > 0);
                //cannot get enough length for message really binary data.
                if (remainingSize > 0)
                {
                    if (msgs.Count > 0) ParseSucceedHandler(new LightSingleArgEventArgs<List<T>>(msgs));
                    return;
                }
                //copy data from child-list!
                int dataOffset = 0;
                byte[] data = new byte[msgSize];
                int dataRemainingCount = data.Length;
                int usedBytes;
                int lastUserBytes = 0;
                while (childHead != null && dataRemainingCount > 0)
                {
                    cloneLastRealNode = childHead;
                    System.Buffer.BlockCopy(childHead.Args.Stub.Cache.Segment.Segment.Array,
                                            childHead.Args.Stub.Cache.Segment.UsedOffset, data, dataOffset,
                                            (usedBytes = (childHead.RemainingSize > dataRemainingCount
                                                              ? dataRemainingCount
                                                              : childHead.RemainingSize)));
                    dataOffset += usedBytes;
                    dataRemainingCount -= usedBytes;
                    childHead.Args.Stub.Cache.Segment.UsedBytes += usedBytes;
                    lastUserBytes = childHead.Args.Stub.Cache.Segment.UsedBytes;
                    if (childHead.RemainingSize <= 0)
                    {
                        //giveup current fixed stub.
                        CSNChannelConst.BuffAsyncStubPool.Giveback(childHead.Args.Stub);
                        CSNChannelCounter.Instance.RateOfFixedBufferStubGiveback.Increment();
                        childHead = childHead.Next;
                    }
                }
                if (cloneLastRealNode.RemainingSize - lastUserBytes == 0 && lastRealNode.Next == null) _head = nextNode = null;
                else if (cloneLastRealNode.RemainingSize - lastUserBytes == 0 && lastRealNode.Next != null) _head = nextNode = lastRealNode.Next;
                else
                {
                    lastRealNode.Args.Stub.Cache.Segment.UsedBytes = lastUserBytes;
                    _head = nextNode = lastRealNode;
                }
                //_head = nextNode = ((cloneLastRealNode.RemainingSize == 0 && lastRealNode.Next == null) ? null : lastRealNode);
                List<T> list1 = _protocolStack.Parse<T>(data);
                if (list1 != null) msgs.AddRange(list1);
            }
            //publish messages.
            if (msgs.Count > 0) ParseSucceedHandler(new LightSingleArgEventArgs<List<T>>(msgs));

            #endregion
        }

        /// <summary>
        ///     解析成功事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<List<T>>> ParseSucceed;
        protected void ParseSucceedHandler(LightSingleArgEventArgs<List<T>> e)
        {
            EventHandler<LightSingleArgEventArgs<List<T>>> handler = ParseSucceed;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     检查当前的包的可用总长度
        /// </summary>
        /// <param name="node">当前数据段节点</param>
        /// <returns>返回可用的下一个消息总长度</returns>
        private unsafe int CheckBytes(CSNSegmentNode node)
        {
            int headOffset = 0;
            CSNSegmentNode nextNode;
            if (node.RemainingSize >= 4) return BitConverter.ToInt32(node.Args.Stub.Cache.Segment.Segment.Array, node.Args.Stub.Cache.Segment.UsedOffset);
            byte* head = stackalloc byte[4];
            nextNode = node;
            while (headOffset < 3)
            {
                fixed (byte* pData = &nextNode.Args.Stub.Cache.Segment.Segment.Array[nextNode.Args.Stub.Cache.Segment.UsedOffset])
                {
                    for (int i = 0; i < nextNode.RemainingSize && headOffset < 4; i++)
                        head[headOffset++] = *(pData + i);
                }
                nextNode = nextNode.Next;
                if (headOffset > 3) break;
                if (nextNode == null) return -1;
            }
            return *(int*)head;
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //give back all segments.
            while (_head != null)
            {
                //giveup current fixed stub.
                CSNChannelConst.BuffAsyncStubPool.Giveback(_head.Args.Stub);
                CSNChannelCounter.Instance.RateOfFixedBufferStubGiveback.Increment();
                _head = _head.Next;
            }
            //release the end of segment ref.
            _tail = null;
        }

        #endregion
    }
}