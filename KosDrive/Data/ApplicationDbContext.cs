using KosDrive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace KosDrive.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Company>? Companies { get; set; }
        public DbSet<Ride>? Rides { get; set; }
        public DbSet<Vehicle>? Vehicles { get; set; }
        public DbSet<FavoriteDriver>? FavoriteDrivers { get; set; }
        public DbSet<Conversation>? Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
        public DbSet<PlatformSetting> PlatformSettings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Ride>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Status)
                    .HasConversion<string>();

                entity.Property(r => r.Price)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(r => r.Rider)
                    .WithMany()
                    .HasForeignKey(r => r.RiderId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.Driver)
                    .WithMany()
                    .HasForeignKey(r => r.DriverId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.Vehicle)
                    .WithMany()
                    .HasForeignKey(r => r.VehicleId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(r => r.Ratings)
                    .WithOne(r => r.Ride)
                    .HasForeignKey(r => r.RideId);
            });

            builder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Type)
                    .HasConversion<string>();

                entity.HasOne(v => v.AssignedDriver)
                    .WithMany()
                    .HasForeignKey(v => v.DriverId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<FavoriteDriver>(entity =>
            {
                entity.HasKey(fd => new { fd.RiderId, fd.DriverId });

                entity.HasOne(fd => fd.Rider)
                    .WithMany()
                    .HasForeignKey(fd => fd.RiderId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(fd => fd.Driver)
                    .WithMany()
                    .HasForeignKey(fd => fd.DriverId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Conversation>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasMany(c => c.Messages)
                    .WithOne(m => m.Conversation)
                    .HasForeignKey(m => m.ConversationId);
            });

            builder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);
            });

            builder.Entity<Rating>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.FromUser)
                    .WithMany()
                    .HasForeignKey(r => r.FromUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.ToUser)
                    .WithMany()
                    .HasForeignKey(r => r.ToUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(r => r.Ride)
                    .WithMany(r => r.Ratings)
                    .HasForeignKey(r => r.RideId);
            });

            builder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Target)
                    .HasConversion<string>();
            });

            builder.Entity<Dispute>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Status)
                    .HasConversion<string>();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(d => d.Ride)
                    .WithMany()
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<WithdrawalRequest>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.Property(w => w.Amount)
                    .HasColumnType("decimal(10,2)");

                entity.HasOne(w => w.User)
                    .WithMany()
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<PlatformSetting>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.BaseFare)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.PricePerKm)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.PricePerMinute)
                    .HasColumnType("decimal(10,2)");

                entity.Property(p => p.SurgeMultiplier)
                    .HasColumnType("decimal(10,2)");
            });

            builder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id);
            });
        }
    }
}
