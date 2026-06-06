using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Context
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members => Set<Member>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<GymClass> GymClasses => Set<GymClass>();
        public DbSet<Membership> Memberships => Set<Membership>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Member Configuration ──
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(m => m.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(m => m.Email)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(m => m.Phone)
                      .HasMaxLength(20);
                entity.Property(m => m.BirthDate)
                      .IsRequired();
                entity.Property(m => m.JoinDate)
                      .IsRequired();
                entity.Property(m => m.CreatedAt)
                      .IsRequired();
                entity.Property(m => m.UpdatedAt)
                      .IsRequired(false);

                // Email must be unique across members
                entity.HasIndex(m => m.Email)
                      .IsUnique();
            });

            // ── Trainer Configuration ──
            modelBuilder.Entity<Trainer>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(t => t.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(t => t.Email)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(t => t.Phone)
                      .HasMaxLength(20);
                entity.Property(t => t.Specialty)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);

                // Email must be unique across trainers
                entity.HasIndex(t => t.Email)
                      .IsUnique();
            });

            // ── GymClass Configuration ──
            modelBuilder.Entity<GymClass>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(g => g.Description)
                      .HasMaxLength(300);
                entity.Property(g => g.Capacity)
                      .IsRequired();
                entity.Property(g => g.ScheduledAt)
                      .IsRequired();
                entity.Property(g => g.Status)
                      .IsRequired();
                entity.Property(g => g.CreatedAt)
                      .IsRequired();
                entity.Property(g => g.UpdatedAt)
                      .IsRequired(false);

                // 1:N with Trainer — deleting trainer blocked if has classes
                entity.HasOne(g => g.Trainer)
                      .WithMany(t => t.GymClasses)
                      .HasForeignKey(g => g.TrainerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Membership Configuration ──
            modelBuilder.Entity<Membership>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Type)
                      .IsRequired();
                entity.Property(m => m.StartDate)
                      .IsRequired();
                entity.Property(m => m.EndDate)
                      .IsRequired();
                entity.Property(m => m.IsActive)
                      .IsRequired();
                entity.Property(m => m.CreatedAt)
                      .IsRequired();
                entity.Property(m => m.UpdatedAt)
                      .IsRequired(false);

                // 1:N with Member — deleting member cascades memberships
                entity.HasOne(m => m.Member)
                      .WithMany(mb => mb.Memberships)
                      .HasForeignKey(m => m.MemberId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Enrollment Configuration ──
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EnrolledAt)
                      .IsRequired();
                entity.Property(e => e.CreatedAt)
                      .IsRequired();
                entity.Property(e => e.UpdatedAt)
                      .IsRequired(false);

                // FK to Member — cascade delete
                entity.HasOne(e => e.Member)
                      .WithMany(m => m.Enrollments)
                      .HasForeignKey(e => e.MemberId)
                      .OnDelete(DeleteBehavior.Cascade);

                // FK to GymClass — cascade delete
                entity.HasOne(e => e.GymClass)
                      .WithMany(g => g.Enrollments)
                      .HasForeignKey(e => e.GymClassId)
                      .OnDelete(DeleteBehavior.Cascade);

                // A member can only enroll once per class
                entity.HasIndex(e => new { e.MemberId, e.GymClassId })
                      .IsUnique();
            });
        }
    }
}