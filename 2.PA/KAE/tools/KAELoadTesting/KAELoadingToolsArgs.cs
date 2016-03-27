using PowerArgs;

namespace KAELoadTesting
{
    public class KAELoadingToolsArgs
    {
        [ArgDescription("Be test KAE app IP Address."), ArgPosition(1), ArgRequired(PromptIfMissing = true)]
        public string ip { get; set; }
        [ArgDescription("Be test KAE app TCP port."), ArgPosition(2), ArgDefaultValue(6678)]
        public ushort port { get; set; }
        [ArgDescription("Testing count."), ArgPosition(3), ArgRequired]
        public int count { get; set; }
        [ArgDescription("The concret communication protocol used for transfering to the targeted KAE app."), ArgPosition(4), ArgRequired]
        public string file { get; set; }
        [ArgDescription("The remote ETCD address."), ArgPosition(5), ArgRequired]
        public string etcd { get; set; }
        [ArgDescription("Parallelism thread counts the tools used."), ArgPosition(6), ArgDefaultValue(1)]
        public int parallismThreadCount { get; set; }
    }
}