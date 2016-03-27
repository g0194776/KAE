using System;
using KJFramework.ServiceModel.Serializers;
using System.Diagnostics;
using KJFramework.ServiceModel;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
        //    OtherMetadataSerializer serializer = new OtherMetadataSerializer();
        //    byte[] data = serializer.Serialize(true);
        //    bool value = serializer.Deserialize<bool>(data);
            Class1 class1 = new Class1();
            class1.Test();
            Console.ReadLine();
        }
    }

    [Serializable]
    public class  test
    {
        private String s1 = "sdffffffffffffffffsddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
        private String s2 = "sdffffffffffffffffsddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
        private String s3 = "sdffffffffffffffffsddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
        private String s4 = "sdffffffffffffffffsddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
        private String s5 = "sdffffffffffffffffsddddddddddddddddddddddddddddddddddddddddddddddddddddddddd";
    }
}
