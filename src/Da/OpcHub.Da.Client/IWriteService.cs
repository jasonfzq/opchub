using System.Threading.Tasks;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Client
{
    public interface IWriteService
    {
        Task<WriteCommandResult> Write(WriteCommandRequest request);
    }
}
