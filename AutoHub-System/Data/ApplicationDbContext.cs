using AutoHub_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutoHub_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<DepositePolicy> DepositePolicies { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        //added by doha
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<User>().HasKey(u => u.UserID);
            modelBuilder.Entity<DepositePolicy>().HasKey(p => p.PolicyID);
            modelBuilder.Entity<Car>().HasKey(c => c.CarID);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderID);

            // CarImage + Feature => JSON conversion
            modelBuilder.Entity<Car>()
                .Property(c => c.CarImage)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());

            modelBuilder.Entity<Car>()
                .Property(c => c.Feature)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
