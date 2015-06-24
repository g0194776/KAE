using System;
using System.IO;
using KJFramework.ApplicationEngine.Resources.Packs;
using KJFramework.ApplicationEngine.Resources.Packs.Sections;

namespace KJFramework.ApplicationEngine.ApplicationTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * KAE Testing Processing Preparation.
             */
            string path = Path.Combine(Directory.GetCurrentDirectory(), "install-apps\\test1");
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            PackTestWithoutDelete(path);
            KAEHost host = new KAEHost(installingListFile: null, configurationProxy: null); 
            host.Start();
            Console.ReadLine();
        }


        public static void PackTestWithoutDelete(string destPath)
        {
            DateTime now = DateTime.Now;
            string fileName = Path.Combine(destPath, DateTime.Now.Ticks + ".kpp");
            Guid guid = Guid.NewGuid();
            KPPDataHead head = new KPPDataHead();
            PackageAttributeDataSection section = new PackageAttributeDataSection();
            section.SetField("PackName", "test.package");
            section.SetField("PackDescription", "test package description.");
            section.SetField("EnvironmentFlag", (byte)0x01);
            section.SetField("Version", "2.0.0");
            section.SetField("PackTime", now);
            section.SetField("ApplicationMainFileName", "KJFramework.ApplicationEngine.ApplicationTest.dll");
            section.SetField("GlobalUniqueIdentity", guid);
            section.SetField("IsCompletedEnvironment", false);
            KPPResource.Pack(@"..\..\..\KJFramework.ApplicationEngine.ApplicationTest\bin\Debug\", fileName, head, false, section);
        }
    }
}
