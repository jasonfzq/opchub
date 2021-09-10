using OpcHub.Da.Contract;
using System;
using System.Collections.Generic;

namespace OpcHub.Da.Client.Metadata
{
    internal interface IMetaBlockCollection
    {
        (string BlockName, List<string> Tags) GetReadTags<T>(string blockName = null);
        List<WriteItemValue> GetWriteItems<T>(T block, string blockName = null);
        T ToEntity<T>(string blockName, ReadCommandResult commandResult);
    }
}
