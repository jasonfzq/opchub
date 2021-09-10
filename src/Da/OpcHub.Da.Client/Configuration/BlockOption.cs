using System.Collections.Generic;

namespace OpcHub.Da.Client.Configuration
{
    internal class BlockOption
    {
        public List<BlockConfig> Blocks { get; set; }
    }

    //internal class Block
    //{
    //    public string Schema { get; set; }
    //    public List<BlockItem> Items { get; set; } = new List<BlockItem>();

    //    public Block Clone()
    //    {
    //        Block block = new Block {Schema = Schema};
    //        Items.ForEach(item => block.Items.Add(item.Clone()));

    //        return block;
    //    }
    //}

    //internal class BlockItem
    //{
    //    public string Alias { get; set; }
    //    public string Name { get; set; }
    //    public int Rows { get; set; }
    //    public int Columns { get; set; }

    //    public BlockItem Clone()
    //    {
    //        return new BlockItem
    //        {
    //            Alias = Alias,
    //            Name = Name,
    //            Rows = Rows,
    //            Columns = Columns
    //        };
    //    }
    //}
}
