using OpcHub.Da.Client.Configuration;
using OpcHub.Da.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace OpcHub.Da.Client.Metadata
{
    internal class BlockItemMetadata
    {
        private readonly BlockItemConfig _blockItemConfig;
        private readonly PropertyDescriptor _propertyDescriptor;
        private readonly BlockItemType _itemType;
        private readonly List<BlockItemDescription> _itemDescriptions;

        public BlockItemMetadata(BlockItemConfig blockItemConfig, PropertyDescriptor propertyDescriptor)
        {
            _blockItemConfig = blockItemConfig;
            _propertyDescriptor = propertyDescriptor;
            _itemType = GetItemType(blockItemConfig);
            _itemDescriptions = GetItemDescriptions(blockItemConfig);
        }

        public List<string> GetReadTags(string blockName)
        {
            return _itemDescriptions.Select(itemDescription => $"{blockName}.{itemDescription.Name}").ToList();
        }

        public List<WriteItemValue> GetWriteItemValues<T>(T block, string blockName)
        {
            List<WriteItemValue> itemValues = new List<WriteItemValue>();

            object propertyValue = _propertyDescriptor.GetValue(block);
            if (propertyValue == null && _itemType != BlockItemType.NonArray)
                throw new InvalidOperationException(
                    $"The value of the array property '{_blockItemConfig.Alias}' of block '{blockName}' shouldn't be null when 'GetWriteItemValues'.");

            foreach (BlockItemDescription itemDescription in _itemDescriptions)
            {
                object value;
                if (_itemType == BlockItemType.NonArray)
                    value = propertyValue;
                else if (_itemType == BlockItemType.ArrayOf1D)
                    value = ((Array) propertyValue).GetValue(itemDescription.RowIndex - 1);
                else
                    value = ((Array) propertyValue).GetValue(itemDescription.RowIndex - 1, itemDescription.ColumnIndex - 1);

                itemValues.Add(new WriteItemValue
                {
                    ItemName = $"{blockName}.{itemDescription.Name}",
                    Value = value ?? string.Empty
                });
            }

            return itemValues;
        }

        public void SetItemValue<T>(T block, string blockName, ReadCommandResult result)
        {
            if (_itemType == BlockItemType.NonArray)
            {
                ReadItemValue item = result.GetItemValue(blockName, _itemDescriptions.First().Name);
                if (item != null && result.FailedItems.All(i => i.ItemName != item.ItemName))
                    _propertyDescriptor.SetValue(block, ConvertTo(item.ItemValue, _propertyDescriptor.PropertyType));
            }
            else if (_itemType == BlockItemType.ArrayOf1D)
            {
                Type arrayItemType = _propertyDescriptor.PropertyType.GetElementType();
                Array array = Array.CreateInstance(arrayItemType, _blockItemConfig.Rows);

                foreach (BlockItemDescription itemDescription in _itemDescriptions)
                {
                    ReadItemValue item = result.GetItemValue(blockName, itemDescription.Name);
                    if (item != null && result.FailedItems.All(i => i.ItemName != item.ItemName))
                        array.SetValue(ConvertTo(item.ItemValue, arrayItemType), itemDescription.RowIndex - 1);
                }

                _propertyDescriptor.SetValue(block, array);
            }
            else if (_itemType == BlockItemType.ArrayOf2D)
            {
                Type arrayItemType = _propertyDescriptor.PropertyType.GetElementType();
                Array array = Array.CreateInstance(arrayItemType, _blockItemConfig.Rows, _blockItemConfig.Columns);

                foreach (BlockItemDescription itemDescription in _itemDescriptions)
                {
                    ReadItemValue item = result.GetItemValue(blockName, itemDescription.Name);
                    if (item != null && result.FailedItems.All(i => i.ItemName != item.ItemName))
                        array.SetValue(ConvertTo(item.ItemValue, arrayItemType), itemDescription.RowIndex - 1, itemDescription.ColumnIndex - 1);
                }

                _propertyDescriptor.SetValue(block, array);
            }
        }

        private List<BlockItemDescription> GetItemDescriptions(BlockItemConfig blockItemConfig)
        {
            List<BlockItemDescription> itemDescriptions = new List<BlockItemDescription>();

            if (blockItemConfig.Rows == 1 && blockItemConfig.Columns == 1)
            {
                itemDescriptions.Add(new BlockItemDescription(1, 1, $"{blockItemConfig.Name}"));
            }
            else if (blockItemConfig.Rows > 1 && blockItemConfig.Columns == 1)
            {
                for (int rowIndex = 1; rowIndex <= blockItemConfig.Rows; rowIndex++)
                {
                    itemDescriptions.Add(new BlockItemDescription(rowIndex, 1, $"{blockItemConfig.Name}[{rowIndex}]"));
                }
            }
            else
            {
                for (int columnIndex = 1; columnIndex <= blockItemConfig.Columns; columnIndex++)
                {
                    for (int rowIndex = 1; rowIndex <= blockItemConfig.Rows; rowIndex++)
                    {
                        itemDescriptions.Add(new BlockItemDescription(rowIndex, columnIndex, $"{blockItemConfig.Name}[{columnIndex},{rowIndex}]"));
                    }
                }
            }

            return itemDescriptions;
        }

        private BlockItemType GetItemType(BlockItemConfig blockItemConfig)
        {
            BlockItemType itemType;
            if (blockItemConfig.Rows == 1 && blockItemConfig.Columns == 1)
                itemType = BlockItemType.NonArray;
            else if (blockItemConfig.Rows > 1 && blockItemConfig.Columns == 1)
                itemType = BlockItemType.ArrayOf1D;
            else
                itemType = BlockItemType.ArrayOf2D;

            return itemType;
        }

        private object ConvertTo(object value, Type destinationType)
        {
            if (value == null) return null;
            if (destinationType.IsInstanceOfType(value)) return value;

            string valueText = value.ToString();
            if (string.IsNullOrWhiteSpace(valueText)) return null;

            object result = null;
            switch (Type.GetTypeCode(destinationType))
            {
                case TypeCode.Decimal:
                    result = decimal.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
                case TypeCode.Double:
                    result = double.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
                case TypeCode.Single:
                    result = float.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
                case TypeCode.Int16:
                    result = short.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
                case TypeCode.Int32:
                    result = int.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
                case TypeCode.Int64:
                    result = long.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
                case TypeCode.String:
                    result = valueText;
                    break;
                case TypeCode.Object:
                    if (destinationType == typeof(decimal?))
                        result = decimal.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    else if (destinationType == typeof(double?))
                        result = double.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    else if (destinationType == typeof(short?))
                        result = short.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    else if (destinationType == typeof(int?))
                        result = int.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    else if (destinationType == typeof(long?))
                        result = long.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    else if (destinationType == typeof(float?))
                        result = float.Parse(valueText, NumberStyles.Any, CultureInfo.CurrentCulture);
                    break;
            }

            return result;
        }
    }
}