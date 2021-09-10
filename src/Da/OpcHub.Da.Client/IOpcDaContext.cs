using OpcHub.Da.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpcHub.Da.Client
{
    public interface IOpcDaContext
    {
        /// <summary>
        /// If short pooling is true, the package will record the logs in a specific folder. Otherwise, the logs will be record in the normal "Read" or "Write" folder.
        /// </summary>
        bool ShortPooling { get; set; }
        
        Task<ReadCommandResult> Read(string tag);
        Task<ReadCommandResult> Read(List<string> tags);
        Task<T> Read<T>(string blockName = null);
        Task<List<(string BlockName, T BlockData)>> Read<T>(List<string> blockNames);

        Task<WriteCommandResult> Write(WriteItemValue itemValue);
        Task<WriteCommandResult> Write(List<WriteItemValue> itemValues);
        Task<WriteCommandResult> Write<T>(T block);
        Task<WriteCommandResult> Write<T>(T block, string blockName);
    }
}
