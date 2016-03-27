using System;
using KJFramework.Attribute;

namespace KJFramework.Dynamic.Template.Service.Configurations
{
    public class ServiceItem
    {
        [CustomerField("Name")]
        public String Name;
        [CustomerField("Path")]
        public String Path;
    }
}