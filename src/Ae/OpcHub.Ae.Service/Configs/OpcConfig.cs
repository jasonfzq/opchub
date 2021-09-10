using System;
using System.Collections.Generic;

namespace OpcHub.Ae.Service.Configs
{
    public class OpcConfig
    {
        public OpcConfig(
            string aeStationName,
            string aeUrl, string aeUID, string aePWD,
            List<AeEventCategoryConfig> aeCategories,
            List<string> eventSourcePrefixes,
            List<string> eventMessagePrefixes)
        {
            AeStationName = aeStationName;
            AeUrl = aeUrl;
            AeUID = aeUID;
            AePWD = aePWD;

            AeCategories = new List<AeEventCategoryConfig>(aeCategories);
            EventSourcePrefixes = new List<string>(eventSourcePrefixes);
            EventMessagePrefixes = new List<string>(eventMessagePrefixes);
        }

        public string AeStationName { get; }

        public string AeUrl { get; }

        public string AeUID { get; }

        public string AePWD { get; }

        public IReadOnlyCollection<AeEventCategoryConfig> AeCategories { get; }

        public IReadOnlyCollection<string> EventSourcePrefixes { get; }

        public IReadOnlyCollection<string> EventMessagePrefixes { get; }
    }

    public class AeEventCategoryConfig
    {
        public AeCategory Category { get; set; }

        public List<AeEventAttributeConfig> Attributes { get; set; }
    }

    public class AeEventAttributeConfig
    {
        public AeAttribute Attribute { get; set; }

        public Type DataType { get; set; }
    }

    public enum AeCategory
    {
        SequenceMessage = 104
    }

    public enum AeAttribute
    {
        StationName = 161,
        StationTimeGMT = 179
    }
}
