using KosDrive.AdminDto;
using KosDrive.AnalyticsDto;
using KosDrive.Data;
using KosDrive.Interfaces;
using KosDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<string>> ApproveCompanyAsync(Guid companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
                return OperationResult<string>.Failure("Company not found", 404);

            if (string.IsNullOrEmpty(company.BusinessLicensePicture) || string.IsNullOrEmpty(company.RepresentantiveIdPicture))
                return OperationResult<string>.Failure("Cannot approve company. Required documents are missing.", 400);

            company.IsApproved = true;
            await _context.SaveChangesAsync();

            return OperationResult<string>.Success("Company approved.");
        }

        public async Task<OperationResult<List<Company>>> GetAllCompaniesAsync()
        {
            var companies = await _context.Companies.Include(c => c.Drivers)
                .Include(c => c.Vehicles).ToListAsync();
            return OperationResult<List<Company>>.Success(companies);
        }

        public async Task<OperationResult<List<UserManagmentDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.Select(u => new UserManagmentDto
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email!,
                    UserType = u.UserType,
                    IsBlocked = u.IsDeleted,
                    IsVerified = u.IsVerified,
                    Rating = u.Rating,
                    NumberOfRatings = u.NumberOfRatings,
                    PhoneNumber = u.PhoneNumber,
                    CreatedAt = u.CreatedAt,
                    DriversLicensePicture = u.DriversLicensePicture,
                    FrontCarLicensePlatePicture = u.FrontCarLicensePlatePicture,
                    BackCarLicensePlatePicture = u.BackCarLicensePlatePicture,
                    IdentificationCardPicture = u.IdentificationCardPicture,
                }).ToListAsync();

                return OperationResult<List<UserManagmentDto>>.Success(users);
            }catch(Exception ex)
            {
                return OperationResult<List<UserManagmentDto>>.Failure("An error occurred");
            }
        }


        public async Task<OperationResult<Company>> GetCompanyByIdAsync(Guid companyId)
        {
            var company = await _context.Companies.Include(c => c.Drivers)
                .Include(c => c.Vehicles).FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
                return OperationResult<Company>.Failure("Company not found", 404);

            return OperationResult<Company>.Success(company);
        }

        public async Task<OperationResult<List<LiveRideDto>>> GetLiveRidesAsync()
        {
            var rides = await _context.Rides
                .Where(r => r.Status == RideStatus.Accepted || r.Status == RideStatus.EnRoute)
                .Include(r => r.Driver).Include(r => r.Rider)
                .Select(r => new LiveRideDto
                {
                    RideId = r.Id,
                    DriverId = r.DriverId,
                    RiderId = r.RiderId,
                    DriverName = r.Driver.FirstName + " " + r.Driver.LastName,
                    RiderName = r.Rider.FirstName + " " + r.Rider.LastName,
                    PickupLat = r.PickupLatitude,
                    PickupLng = r.PickupLongitude,
                    DestinationLat = r.DestinationLatitude,
                    DestinationLng = r.DestinationLongitude,
                    DriverLat = r.Driver.CurrentLatitude,
                    DriverLng = r.Driver.CurrentLongitude,
                    Status = r.Status.ToString(),
                    StartedAt = r.StartedAt,
                }).ToListAsync();
            return OperationResult<List<LiveRideDto>>.Success(rides);
        }

        public async Task<OperationResult<AdminOverviewDto>> GetOverviewAsync()
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var overview = new AdminOverviewDto
            {
                TotalRides = await _context.Rides.CountAsync(),
                ActiveRides = await _context.Rides.CountAsync(r => r.Status == RideStatus.InProgress),
                TotalDrivers = await _context.Users.CountAsync(u => u.UserType == UserType.Driver),
                OnlineDrivers = await _context.Users.CountAsync(u => u.UserType == UserType.Driver && u.IsAvailable),
                TotalRiders = await _context.Users.CountAsync(u => u.UserType == UserType.Rider),
                RegisteredCompanies = await _context.Companies.CountAsync(),
                MonthlyRevenue = await _context.Rides
                    .Where(r => r.CreatedAt >= startOfMonth && r.Status == RideStatus.Completed)
                    .SumAsync(r => r.Price),
                PendingDriverApprovals = await _context.Users
                    .CountAsync(u => u.UserType == UserType.Driver && u.IsVerified == false),
                PendingCompanyApprovals = await _context.Companies
                    .CountAsync(c => !c.IsApproved)
            };

            return OperationResult<AdminOverviewDto>.Success(overview);
        }

        public async Task<OperationResult<List<DriverVerificationDto>>> GetPendingDriversAsync()
        {
            var pendingDrivers = await _context.Users
                .Where(u => u.UserType == UserType.Driver && u.IsVerified == false)
                .Select(u => new DriverVerificationDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email!,
                    DriversLicensePicture = u.DriversLicensePicture,
                    FrontCarLicensePlatePicture = u.FrontCarLicensePlatePicture,
                    BackCarLicensePlatePicture = u.BackCarLicensePlatePicture,
                    IsVerified = u.IsVerified,
                    CreatedAt = u.CreatedAt,
                }).ToListAsync();
            return OperationResult<List<DriverVerificationDto>>.Success(pendingDrivers);
        }

        public async Task<OperationResult<List<RideSummaryDto>>> GetRecentRidesAsync(string? driverId, DateTime? from, DateTime? to)
        {
            var query = _context.Rides.Include(r => r.Driver)
                .Include(r => r.Rider).AsQueryable();

            if (!string.IsNullOrWhiteSpace(driverId))
                query = query.Where(r => r.DriverId.ToString() == driverId);

            if (from.HasValue)
                query = query.Where(r => r.StartedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(r => r.CompletedAt >= to.Value);

            var results = await query.OrderByDescending(r => r.StartedAt)
                .Select(r => new RideSummaryDto
                {
                    Id = r.Id,
                    DriverName = r.Driver.FirstName + " " + r.Driver.LastName,
                    RiderName = r.Rider.FirstName + " " + r.Rider.LastName,
                    Status = r.Status.ToString(),
                    Price = r.Price,
                    StartTime = r.StartedAt,
                    EndTime = r.CompletedAt,
                }).ToListAsync();
            return OperationResult<List<RideSummaryDto>>.Success(results);
        }

        public async Task<OperationResult<string>> UpdateCompanySubscriptionAsync(Guid companyId, SubscriptionTier newTier)
        {
            try
            {
                var company = await _context.Companies.FindAsync(companyId);
                if (company == null)
                    return OperationResult<string>.Failure("Company not found", 404);

                company.Subscription = newTier;
                company.SubscriptionExpiresAt = DateTime.UtcNow.AddMonths(1);
                company.MaxAllowedVehicles = newTier switch
                {
                    SubscriptionTier.Basic => 5,
                    SubscriptionTier.Standart => 15,
                    SubscriptionTier.Premium => 50,
                    _ => 5
                };

                await _context.SaveChangesAsync();

                return OperationResult<string>.Success("Subscription updated.");
            }catch(Exception ex)
            {
                return OperationResult<string>.Failure("An error occurred");
            }
        }

        public async Task<OperationResult<string>> VerifyDriverAsync(Guid userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null || user.UserType != UserType.Driver)
                    return OperationResult<string>.Failure("Driver not found or invalid user type", 404);

                if (string.IsNullOrEmpty(user.DriversLicensePicture) ||
                    string.IsNullOrEmpty(user.FrontCarLicensePlatePicture) ||
                    string.IsNullOrEmpty(user.BackCarLicensePlatePicture))
                {
                    return OperationResult<string>.Failure("Missing required documents.", 400);
                }

                user.IsVerified = true;
                await _context.SaveChangesAsync();

                return OperationResult<string>.Success("Driver verified successfully");
            }catch (Exception ex)
            {
                return OperationResult<string>.Failure("An error occurred");
            }
        }
    }
}
