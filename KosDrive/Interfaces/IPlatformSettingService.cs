using KosDrive.AdminDto;
using KosDrive.Services;

namespace KosDrive.Interfaces
{
    public interface IPlatformSettingService
    {
        Task<OperationResult<PlatformSettingDto>> GetSettingsAsync();
        Task<OperationResult<PlatformSettingDto>> UpdateSettingsAsync(UpdatePlatformSettingDto dto);
    }

}
