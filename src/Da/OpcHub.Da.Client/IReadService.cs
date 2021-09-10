using System.Threading.Tasks;
using OpcHub.Da.Contract;

namespace OpcHub.Da.Client
{
    public interface IReadService
    {
        Task<ReadCommandResult> Read(ReadCommandRequest request);
    }
}
