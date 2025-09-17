using KosDrive.AdminDto;
using KosDrive.AnalyticsDto;
using KosDrive.Data;
using KosDrive.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Services
{
    public class PlatformSettingService : IPlatformSettingService
    {
        private readonly ApplicationDbContext _context;

        public PlatformSettingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PlatformSettingDto>> GetSettingsAsync()
        {
            try
            {
                var setting = await _context.PlatformSettings.FirstOrDefaultAsync();

                if (setting == null)
                {
                    var defaultSettings = new PlatformSettingDto
                    {
                        BaseFare = 1.0m,
                        PricePerKm = 0.5m,
                        PricePerMinute = 0.2m,
                        SurgeMultiplier = 1.0m,
                        MinimumDriverRating = 4.0,
                        IsCashPaymentEnabled = true
                    };

                    return OperationResult<PlatformSettingDto>.Success(defaultSettings);
                }

                return OperationResult<PlatformSettingDto>.Success(new PlatformSettingDto
                {
                    BaseFare = setting.BaseFare,
                    PricePerKm = setting.PricePerKm,
                    PricePerMinute = setting.PricePerMinute,
                    SurgeMultiplier = setting.SurgeMultiplier,
                    MinimumDriverRating = setting.MinimumDriverRating,
                    IsCashPaymentEnabled = setting.IsCashPaymentEnabled
                });
            }catch (Exception ex)
            {
                return OperationResult<PlatformSettingDto>.Failure("An error occurred");
            }
        }


        public async Task<OperationResult<PlatformSettingDto>> UpdateSettingsAsync(UpdatePlatformSettingDto dto)
        {
            try
            {
                var setting = await _context.PlatformSettings.FirstOrDefaultAsync();
                if (setting == null)
                    return OperationResult<PlatformSettingDto>.Failure("Settings not found", 404);

                setting.BaseFare = dto.BaseFare;
                setting.PricePerKm = dto.PricePerKm;
                setting.PricePerMinute = dto.PricePerMinute;
                setting.SurgeMultiplier = dto.SurgeMultiplier;
                setting.MinimumDriverRating = dto.MinimumDriverRating;
                setting.IsCashPaymentEnabled = dto.IsCashPaymentEnabled;
                setting.LastUpdated = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return OperationResult<PlatformSettingDto>.Success(dto, "Settings updated successfully");
            }catch (Exception ex)
            {
                return OperationResult<PlatformSettingDto>.Failure("An error occurred");
            }
        }
    }
}
