using KosDrive.Data;
using KosDrive.DTOs;
using KosDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace KosDrive.Services
{
    public class RatingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public RatingService(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task<bool> AddRatingAsync(CreateRatingDto dto)
        {
            var fromUserId = _http.HttpContext.User.FindFirst("id")?.Value;
            if (fromUserId == null || fromUserId.Equals(dto.ToUserId) || dto.Stars < 1 || dto.Stars > 5)
                return false;

            var rating = new Rating
            {
                FromUserId = Guid.Parse(fromUserId),
                ToUserId = dto.ToUserId,
                Stars = dto.Stars,
                Comment = dto.Comment,
                RideId = dto.RideId,
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<double> GetAverageRatingAsync(string userId)
        {
            return await _context.Ratings
                .Where(r => r.ToUserId.Equals(userId))
                .AverageAsync(r => (double?)r.Stars) ?? 0;
        }

        public async Task<List<Rating>> GetUserReviewsAsync(string userId)
        {
            return await _context.Ratings
                .Where(r => r.ToUserId.Equals(userId))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
