//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using KJFramework.Data.ObjectDB.Controllers;
//using KJFramework.Data.ObjectDB.Helpers;
//using KJFramework.Data.ObjectDB.Structures;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace KJFramework.Data.ObjectDB.UnitTest
//{
//    [TestClass]
//    public class FileHeaderControllerUnitTest
//    {
//        #region Members

//        private static readonly string _testFile = "E:\\test1.obd";

//        #endregion

//        #region Methods
        
//        [TestMethod]
//        [Description("创建一个新DB文件测试")]
//        public void CreateNewTest()
//        {
//            if (File.Exists(_testFile)) File.Delete(_testFile);
//            FileHeaderController controller = new FileHeaderController(_testFile);
//            IndexTable table = (IndexTable) controller.GetType().GetField("_indexTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance).GetValue(controller);
//            Assert.IsNotNull(table);
//            controller.Dispose();
//        }

//        [TestMethod]
//        [Description("创建一个新DB文件, 并且插入一个新的类型令牌测试")]
//        public void CreateNewAddInsertTokenTest()
//        {
//            if (File.Exists(_testFile)) File.Delete(_testFile);
//            FileHeaderController controller = new FileHeaderController(_testFile);
//            IndexTable table = (IndexTable)controller.GetType().GetField("_indexTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance).GetValue(controller);
//            Assert.IsNotNull(table);
//            TypeToken token = controller.AddToken(typeof (Obj1).FullName);
//            controller.Commit();
//            Assert.IsNotNull(token);
//            controller.Dispose();
//        }

//        [TestMethod]
//        [Description("创建一个新DB文件, 并且插入一个新的类型令牌，关闭整个文件句柄，然后再打开的读取测试")]
//        public void GET_CreateNewAddInsertTokenTest()
//        {
//            if (File.Exists(_testFile)) File.Delete(_testFile);
//            FileHeaderController controller = new FileHeaderController(_testFile);
//            IndexTable table = (IndexTable)controller.GetType().GetField("_indexTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance).GetValue(controller);
//            Assert.IsNotNull(table);
//            TypeToken token = controller.AddToken(typeof(Obj1).FullName);
//            Assert.IsNotNull(token);
//            controller.Commit();
//            controller.Dispose();
//            //read it again.
//            controller = new FileHeaderController(_testFile);
//            IEnumerable<TypeToken> typeTokens = controller.GetTokens();
//            Assert.IsNotNull(typeTokens);
//            IList<TypeToken> tokens = new List<TypeToken>(typeTokens);
//            Assert.IsTrue(tokens.Count > 0);
//            controller.Dispose();
//        }

//        #endregion
//    }
//}