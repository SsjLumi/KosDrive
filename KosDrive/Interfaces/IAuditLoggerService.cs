namespace KosDrive.Interfaces
{
    public interface IAuditLoggerService
    {
        Task LogAsync(string userId, string action, string resource, string oldValues = null, string newValues = null, string ipAddress = null);
    }
}
