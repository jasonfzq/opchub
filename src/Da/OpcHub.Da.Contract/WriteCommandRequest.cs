using System.Collections.Generic;

namespace OpcHub.Da.Contract
{
    public class WriteCommandRequest
    {
        public bool ShortPolling { get; set; }
        public List<WriteItemValue> ItemValues { get; set; }
    }

    public class WriteItemValue
    {
        public string ItemName { get; set; }

        public object Value { get; set; }
    }
}
