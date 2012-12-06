using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Counter.Instance.Initialize();
            GlobalMemory.Initialize();
            TcpHostTransportChannel tcpHostTransport = new TcpHostTransportChannel(6666);
            tcpHostTransport.ChannelCreated += tcpHostTransport_ChannelCreated;
            Console.WriteLine(tcpHostTransport.Regist());
            Console.ReadLine();
        }

        static void tcpHostTransport_ChannelCreated(object sender, LightSingleArgEventArgs<ITransportChannel> e)
        {
            IMessageTransportChannel<string> msgChannel = new MessageTransportChannel<string>((IRawTransportChannel) e.Target, new StringP());
            msgChannel.Disconnected += msgChannel_Disconnected;
            msgChannel.ReceivedMessage += msgChannel_ReceivedMessage;
            Counter.Instance.ChannelCount.Increment();
        }

        static void msgChannel_ReceivedMessage(object sender, LightSingleArgEventArgs<System.Collections.Generic.List<string>> e)
        {
            IMessageTransportChannel<string> msgChannel = (IMessageTransportChannel<string>)sender;
            Counter.Instance.RateOfReq.IncrementBy(e.Target.Count);
            Counter.Instance.TotalReq.IncrementBy(e.Target.Count);
            if(msgChannel.Send("ACK") > 0)
            {
                Counter.Instance.RateOfRsp.Increment();
                Counter.Instance.TotalRsp.Increment();
            }
        }

        static void msgChannel_Disconnected(object sender, EventArgs e)
        {
            IMessageTransportChannel<string> msgChannel = (IMessageTransportChannel<string>)sender;
            msgChannel.Disconnected -= msgChannel_Disconnected;
            msgChannel.ReceivedMessage -= msgChannel_ReceivedMessage;
            msgChannel.Dispose();
            Counter.Instance.ChannelCount.Decrement();
        }
    }
}
