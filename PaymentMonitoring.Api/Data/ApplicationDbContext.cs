using Microsoft.EntityFrameworkCore;
using PaymentMonitoring.Api.Models;

namespace PaymentMonitoring.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.VerifiedByUser)
                .WithMany()
                .HasForeignKey(t => t.VerifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SubmittedByUser)
                .WithMany()
                .HasForeignKey(t => t.SubmittedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}