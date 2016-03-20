using System;
using System.IO;
using System.Reflection;
using KJFramework.ApplicationEngine;
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
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Entirely builds a KAE app from given path.")]
        public void build(KPPArgs args)
        {
            DateTime now = DateTime.Now;
            string fileName = Path.Combine(args.outputPath, args.name + "_" + DateTime.Now.Ticks + ".kpp");
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", args.name);
            section.SetField("PackDescription", args.description);
            section.SetField("EnvironmentFlag", args.environmentFlag);
            section.SetField("Version", args.version);
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", FindEntryPoint(args.path));
            section.SetField("GlobalUniqueIdentity", guid);
            section.SetField("IsCompletedEnvironment", args.isCompletedEnvironment);
            KPPResource.Pack(args.path, fileName, head, false, section);
        }

        private string FindEntryPoint(string path)
        {
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
        [ArgRequired, ArgDescription("KPP's version"), ArgPosition(5), ArgDefaultValue("0.0.0.1")]
        public string version { get; set; }
        [ArgRequired, ArgDescription("Properly mark this value for the better KAE runtime usages of memory."), ArgPosition(6), ArgDefaultValue("false")]
        public bool isCompletedEnvironment { get; set; }
        [ArgRequired, ArgDescription("The output KPP's storage path."), ArgPosition(7), ArgDefaultValue(".")]
        public string outputPath { get; set; }
    }

}
