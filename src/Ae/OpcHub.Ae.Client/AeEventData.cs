namespace OpcHub.Ae.Client
{
    public class AeEventData
    {
        public AeEventData(
            string blockName,
            string equipmentName,
            string eventCode,
            string eventCallback,
            string eventRemark)
        {
            BlockName = blockName;
            EquipmentName = equipmentName;
            EventCode = eventCode;
            EventCallback = eventCallback;
            EventRemark = eventRemark;
        }

        public string BlockName { get; }
        public string EquipmentName { get; }
        public string EventCode { get; }
        public string EventCallback { get; }
        public string EventRemark { get; }
    }
}
