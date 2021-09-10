namespace OpcHub.Da.Contract
{
    public class ItemCommandFailureReason
    {
        public ItemCommandFailureReason()
        {
        }

        public ItemCommandFailureReason(string itemName, string reason)
        {
            ItemName = itemName;
            Reason = reason;
        }

        public string ItemName { get; set; }

        public string Reason { get; set; }
    }
}
