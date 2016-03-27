using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Objects;
using KJFramework.Net.Transaction.ValueStored;
using KJFramework.Tracing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PowerArgs;

namespace KAELoadTesting
{
    class Program
    {
        private static Dictionary<byte, BaseValueStored> _cachedValueStoreds; 

        static void Main(string[] args)
        {
            Args.InvokeAction<CalculatorProgram>(args);
            Console.ReadLine();
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static MetadataContainer GetMetadataContainer(string file)
        {
            if (_cachedValueStoreds == null)
            {
                byte[] data = File.ReadAllBytes(file);
                string jsonStr = Encoding.UTF8.GetString(data);
                JObject obj = (JObject)JsonConvert.DeserializeObject(jsonStr);
                _cachedValueStoreds = new Dictionary<byte, BaseValueStored>();
                _cachedValueStoreds.Add(0x00, new MessageIdentityValueStored(new MessageIdentity
                {
                    ProtocolId = obj["message-identity"]["protocol-id"].ToObject<byte>(),
                    ServiceId = obj["message-identity"]["service-id"].ToObject<byte>(),
                    DetailsId = obj["message-identity"]["details-id"].ToObject<byte>()
                }));
                JArray array = (JArray)obj["value-slots"];
                foreach (JObject slot in array)
                {
                    byte id = slot["id"].ToObject<byte>();
                    string type = slot["type"].ToObject<string>();
                    string value = slot["value"].ToObject<string>();
                    _cachedValueStoreds.Add(id, GetValueStoredByType(type, value));
                }
            }
            MetadataContainer metadataContainer = new MetadataContainer();
            foreach (KeyValuePair<byte, BaseValueStored> pair in _cachedValueStoreds)
                metadataContainer.SetAttribute(pair.Key, pair.Value);
            return metadataContainer;
        }

        public static BaseValueStored GetValueStoredByType(string key, string value)
        {
            switch (key.ToLower())
            {
                case "byte": return new ByteValueStored(byte.Parse(value));
                case "short": return new Int16ValueStored(short.Parse(value));
                case "ushort": return new UInt16ValueStored(ushort.Parse(value));
                case "int": return new Int32ValueStored(int.Parse(value));
                case "uint": return new UInt32ValueStored(uint.Parse(value));
                case "long": return new Int64ValueStored(long.Parse(value));
                case "ulong": return new UInt64ValueStored(ulong.Parse(value));
                case "datetime": return new DateTimeValueStored(DateTime.Parse(value));
                case "string": return new StringValueStored(value);
            }
            return null;
        }
    }

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class CalculatorProgram
    {
        private int _totalAckCount;
        private int _totalSendCount;
        private int _totalFailedCount;
        private int _totalTimeoutCount;

        private DateTime _startTime;
        private readonly object _lockObj = new object();
        private List<RequestStub> _requests = new List<RequestStub>();
            
            
            
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Run a specified KPP.")]
        public void run(KAELoadingToolsArgs args)
        {
            ThreadPool.SetMinThreads(300, 300);
            Console.WriteLine("KAE Loading Test Tool [Version 1.0.0.0]");
            Console.WriteLine("(c) 2016 KJFramework Community Team. Reserved all rights.");
            string ip = args.ip;
            ushort port = args.port;
            EtcdRemoteConfigurationProxy configurationProxy = new EtcdRemoteConfigurationProxy(new Uri(args.etcd));
            TracingSettings.Datasource = configurationProxy.GetField("kae-system", "tracing-data-source", false);
            TracingSettings.Level = (TracingLevel)int.Parse(configurationProxy.GetField("kae-system", "tracing-level", false));
            TracingSettings.Provider = configurationProxy.GetField("kae-system", "tracing-provider", false);
            SystemWorker.Initialize("KAE Loading Test Tool", RemoteConfigurationSetting.Default, configurationProxy, proxy: new LoadingTestProxy(new IPEndPoint(IPAddress.Parse(ip), port)));
            Stack<MetadataContainer> messages = new Stack<MetadataContainer>();
            Console.Write("Performing some task... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < args.count; i++)
                {
                    messages.Push(Program.GetMetadataContainer(args.file));
                    progress.Report((double)i / 100);
                }
            }
            Console.WriteLine("Done.");
            int segmentCount = args.count/args.parallismThreadCount;
            IMessageTransactionProxy<MetadataContainer> transactionProxy = SystemWorker.GetTransactionProxy<MetadataContainer>(ProtocolTypes.Metadata);
            _startTime = DateTime.Now;
            Console.Write("Transfering TCP messages to the remote end-point...");
            ProgressBar progress2 = new ProgressBar();
            Parallel.For(0, args.parallismThreadCount, index =>
            {
                for (int i = 0; i < segmentCount; i++)
                {
                    MetadataContainer req;
                    lock (messages) req = messages.Pop();
                    RequestStub stub = new RequestStub();
                    IMessageTransaction<MetadataContainer> transaction = transactionProxy.CreateTransaction("test",
                        new Protocols
                        {
                            ProtocolId = req.GetAttributeAsType<MessageIdentity>(0x00).ProtocolId,
                            ServiceId = req.GetAttributeAsType<MessageIdentity>(0x00).ServiceId,
                            DetailsId = req.GetAttributeAsType<MessageIdentity>(0x00).DetailsId,
                        });
                    transaction.Timeout += (sender, eventArgs) =>
                    {
                        Interlocked.Increment(ref _totalTimeoutCount);
                        stub.ResponseTime = DateTime.Now;
                        stub.ErrorId = 0xFE;
                        stub.ErrorMessage = "TCP Communication Timeout.";
                    };
                    transaction.Failed += (sender, eventArgs) =>
                    {
                        Interlocked.Increment(ref _totalFailedCount);
                        stub.ResponseTime = DateTime.Now;
                        stub.ErrorId = 0xFF;
                        stub.ErrorMessage = "TCP Communication Failed.";
                    };
                    transaction.ResponseArrived += (sender, eventArgs) =>
                    {
                        MetadataContainer rsp = eventArgs.Target;
                        stub.ResponseTime = DateTime.Now;
                        stub.ErrorId = (byte)(!rsp.IsAttibuteExsits(0x0A) ? 0x00 : rsp.GetAttributeAsType<byte>(0x0A));
                        if (stub.ErrorId != 0x00) stub.ErrorMessage = rsp.GetAttributeAsType<string>(0x0B);
                        Interlocked.Increment(ref _totalAckCount);
                    };
                    transaction.SendRequest(req);
                    stub.RequestTime = DateTime.Now;
                    lock (_lockObj) _requests.Add(stub);
                    Interlocked.Increment(ref _totalSendCount);
                    progress2.Report((Math.Round(((double)_totalSendCount / args.count), 2) * 100) % 100);
                }
            });
            DateTime endTime = DateTime.Now;
            progress2.Dispose();
            Console.WriteLine("Done.");
            Console.Write("Collecting Results...");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < 10; i++)
                {
                    progress.Report((10*i) % 100);
                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("Done.");
            Console.WriteLine();
            Console.WriteLine(new string('-', 10));
            Console.WriteLine("#Total Sent Count: {0}", _totalSendCount);
            Console.WriteLine("#Total ACK Count: {0}", _totalAckCount);
            Console.WriteLine("#Total Timeout Count: {0}", _totalTimeoutCount);
            Console.WriteLine("#Total Failed Count: {0}", _totalFailedCount);
            Console.WriteLine(new string('-', 10));
            Console.WriteLine("Error-Id Statistics...");
            IEnumerable<IGrouping<byte, RequestStub>> groupBy = _requests.GroupBy(v => v.ErrorId);
            foreach (IGrouping<byte, RequestStub> grouping in groupBy)
            {
                Console.WriteLine("\tERROR: {0}, COUNT: {1}", grouping.Key.ToString("X2"), grouping.Count());
                if (grouping.Key != 0x00) Console.WriteLine("\t\tERROR REASON: {0}.", grouping.First().ErrorMessage);
            }
            Console.WriteLine(new string('-', 10));
            double currentSec = (endTime - _startTime).TotalSeconds;
            Console.WriteLine("Avg ACK Count(per second): {0}", Math.Round(_totalAckCount / currentSec, 2));
            Console.WriteLine("Avg Sent Count (per second): {0}", Math.Round(_totalSendCount / currentSec, 2));
            Console.WriteLine("Avg Timeout Count (per second): {0}", Math.Round(_totalTimeoutCount / currentSec, 2));
            Console.WriteLine("Avg Failed Count (per second): {0}", Math.Round(_totalFailedCount / currentSec, 2));
            Console.WriteLine(new string('-', 10));
            int[] ms = {1, 10, 50, 100, 200, 300, 400, 500, 600, 1000, 100000};
            for (int i = 0; i < ms.Length; i++)
            {
                int preTime = (i == 0 ? 0 : ms[i - 1]);
                Console.WriteLine("ACK time lower than {0}ms: {1}", ms[i], _requests.Count(v => v.ErrorId == 0 && (v.ResponseTime - v.RequestTime).TotalMilliseconds>= preTime && (v.ResponseTime - v.RequestTime).TotalMilliseconds <= ms[i]));
            }
        }
    }

    internal class RequestStub
    {
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public byte ErrorId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
