using System;
using System.IO;
using System.Reflection;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.ApplicationEngine.Resources.Packs;
using KJFramework.ApplicationEngine.Resources.Packs.Sections;
using PowerArgs;

namespace KPPBuilder
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

        [ArgActionMethod, ArgDescription("Entirely builds a KAE app from given path.")]
        public void build(KPPArgs args)
        {
            DateTime now = DateTime.Now;
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            string outVersion;
            section.SetField("ApplicationMainFileName", FindEntryPoint(args.path, out outVersion));
            section.SetField("PackName", args.name);
            section.SetField("PackDescription", args.description);
            section.SetField("EnvironmentFlag", args.environmentFlag);
            section.SetField("Version", outVersion);
            section.SetField("PackTime", now);
            section.SetField("GlobalUniqueIdentity", guid);
            section.SetField("IsCompletedEnvironment", args.isCompletedEnvironment);
            string realVersion = (string.IsNullOrEmpty(args.version) ? outVersion : args.version);
            string fileName = Path.Combine(args.outputPath, args.name + "-" + realVersion + ".kpp");
            KPPResource.Pack(args.path, fileName, head, false, section);
        }

        private string FindEntryPoint(string path, out string version)
        {
            version = string.Empty;
            foreach (string file in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                Assembly assembly = Assembly.Load(File.ReadAllBytes(file));
                Type[] types = assembly.GetTypes();
                if (types.Length == 0) return null;
                Type targetAppType = null;
                foreach (Type type in types)
                {
                    try
                    {
                        //找到入口点
                        if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(MetadataKAEProcessor)))
                        {
                            //version = FileVersionInfo.GetVersionInfo(file).FileVersion;
                            version = type.Assembly.GetName().Version.ToString();
                            return Path.GetFileName(file);
                        }
                    }
                    catch (ReflectionTypeLoadException) { }
                }
            }
            throw new EntryPointNotFoundException();
        }
    }

    public class KPPArgs
    {
        [ArgRequired, ArgDescription("The dest path where's your KPP original files stores."), ArgPosition(1)]
        public string path { get; set; }
        [ArgRequired, ArgDescription("KPP's package name."), ArgPosition(2)]
        public string name { get; set; }
        [ArgRequired, ArgDescription("KPP's package description."), ArgPosition(3), ArgDefaultValue("none.")]
        public string description { get; set; }
        [ArgRequired, ArgDescription("KPP's Env flag"), ArgPosition(4), ArgDefaultValue(0x01)]
        public byte environmentFlag { get; set; }
        [ArgDescription("KPP assembly file's version, it be the DLL's image version by the default."), ArgPosition(5)]
        public string version { get; set; }
        [ArgRequired, ArgDescription("Properly mark this value for the better KAE runtime usages of memory."), ArgPosition(6), ArgDefaultValue("false")]
        public bool isCompletedEnvironment { get; set; }
        [ArgRequired, ArgDescription("The output KPP's storage path."), ArgPosition(7), ArgDefaultValue(".")]
        public string outputPath { get; set; }
    }

}
