using System.Collections.Generic;
using KJFramework.Attribute;
using KJFramework.Configurations;
using KJFramework.Configurations.Items;

namespace ConsoleApplication1.Configurations
{
    [CustomerSection("Connections")]
    public class ConnectionsSection : CustomerSection<ConnectionsSection>
    {
        [CustomerField("Settings", IsList = true, ElementName = "Set", ElementType = typeof(SetItem))]
        public List<SetItem> SetItem;
        [CustomerField("Addresses", IsList = true, ElementName = "Address", ElementType = typeof(AddressItem))]
        public List<AddressItem> AddressItem;
        [CustomerField("Conn")]
        public ConnItem Item;
    }
}