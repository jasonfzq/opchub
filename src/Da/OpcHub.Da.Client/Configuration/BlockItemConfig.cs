namespace OpcHub.Da.Client.Configuration
{
    internal class BlockItemConfig
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public BlockItemConfig Clone()
        {
            return new BlockItemConfig
            {
                Alias = Alias,
                Name = Name,
                Rows = Rows,
                Columns = Columns
            };
        }
    }
}
