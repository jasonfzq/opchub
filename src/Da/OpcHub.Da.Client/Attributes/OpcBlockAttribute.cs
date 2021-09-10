using System;

namespace OpcHub.Da.Client.Attributes
{
    public class OpcBlockAttribute : Attribute
    {
        public OpcBlockAttribute(string schema)
            : this(schema, string.Empty)
        {
        }

        public OpcBlockAttribute(string schema, string blockName)
        {
            Schema = schema.Trim();
            BlockName = blockName.Trim();
        }

        public string Schema { get; }
        public string BlockName { get; }
    }
}
