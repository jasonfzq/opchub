using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpcHub.Da.Contract
{
    public class ReadCommandResult
    {
        private ReadCommandResult(
            bool succeeded,
            List<ReadItemValue> itemValues,
            List<ItemCommandFailureReason> failedItems = null)
        {
            Succeeded = succeeded;
            ItemValues = new ReadOnlyCollection<ReadItemValue>(itemValues ?? new List<ReadItemValue>());
            FailedItems = new ReadOnlyCollection<ItemCommandFailureReason>(failedItems ?? new List<ItemCommandFailureReason>());
        }

        public ReadCommandResult()
        {
        }

        public bool Succeeded { get; set; }

        public IReadOnlyCollection<ReadItemValue> ItemValues { get; set; }

        public IReadOnlyCollection<ItemCommandFailureReason> FailedItems { get; set; }

        public ReadItemValue GetItemValue(string blockName, string itemName)
        {
            string tag = $"{blockName}.{itemName}";
            return ItemValues.FirstOrDefault(iv => iv.ItemName == tag);
        }

        public static ReadCommandResult Successful(List<ReadItemValue> itemValues)
        {
            return new ReadCommandResult(true, itemValues);
        }

        public static ReadCommandResult Failed(List<ReadItemValue> itemValues, List<ItemCommandFailureReason> failedItems)
        {
            return new ReadCommandResult(false, itemValues, failedItems);
        }
    }
    
    public class ReadItemValue
    {
        public string ItemName { get; set; }

        public string ItemPath { get; set; }

        public object ClientHandle { get; set; }

        public object ServerHandle { get; set; }

        public object ItemValue { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
