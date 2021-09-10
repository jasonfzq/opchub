using System.Collections.Generic;

namespace OpcHub.Da.Contract
{
    public class ReadCommandRequest
    {
        public bool ShortPooling { get; set; }
        public List<string> Tags { get; set; }
    }
}
