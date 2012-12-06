using KJFramework.Attribute;
using System;

namespace ConsoleApplication1.Configurations
{
    public class AddressItem
    {
        [CustomerField("Ip")]
        public String Ip;
        [CustomerField("Port")]
        public int Port;
    }
}