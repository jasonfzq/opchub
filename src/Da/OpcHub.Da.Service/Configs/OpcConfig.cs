namespace OpcHub.Da.Service.Configs
{
    public class OpcConfig
    {
        public OpcConfig(
            string daStationName,
            string daUrl,
            string daUID,
            string daPWD,
            int maxItemsReadPerBatch,
            int maxItemsWritePerBatch)
        {
            DaStationName = daStationName;
            DaUrl = daUrl;
            DaUID = daUID;
            DaPWD = daPWD;
            MaxItemsReadPerBatch = maxItemsReadPerBatch;
            MaxItemsWritePerBatch = maxItemsWritePerBatch;
        }

        public string DaStationName { get; }
        public string DaUrl { get; }
        public string DaUID { get; }
        public string DaPWD { get; }
        public int MaxItemsReadPerBatch { get; }
        public int MaxItemsWritePerBatch { get; }
    }
}
