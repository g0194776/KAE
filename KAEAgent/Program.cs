using System;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Configurations.Settings;
using KJFramework.ApplicationEngine.Proxies;
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
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Run a specified KPP.")]
        public void run(KAEArgs args)
        {
            Console.WriteLine("KAE Agent [Version 1.0.10586]");
            Console.WriteLine("(c) 2016 KJFramework Community Team. Reserved all rights.");
            string etcdClusterUrl = args.csn;
            string appFilePath = args.app;
            if (string.IsNullOrEmpty(etcdClusterUrl)) throw new ArgumentException("#The remote ETCD URL you specified CANNOT be null!");
            SystemWorker.Initialize("KAEAgent", RemoteConfigurationSetting.Default, new EtcdRemoteConfigurationProxy(new Uri(etcdClusterUrl)));
            APPExecuter.Initialize();
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
    }
}
