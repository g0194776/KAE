using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Enums;
using ServerTest.Contract;

namespace ClientTest
{
    [ServiceContract(ServiceConcurrentType = ServiceConcurrentTypes.Singleton, Description="这是一个契约的描述", Name = "IHellowWorld 服务契约", Version = "0.0.0.1")]
    public interface IHelloWorld
    {
        [Operation(MethodToken = 100663300)]
        int[] Hello(int[] text);
        [Operation(MethodToken = 100663301)]
        String Hello(int x, int y, string z);
        [Operation(MethodToken = 100663302)]
        string[] TestStrings(string[] content, string s, int n);
        [Operation(MethodToken = 100663303)]
        InObject CallIntellectObject(InObject obj);
        [Operation(MethodToken = 100663304)]
        InObject[] CallIntellectObjectArray(InObject[] obj);
        [Operation(MethodToken = 100663305)]
        void Call();
        [Operation(IsOneWay = true, MethodToken = 100663306)]
        void CallOneway();
        [Operation(MethodToken = 100663307)]
        string CallReturnNull();
        [Operation(MethodToken = 100663308)]
        void ThrowException();
    }
}