using OpcHub.Da.Client.Attributes;
using OpcHub.Da.Client.Configuration;
using OpcHub.Da.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OpcHub.Da.Client.Metadata
{
    internal class BlockMetadata
    {
        private Type _blockType;
        private string _blockTypeName;
        private string _defaultBlockName;
        private IReadOnlyList<BlockItemMetadata> _blockItems;

        public BlockMetadata()
        {
        }

        public string BlockTypeName => _blockTypeName;

        public void Configure(TypeInfo blockType, BlockOption blockOption)
        {
            _blockType = blockType;
            _blockTypeName = blockType.FullName;

            BlockConfig blockConfig = GetBlockConfig(blockType, blockOption, out string blockName);
            _defaultBlockName = blockName;
            _blockItems = new ReadOnlyCollection<BlockItemMetadata>(GetBlockItems(blockType, blockConfig));
        }

        public string GetBlockName(string blockName = null)
        {
            if (string.IsNullOrWhiteSpace(blockName) &&
                string.IsNullOrWhiteSpace(_defaultBlockName))
                throw new InvalidOperationException($"BlockName isn't specified for {_blockType.FullName}");

            blockName = string.IsNullOrWhiteSpace(blockName) ? _defaultBlockName : blockName;
            return blockName;
        }

        public List<string> GetReadTags(string blockName)
        {
            if (string.IsNullOrWhiteSpace(blockName))
                throw new ArgumentNullException($"BlockName can't be null when 'GetReadTags' for {_blockType.FullName}");

            return _blockItems.SelectMany(item => item.GetReadTags(blockName)).ToList();
        }

        public List<WriteItemValue> GetWriteItems<T>(T block, string blockName)
        {
            if (string.IsNullOrWhiteSpace(blockName))
                throw new ArgumentNullException($"BlockName can't be null 'GetWriteItems' for {_blockType.FullName}");

            return _blockItems
                .SelectMany(blockItemMetadata => blockItemMetadata.GetWriteItemValues(block, blockName))
                .ToList();
        }

        public T ToEntity<T>(string blockName, ReadCommandResult commandResult)
        {
            if (commandResult == null)
                throw new ArgumentNullException(nameof(commandResult));

            T entity = Activator.CreateInstance<T>();
            foreach (BlockItemMetadata itemMetadata in _blockItems)
            {
                itemMetadata.SetItemValue(entity, blockName, commandResult);
            }
            return entity;
        }

        private BlockConfig GetBlockConfig(Type blockType, BlockOption blockOption, out string blockName)
        {
            OpcBlockAttribute opcBlockAttribute = blockType.GetCustomAttribute<OpcBlockAttribute>();
            if (opcBlockAttribute == null)
                throw new InvalidOperationException($"OpcBlockAttribute isn't flagged on {blockType.FullName}");

            blockName = opcBlockAttribute.BlockName;
            string blockSchema = opcBlockAttribute.Schema;

            BlockConfig blockConfig = blockOption.Blocks.FirstOrDefault(b => b.Schema.ToLower() == blockSchema.ToLower());
            if (blockConfig == null)
                throw new InvalidOperationException($"The block class '{blockType.FullName}' with schema '{blockSchema}' doesn't configured in the json file.");

            return blockConfig;
        }

        private List<BlockItemMetadata> GetBlockItems(Type blockType, BlockConfig blockConfig)
        {
            List<BlockItemMetadata> blockItems = new List<BlockItemMetadata>();

            Type itemAttributeType = typeof(OpcItemAttribute);
            PropertyDescriptorCollection propertyDescriptors = TypeDescriptor.GetProperties(blockType);
            if (propertyDescriptors == null || propertyDescriptors.Count == 0)
                throw new InvalidOperationException($"OpcBlockAttribute isn't flagged on any properties of block class '{blockType.FullName}' with schema '{blockConfig.Schema}'.");

            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
            {
                OpcItemAttribute attr = propertyDescriptor.Attributes[itemAttributeType] as OpcItemAttribute;
                if (attr == null) continue;

                string itemAlias = string.IsNullOrEmpty(attr.ItemAlias) ? propertyDescriptor.Name : attr.ItemAlias;
                BlockItemConfig blockItemConfig = blockConfig.Items.FirstOrDefault(item => item.Alias.ToLower() == itemAlias.ToLower());
                if (blockItemConfig == null)
                    throw new InvalidOperationException($"Opc Item with alias '{itemAlias}' of block class '{blockType.FullName}' with schema '{blockConfig.Schema}' doesn't configured in the json file.");

                blockItems.Add(new BlockItemMetadata(blockItemConfig.Clone(), propertyDescriptor));
            }

            return blockItems;
        }
    }
}
