using System;
using Opc.Da;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Service.Hub
{
    public static class Converters
    {
        public static ItemValue ToOpcType(this WriteItemValue itemValue)
        {
            return new ItemValue
            {
                ClientHandle = Guid.NewGuid(),
                ItemPath = string.Empty,
                ItemName = itemValue.ItemName,
                Value = itemValue.Value ?? string.Empty
            };
        }

        public static Item ToOpcType(this string tag)
        {
            return new Item
            {
                ClientHandle = Guid.NewGuid(),
                ItemPath = string.Empty,
                ItemName = tag
            };
        }
    }
}
