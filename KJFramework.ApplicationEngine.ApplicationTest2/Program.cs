using System.IO;
using System.Net;

using KJFramework.Messages.Proxies;
using KJFramework.Messages.ValueStored.DataProcessor.Mapping;
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
            KAEHost host = new KAEHost(Path.GetFullPath(".")); 
            host.Start();
        }
    }
}
