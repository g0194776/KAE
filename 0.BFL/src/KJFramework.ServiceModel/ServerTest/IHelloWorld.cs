using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Enums;
using ServerTest.Contract;

namespace ServerTest
{
    [ServiceContract(ServiceConcurrentType = ServiceConcurrentTypes.Concurrent, Description="这是一个契约的描述", Name = "IHellowWorld 服务契约", Version = "0.0.0.1")]
    public interface IHelloWorld
    {
        [Operation]
        int[] Hello(int[] text);
        [Operation]
        String Hello(int x, int y, string z);
        [Operation]
        string[] TestStrings(string[] content,string s, int n);
        [Operation]
        InObject CallIntellectObject(InObject obj);
        [Operation]
        InObject[] CallIntellectObjectArray(InObject[] obj);
        [Operation]
        void Call();
        [Operation(IsOneWay = true)]
        void CallOneway();
        [Operation]
        string CallReturnNull();
        [Operation]
        void ThrowException();
    }
}