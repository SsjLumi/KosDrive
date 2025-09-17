using KosDrive.AdminDto;
using KosDrive.Interfaces;
using KosDrive.Models;
using KosDrive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KosDrive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("overview")]
        public async Task<ActionResult<OperationResult<AdminOverviewDto>>> GetOverview()
        {
            var result = await _adminService.GetOverviewAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("users")]
        public async Task<ActionResult<OperationResult<List<User>>>> GetAllUsers()
        {
            var result = await _adminService.GetAllUsersAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("pending-drivers")]
        public async Task<ActionResult<OperationResult<List<User>>>> GetDriversPending()
        {
            var result = await _adminService.GetPendingDriversAsync();
            return StatusCode(result.Status, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("verify-driver/{userId}")]
        public async Task<ActionResult<OperationResult<bool>>> VerifyDriver(Guid userId)
        {
            var result = await _adminService.VerifyDriverAsync(userId);
            return StatusCode(result.Status, result);
        }

        [HttpGet("live-rides")]
        public async Task<ActionResult<OperationResult<List<Ride>>>> GetLiveRides()
        {
            var result = await _adminService.GetLiveRidesAsync();
            return StatusCode(result.Status, result);
        }

        [HttpGet("recent-rides")]
        public async Task<ActionResult<OperationResult<List<Ride>>>> GetRecentRides(
            string? driverId = null, DateTime? from = null, DateTime? to = null)
        {
            var result = await _adminService.GetRecentRidesAsync(driverId, from, to);
            return StatusCode(result.Status, result);
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<OperationResult<Company>>> GetCompany(Guid companyId)
        {
            var result = await _adminService.GetCompanyByIdAsync(companyId);
            return StatusCode(result.Status, result);
        }

        [HttpGet("all-companies")]
        public async Task<ActionResult<OperationResult<List<Company>>>> GetAllCompanies()
        {
            var result = await _adminService.GetAllCompaniesAsync();
            return StatusCode(result.Status, result);
        }

        [HttpPost("approve/{companyId}")]
        public async Task<ActionResult<OperationResult<bool>>> ApproveCompany(Guid companyId)
        {
            var result = await _adminService.ApproveCompanyAsync(companyId);
            return StatusCode(result.Status, result);
        }

        [HttpPost("update-subscription/{companyId}")]
        public async Task<ActionResult<OperationResult<bool>>> UpdateSubscription(
            Guid companyId, [FromBody] SubscriptionTier newTier)
        {
            var result = await _adminService.UpdateCompanySubscriptionAsync(companyId, newTier);
            return StatusCode(result.Status, result);
        }
    }
}