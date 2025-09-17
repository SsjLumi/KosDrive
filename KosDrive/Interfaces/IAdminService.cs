using KosDrive.AdminDto;
using KosDrive.Models;
using KosDrive.Services;

namespace KosDrive.Interfaces
{
    public interface IAdminService
    {
        Task<OperationResult<AdminOverviewDto>> GetOverviewAsync();
        Task<OperationResult<List<UserManagmentDto>>> GetAllUsersAsync();
        Task<OperationResult<List<DriverVerificationDto>>> GetPendingDriversAsync();
        Task<OperationResult<string>> VerifyDriverAsync(Guid userId);
        Task<OperationResult<List<LiveRideDto>>> GetLiveRidesAsync();
        Task<OperationResult<List<RideSummaryDto>>> GetRecentRidesAsync(string? driverId, DateTime? from, DateTime? to);
        Task<OperationResult<Company>> GetCompanyByIdAsync(Guid companyId);
        Task<OperationResult<List<Company?>>> GetAllCompaniesAsync();
        Task<OperationResult<string>> ApproveCompanyAsync(Guid companyId);
        Task<OperationResult<string>> UpdateCompanySubscriptionAsync(Guid companyId, SubscriptionTier newTier);
    }
}
