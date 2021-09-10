using OpcHub.Da.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpcHub.Da.Client.Metadata
{
    internal class BlockMetadataCollection : IMetaBlockCollection
    {
        private readonly ReadOnlyCollection<BlockMetadata> _metaBlocks;

        public BlockMetadataCollection(List<BlockMetadata> metaBlocks)
        {
            _metaBlocks = new ReadOnlyCollection<BlockMetadata>(metaBlocks);
        }

        public (string BlockName, List<string> Tags) GetReadTags<T>(string blockName = null)
        {
            BlockMetadata blockMetadata = GetBlockMetadata<T>();
            blockName = blockMetadata.GetBlockName(blockName);

            return (blockName, blockMetadata.GetReadTags(blockName));
        }

        public List<WriteItemValue> GetWriteItems<T>(T block, string blockName = null)
        {
            BlockMetadata blockMetadata = GetBlockMetadata<T>();
            blockName = blockMetadata.GetBlockName(blockName);

            return blockMetadata.GetWriteItems(block, blockName);
        }

        public T ToEntity<T>(string blockName, ReadCommandResult commandResult)
        {
            BlockMetadata blockMetadata = GetBlockMetadata<T>();
            return blockMetadata.ToEntity<T>(blockName, commandResult);
        }

        private BlockMetadata GetBlockMetadata<T>()
        {
            Type blockType = typeof(T);
            BlockMetadata blockMetadata = _metaBlocks.FirstOrDefault(b => b.BlockTypeName == blockType.FullName);
            if (blockMetadata == null)
                throw new InvalidOperationException($"'{blockType.FullName}' is an invalid, no block metadata can be found");

            return blockMetadata;
        }
    }
}
