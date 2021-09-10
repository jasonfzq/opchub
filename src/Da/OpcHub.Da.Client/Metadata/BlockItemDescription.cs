namespace OpcHub.Da.Client.Metadata
{
    internal class BlockItemDescription
    {
        public BlockItemDescription(int rowIndex, int columnIndex, string itemName)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Name = itemName;
        }

        public int RowIndex { get; }
        public int ColumnIndex { get; }
        public string Name { get; }
    }
}