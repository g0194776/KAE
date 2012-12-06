using KJFramework.Attribute;
using System;

namespace ConsoleApplication1.Configurations
{
    public class SetItem
    {
        [CustomerField("ab")]
        public String ab;
        [CustomerField("cd")]
        public String cd;
    }
}