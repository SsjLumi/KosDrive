using KosDrive.Data;
using KosDrive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KosDrive.Controllers
{
    public class WithdrawalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WithdrawalController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("request-withdrawal")]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> RequestWithdrawal([FromBody] decimal amount)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var request = new WithdrawalRequest
            {
                UserId = userId,
                Amount = amount
            };

            _context.WithdrawalRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Withdrawal Requested" });
        }

        [HttpGet("pending-withdrawals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingWithdrawals()
        {
            var pending = await _context.WithdrawalRequests
                .Where(w => w.Status == "Pending").ToListAsync();

            return Ok(pending);
        }
    }
}
