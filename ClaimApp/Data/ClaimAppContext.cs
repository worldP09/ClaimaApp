using Microsoft.EntityFrameworkCore;
using ClaimApp.Models;

namespace ClaimApp.Data
{
    public class ClaimAppContext : DbContext
    {
        public ClaimAppContext(DbContextOptions<ClaimAppContext> options) : base(options)
        {
        }

        // DbSets for your models
        public DbSet<Claim> Claims { get; set; }
        public DbSet<LecturerApplication> LecturerApplications { get; set; }
        public DbSet<ModuleEntry> ModuleEntries { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Manager> Managers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Lecturers
            modelBuilder.Entity<Lecturer>().HasData(
                new Lecturer
                {
                    LecturerId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "Password1",
                    Phone = "0716290194",
                    Department = "Computer Science"  
                },
                new Lecturer
                {
                    LecturerId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Password = "Password2",
                    Phone = "0716290194",
                    Department = "Mathematics"  
                }
            );

            // Seed Managers
            modelBuilder.Entity<Manager>().HasData(
                new Manager { ManagerId = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", Password = "Password3" },
                new Manager { ManagerId = 2, FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com", Password = "Password4" }
            );

            // Seed Coordinators
            modelBuilder.Entity<Coordinator>().HasData(
                new Coordinator { CoordinatorId = 1, FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@example.com", Password = "Password5", ManagerId = 1 },
                new Coordinator { CoordinatorId = 2, FirstName = "Dana", LastName = "White", Email = "dana.white@example.com", Password = "Password6", ManagerId = 2 }
            );

            // Define relationships between models

            // Lecturer -> Claim (One-to-Many)
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Claims)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lecturer -> LecturerApplication (One-to-Many)
            modelBuilder.Entity<Lecturer>()
                .HasMany(l => l.Applications)
                .WithOne(a => a.Lecturer)
                .HasForeignKey(a => a.LecturerId);

            // LecturerApplication -> ModuleEntry (One-to-Many)
            modelBuilder.Entity<LecturerApplication>()
                .HasMany(a => a.Modules)
                .WithOne() 
                .HasForeignKey(m => m.ModuleEntryId);

            // Coordinator -> LecturerApplication (One-to-Many)
            modelBuilder.Entity<Coordinator>()
                .HasMany(c => c.Applications)
                .WithOne(a => a.Coordinator)
                .HasForeignKey(a => a.CoordinatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Manager -> Coordinator (One-to-Many)
            modelBuilder.Entity<Manager>()
                .HasMany(m => m.Coordinators)
                .WithOne(c => c.Manager)
                .HasForeignKey(c => c.ManagerId);

            // LecturerApplication -> Manager (One-to-Many)
            modelBuilder.Entity<LecturerApplication>()
                .HasOne(l => l.Manager)
                .WithMany(m => m.Applications)
                .HasForeignKey(l => l.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
