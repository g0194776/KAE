using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Enums;
using KJFramework.ServiceModel.Proxy;
using ServerTest.Contract;

namespace ClientTest
{
    [ServiceContract(ServiceConcurrentType = ServiceConcurrentTypes.Singleton, Description="这是一个契约的描述", Name = "IHellowWorld 服务契约", Version = "0.0.0.1")]
    public interface IHelloWorld
    {
        [Operation(IsAsync = true, MethodToken = 3)]
        int[] Hello(int[] text);
        [Operation(MethodToken = 100663306)]
        String Hello(int x, int y, string z);
        [Operation]
        string[] TestStrings(string[] content, string s, int n);
        [Operation]
        InObject CallIntellectObject(InObject obj);
        [Operation]
        InObject[] CallIntellectObjectArray(InObject[] obj);
        [Operation]
        void Call();
        [Operation(IsOneWay = true)]
        void CallOneway();
    }
}