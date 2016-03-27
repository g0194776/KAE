using KJFramework.Attribute;
using System;

namespace ConsoleApplication1.Configurations
{
    public class ConnItem
    {
        [CustomerField("Address")]
        public String Address;
        [CustomerField("Port")]
        public int Port;
    }
}