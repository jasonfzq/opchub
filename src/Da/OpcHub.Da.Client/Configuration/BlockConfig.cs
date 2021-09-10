using System.Collections.Generic;

namespace OpcHub.Da.Client.Configuration
{
    internal class BlockConfig
    {
        public string Schema { get; set; }
        public List<BlockItemConfig> Items { get; set; } = new List<BlockItemConfig>();

        public BlockConfig Clone()
        {
            BlockConfig block = new BlockConfig { Schema = Schema };
            Items.ForEach(item => block.Items.Add(item.Clone()));

            return block;
        }
    }
}
