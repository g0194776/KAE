using System;
using KJFramework.ServiceModel.Core.Attributes;
using ServerTest.Contract;

namespace ServerTest
{
    [ServiceMetadataExchange("demo1")]
    public class HellowWorld : IHelloWorld
    {
        #region Implementation of IHelloWorld


        public int[] Hello(int[] text)
        {
            return new[] { 200, 600 };
        }

        public string[] TestStrings(string[] content, string s, int n)
        {
            return new[] { "aaaa", "bbbb" };
        }

        public InObject CallIntellectObject(InObject obj)
        {
            return obj;
        }

        public InObject[] CallIntellectObjectArray(InObject[] obj)
        {
            return obj;
        }

        public void Call()
        {
        }

        public void CallOneway()
        {
        }

        public string CallReturnNull()
        {
            return null;
        }

        public void ThrowException()
        {
            throw new NotImplementedException("Oops, did you get it?");
        }

        public String Hello(int x, int y, string z)
        {
            return "ÎÒÊÕµ½À²£¬¹þ¹þ£¡";
        }

        #endregion
    }
}