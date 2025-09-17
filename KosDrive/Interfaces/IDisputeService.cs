using KosDrive.DTOs;
using KosDrive.Services;

namespace KosDrive.Interfaces
{
    public interface IDisputeService
    {
        Task<OperationResult<DisputeResponseDto>> CreateDisputeAsync(string userId, CreateDisputeDto dto);
        Task<List<DisputeResponseDto>> GetAllDisputesAsync();
    }
}
