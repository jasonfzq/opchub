using System.Collections.Generic;

namespace OpcHub.Ae.Client.Configuration
{
    internal class AeBlockConfig
    {
        /// <summary>
        /// The configured opc block name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The configured equipment code which mapped to the opc block 
        /// </summary>
        public string Equipment { get; set; }

        /// <summary>
        /// The configured events of the opc block
        /// </summary>
        public List<AeBlockEventConfig> Events { get; set; } = new List<AeBlockEventConfig>();
    }
}
