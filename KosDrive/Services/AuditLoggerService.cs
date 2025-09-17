using KosDrive.Data;
using KosDrive.Interfaces;
using KosDrive.Models;

namespace KosDrive.Services
{
    public class AuditLoggerService : IAuditLoggerService
    {
        private readonly ApplicationDbContext _context;

        public AuditLoggerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task LogAsync(string userId, string action, string resource, string oldValues = null, string newValues = null, string ipAddress = null)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Resource = resource,
                OldValues = oldValues,
                NewValues = newValues,
                IPAddress = ipAddress
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
