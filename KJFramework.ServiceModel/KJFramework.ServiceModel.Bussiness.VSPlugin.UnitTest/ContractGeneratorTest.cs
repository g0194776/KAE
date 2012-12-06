using System.IO;
using KJFramework.ServiceModel.Bussiness.VSPlugin.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.UnitTest
{
    [TestClass()]
    public class ContractGeneratorTest
    {
        /// <summary>
        ///A test for Generate
        ///</summary>
        [TestMethod()]
        public void GenerateTest()
        {
            string path = "D:\\";
            IRemotingService remotingService = new RemotingService("http://localhost:65300/demo1", true);
            ContractGenerator target = new ContractGenerator(path);
            target.Generate(remotingService);
            Assert.IsTrue(File.Exists(path + remotingService.Infomation.FullName + ".cs"));
        }

        [TestMethod]
        public void ArgumentGenerateTest()
        {
            ISourceFileGenerator generator = new DefaultSourceFileGenerator("GenerateTest", "D:\\");
            generator.GenerateProperty("System.Int32", "Id", 0, true);
            generator.GenerateProperty("System.String", "Username", 1, false);
            generator.GenerateProperty("System.String", "Password", 2, false);
            generator.Generate();
            Assert.IsTrue(File.Exists("D:\\GenerateTest.cs"));
        }
    }
}
