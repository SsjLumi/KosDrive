using KosDrive.DTOs;
using KosDrive.Models;
using KosDrive.Services;

namespace KosDrive.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<UserProfileResponse>> UpdateUserProfileAsync(string userId, UpdateUserProfileDto dto);
        Task<OperationResult<UserProfileResponse>> DeleteAsync(Guid userId);
    }

}
