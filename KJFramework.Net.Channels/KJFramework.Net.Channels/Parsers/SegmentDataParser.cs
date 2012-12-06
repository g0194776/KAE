using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Events;
using KJFramework.Net.Channels.Objects;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net.Channels.Parsers
{
    /// <summary>
    ///     数据段解析器元接口
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class SegmentDataParser<T> : ISegmentDataParser<T>
    {
        #region Constructor

        /// <summary>
        ///     数据段解析器元接口
        /// </summary>
        /// <param name="protocolStack">协议栈</param>
        public SegmentDataParser(IProtocolStack<T> protocolStack)
        {
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            _protocolStack = protocolStack;
        }

        #endregion

        #region Members

        private SegmentNode _head;
        private SegmentNode _tail;
        private readonly IProtocolStack<T> _protocolStack;

        #endregion

        #region Implementation of ISegmentDataParser<T>

        /// <summary>
        ///     追加一个新的数据段
        /// </summary>
        /// <param name="args">数据段接受参数</param>
        public void Append(SegmentReceiveEventArgs args)
        {
            #region Step 1, check-on args.

            if (_head == null) _head = _tail = new SegmentNode(args);
            else
            {
                _tail.Next = new SegmentNode(args);
                _tail = _tail.Next;
            }

            #endregion

            #region Step 2, check bytes enough & pickup data.

            int msgSize;
            //RESET next node for current expression.
            SegmentNode nextNode = _head;
            List<T> msgs = new List<T>();
            //check whatever bytes can be parse.
            while (nextNode != null && (msgSize = CheckBytes(nextNode)) > 0)
            {
                //direct parse.
                if (nextNode.RemainingSize >= msgSize)
                {
                    List<T> list = _protocolStack.Parse(nextNode.Value.Segment.Segment.Array, nextNode.Value.Segment.UsedOffset, msgSize);
                    if (list != null) msgs.AddRange(list);
                    nextNode.Value.Segment.UsedBytes += msgSize;
                    ChannelCounter.Instance.RateOfDirectParse.Increment();
                    if (nextNode.RemainingSize > 0) continue;
                    //giveup current segment.
                    GlobalMemory.SegmentContainer.Giveback(nextNode.Value.Segment);
                    ChannelCounter.Instance.RateOfMemSegmentGiveback.Increment();
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
                SegmentNode childHead = (SegmentNode)nextNode.Clone();
                SegmentNode childTail = (SegmentNode)nextNode.Clone();
                SegmentNode tempNode = nextNode;
                SegmentNode lastRealNode = null;
                SegmentNode cloneLastRealNode = null;
                do
                {
                    tempNode = tempNode.Next;
                    if (tempNode == null) break;
                    remainingSize -= tempNode.RemainingSize;
                    //clone target node for child-list.
                    lastRealNode = tempNode;
                    SegmentNode cloneNode = (SegmentNode)tempNode.Clone();
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
                    System.Buffer.BlockCopy(childHead.Value.Segment.Segment.Array,
                                            childHead.Value.Segment.UsedOffset, data, dataOffset,
                                            (usedBytes = (childHead.RemainingSize > dataRemainingCount
                                                              ? dataRemainingCount
                                                              : childHead.RemainingSize)));
                    dataOffset += usedBytes;
                    dataRemainingCount -= usedBytes;
                    childHead.Value.Segment.UsedBytes += usedBytes;
                    lastUserBytes = childHead.Value.Segment.UsedBytes;
                    if (childHead.RemainingSize <= 0)
                    {
                        GlobalMemory.SegmentContainer.Giveback(childHead.Value.Segment);
                        ChannelCounter.Instance.RateOfMemSegmentGiveback.Increment();
                        childHead = childHead.Next;
                    }
                }
                if (cloneLastRealNode.RemainingSize - lastUserBytes == 0 && lastRealNode.Next == null) _head = nextNode = null;
                else if (cloneLastRealNode.RemainingSize - lastUserBytes == 0 && lastRealNode.Next != null) _head = nextNode = lastRealNode.Next;
                else
                {
                    lastRealNode.Value.Segment.UsedBytes = lastUserBytes;
                    _head = nextNode = lastRealNode;
                }
                //_head = nextNode = ((cloneLastRealNode.RemainingSize == 0 && lastRealNode.Next == null) ? null : lastRealNode);
                List<T> list1 = _protocolStack.Parse(data);
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
        private unsafe int CheckBytes(SegmentNode node)
        {
            int headOffset = 0;
            SegmentNode nextNode;
            if (node.RemainingSize >= 4) return BitConverter.ToInt32(node.Value.Segment.Segment.Array, node.Value.Segment.UsedOffset);
            byte* head = stackalloc byte[4];
            nextNode = node;
            while (headOffset < 3)
            {
                fixed (byte* pData = &nextNode.Value.Segment.Segment.Array[nextNode.Value.Segment.UsedOffset])
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
                GlobalMemory.SegmentContainer.Giveback(_head.Value.Segment);
                ChannelCounter.Instance.RateOfMemSegmentGiveback.Increment();
                _head = _head.Next;
            }
            //release the end of segment ref.
            _tail = null;
        }

        #endregion
    }
}