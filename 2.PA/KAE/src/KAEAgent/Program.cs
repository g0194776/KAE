using System;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Proxies;
using KJFramework.Tracing;
using PowerArgs;

namespace KAEAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<CalculatorProgram>(args);
        }
    }

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class CalculatorProgram
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool help { get; set; }

        [ArgActionMethod, ArgDescription("Run a specified KPP.")]
        public void run(KAEArgs args)
        {
            Console.WriteLine("KAE Agent [Version 1.0.10586]");
            Console.WriteLine("(c) 2016 KJFramework Community Team. Reserved all rights.");
            string etcdClusterUrl = args.csn;
            string appFilePath = args.app;
            if (string.IsNullOrEmpty(etcdClusterUrl)) throw new ArgumentException("#The remote ETCD URL you specified CANNOT be null!");
            EtcdRemoteConfigurationProxy configurationProxy = new EtcdRemoteConfigurationProxy(new Uri(etcdClusterUrl));
            TracingSettings.Datasource = configurationProxy.GetField("kae-system", "tracing-data-source", false);
            TracingSettings.Level = (TracingLevel)int.Parse(configurationProxy.GetField("kae-system", "tracing-level", false));
            TracingSettings.Provider = configurationProxy.GetField("kae-system", "tracing-provider", false);
            SystemWorker.Initialize("KAEAgent", RemoteConfigurationSetting.Default, configurationProxy);
            APPExecuter.Initialize(args.businessPort, args.systemPort);
            APPExecuter.Run(etcdClusterUrl, appFilePath);
            Console.WriteLine("KAE Agent Started!");
            Console.ReadLine();
        }
    }




    public class KAEArgs
    {
        [ArgDescription("The remote CSN(ETCD) address."), ArgPosition(1), ArgRequired(PromptIfMissing = true)]
        public string csn { get; set; }
        [ArgDescription("The KPP's location which you want to run."), ArgPosition(2), ArgRequired(PromptIfMissing = true)]
        public string app { get; set; }
        [ArgDescription("A TCP port which used to opens and accepts the business requests."), ArgPosition(3), ArgDefaultValue(6678)]
        public ushort businessPort { get; set; }
        [ArgDescription("A TCP port which used to opens and accepts the system management requests."), ArgPosition(4), ArgDefaultValue(6679)]
        public ushort systemPort { get; set; }
    }
}
