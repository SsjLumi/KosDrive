using KosDrive.AnalyticsDto;
using KosDrive.Data;
using KosDrive.DTOs;
using KosDrive.Interfaces;
using KosDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Services
{
    public class DisputeService : IDisputeService
    {
        private readonly ApplicationDbContext _context;

        public DisputeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<DisputeResponseDto>> CreateDisputeAsync(string userId, CreateDisputeDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return OperationResult<DisputeResponseDto>.Failure("User not found");

                var dispute = new Dispute
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    CreatedByUserId = userId,
                    RideId = dto.RideId,
                };

                _context.Disputes.Add(dispute);
                await _context.SaveChangesAsync();

                return OperationResult<DisputeResponseDto>.Success(new DisputeResponseDto
                {
                    Id = dispute.Id,
                    Title = dispute.Title,
                    Description = dispute.Description,
                    CreatedAt = dispute.CreatedDate,
                    Status = dispute.Status.ToString(),
                    UserFullName = user.FirstName + " " + user.LastName,
                    RideId = dto.RideId
                });
            }catch (Exception ex)
            {
                return OperationResult<DisputeResponseDto>.Failure("An error occurred");
            }
        }

        public async Task<List<DisputeResponseDto>> GetAllDisputesAsync()
        {
            return await _context.Disputes.Include(d => d.CreatedByUser)
                .Select(d => new DisputeResponseDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    CreatedAt = d.CreatedDate,
                    Status = d.Status.ToString(),
                    UserFullName = d.CreatedByUser.FirstName + " " + d.CreatedByUser.LastName,
                    RideId = d.RideId
                }).ToListAsync();
        }
    }
}
