using Microsoft.EntityFrameworkCore;
using StbApp.Api.Models;

namespace StbApp.Api.Data
{
    public class StbContext : DbContext
    {
        public StbContext(DbContextOptions<StbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Demande> Demandes => Set<Demande>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Userfirstname).HasColumnName("userfirstname").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Mobile).HasColumnName("mobile").HasMaxLength(30).IsRequired();
                entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(200).IsRequired();
                entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(50).HasDefaultValue("User");

                entity.HasMany(e => e.Demandes)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(e => e.Notifications)
                      .WithOne(n => n.User)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Demande>(entity =>
            {
                entity.ToTable("demandes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RequestType).HasColumnName("request_type").HasMaxLength(100);
                entity.Property(e => e.RequestSubType).HasColumnName("request_sub_type").HasMaxLength(100);
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(50).HasDefaultValue("En attente").IsRequired();
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasMany(e => e.Notifications)
                      .WithOne(n => n.Demande)
                      .HasForeignKey(n => n.DemandeId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Message).HasColumnName("message").HasMaxLength(500).IsRequired();
                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
                entity.Property(e => e.IsRead).HasColumnName("is_read");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.DemandeId).HasColumnName("demande_id");
            });
        }
    }
}

