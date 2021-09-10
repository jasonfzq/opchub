using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpcHub.Da.Contract
{
    public class WriteCommandResult
    {
        private WriteCommandResult(bool succeeded, List<ItemCommandFailureReason> failedItems = null)
        {
            Succeeded = succeeded;
            FailedItems = new ReadOnlyCollection<ItemCommandFailureReason>(failedItems ?? new List<ItemCommandFailureReason>());
        }

        public WriteCommandResult()
        {
        }

        public bool Succeeded { get; set; }

        public IReadOnlyCollection<ItemCommandFailureReason> FailedItems { get; set; }

        public static WriteCommandResult Successful()
        {
            return new WriteCommandResult(true);
        }

        public static WriteCommandResult Failed(List<ItemCommandFailureReason> failedItems)
        {
            return new WriteCommandResult(false, failedItems);
        }
    }
}
