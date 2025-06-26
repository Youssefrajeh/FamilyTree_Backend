using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FamilyTreeAPI.Models;

namespace FamilyTreeAPI.Data
{
    public class FamilyTreeContext : IdentityDbContext<User>
    {
        public FamilyTreeContext(DbContextOptions<FamilyTreeContext> options) : base(options)
        {
        }

        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Spouse> Spouses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure FamilyMember relationships
            builder.Entity<FamilyMember>()
                .HasOne(fm => fm.Father)
                .WithMany(f => f.ChildrenAsFather)
                .HasForeignKey(fm => fm.FatherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FamilyMember>()
                .HasOne(fm => fm.Mother)
                .WithMany(m => m.ChildrenAsMother)
                .HasForeignKey(fm => fm.MotherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FamilyMember>()
                .HasOne(fm => fm.User)
                .WithMany(u => u.FamilyMembers)
                .HasForeignKey(fm => fm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Spouse relationships
            builder.Entity<Spouse>()
                .HasOne(s => s.FirstMember)
                .WithMany(fm => fm.SpousesAsFirstMember)
                .HasForeignKey(s => s.FirstMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Spouse>()
                .HasOne(s => s.SecondMember)
                .WithMany(fm => fm.SpousesAsSecondMember)
                .HasForeignKey(s => s.SecondMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure spouse relationships are unique
            builder.Entity<Spouse>()
                .HasIndex(s => new { s.FirstMemberId, s.SecondMemberId })
                .IsUnique();

            // Configure indexes for better performance
            builder.Entity<FamilyMember>()
                .HasIndex(fm => fm.UserId);

            builder.Entity<FamilyMember>()
                .HasIndex(fm => new { fm.FirstName, fm.LastName });

            builder.Entity<FamilyMember>()
                .HasIndex(fm => fm.DateOfBirth);

            // Configure table names
            builder.Entity<User>().ToTable("Users");
            builder.Entity<FamilyMember>().ToTable("FamilyMembers");
            builder.Entity<Spouse>().ToTable("Spouses");
        }
    }
}