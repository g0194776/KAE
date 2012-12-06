using System;
using System.Diagnostics;
using System.IO;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.UnitTest
{
    [ServiceContract(ServiceConcurrentType = ServiceConcurrentTypes.Concurrent)]
    public interface ITestContract
    {
        [Operation]
        void VoidCall(string content);
        [Operation]
        string BigData(string content);
        [Operation]
        string NormalCall(string content);
        [Operation]
        int[] GetBigArrayRank();
        [Operation(IsOneWay = true)]
        void OnewayCall();
    }

    public class TestContract : ITestContract
    {
        #region Implementation of ITestContract

        public void VoidCall(string content)
        {
            
        }

        public string BigData(string content)
        {
            Console.WriteLine(content);
            string c;
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (StreamReader reader = new StreamReader("D:\\assert.txt"))
            {
                c = reader.ReadToEnd();
            }
            stopwatch.Stop();
            Console.WriteLine("Read file spend time: " + stopwatch.Elapsed);
            return c;
        }

        public string NormalCall(string content)
        {
            Console.WriteLine(content);
            return "Yes!~";
        }

        public int[] GetBigArrayRank()
        {
            int[] bigArr = new int[8888];
            bigArr[8887] = 88;
            return bigArr;
        }

        public void OnewayCall()
        {
            Console.WriteLine("Oneway call successed.");
        }

        #endregion
    }
}