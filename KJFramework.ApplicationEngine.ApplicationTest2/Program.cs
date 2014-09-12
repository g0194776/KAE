using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using KJFramework.EventArgs;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.Proxies;
using KJFramework.Messages.ValueStored;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.ProtocolStack;
using KJFramework.Net.Transaction.ValueStored;

namespace KJFramework.ApplicationEngine.ApplicationTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtensionTypeMapping.Regist(typeof(MessageIdentityValueStored));
            ExtensionTypeMapping.Regist(typeof(TransactionIdentityValueStored));
            MemoryAllotter.Instance.Initialize();
            KAEHost host = new KAEHost(Path.GetFullPath("."), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6611)); 
            host.Start();
        }
    }
}
