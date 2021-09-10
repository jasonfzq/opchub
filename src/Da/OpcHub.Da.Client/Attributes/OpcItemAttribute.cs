using System;

namespace OpcHub.Da.Client.Attributes
{
    public class OpcItemAttribute : Attribute
    {
        public OpcItemAttribute()
        {
        }

        public OpcItemAttribute(string itemAlias)
        {
            ItemAlias = itemAlias?.Trim();
        }

        public string ItemAlias { get; }
    }
}
