using Microsoft.EntityFrameworkCore;
using Stb.Api.Models;

namespace Stb.Api.Data;

public class StbContext : DbContext
{
    public StbContext(DbContextOptions<StbContext> options) : base(options) { }

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
        });

        modelBuilder.Entity<Demande>(entity =>
        {
            entity.ToTable("demandes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RequestType).HasColumnName("request_type").HasMaxLength(100);
            entity.Property(e => e.RequestSubType).HasColumnName("request_sub_type").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.CreationDate)
                  .HasColumnName("creation_date")
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAdd();
            entity.Property(e => e.Status)
                  .HasColumnName("status")
                  .HasMaxLength(50)
                  .HasDefaultValue("En attente");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Demandes)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Message).HasColumnName("message").HasMaxLength(500).IsRequired();
            entity.Property(e => e.CreationDate)
                  .HasColumnName("creation_date")
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAdd();
            entity.Property(e => e.IsRead).HasColumnName("is_read").HasDefaultValue(false);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DemandeId).HasColumnName("demande_id");

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Demande)
                  .WithMany(d => d.Notifications)
                  .HasForeignKey(e => e.DemandeId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
    }
}

