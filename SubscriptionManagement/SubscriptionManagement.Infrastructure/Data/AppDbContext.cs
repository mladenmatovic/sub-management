using Microsoft.EntityFrameworkCore;
using SubscriptionManagement.Business.Models;
using System.Reflection.Metadata;
using System.Transactions;

namespace SubscriptionManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(t => t.Id);
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(t => t.Id);
            });

            modelBuilder.Entity<Account>()
                .HasOne(d => d.Customer)
                .WithMany(t => t.Accounts)
                .HasForeignKey(d => d.CustomerId);

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasIndex(t => t.Status);
            });

            modelBuilder.Entity<Subscription>()
                .HasOne(d => d.Account)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(d => d.AccountId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
