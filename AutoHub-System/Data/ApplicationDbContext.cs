using Microsoft.EntityFrameworkCore;
using AutoHub_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AutoHub_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DepositePolicy> DepositePolicies { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Relation One-to-One between Order && PaymentInfo  => DOHA
            modelBuilder.Entity<Order>()
                .HasOne(o => o.PaymentInfo)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.PaymentInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

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
            modelBuilder.Entity<DepositePolicy>(entity =>
            {
                entity.HasKey(p => p.PolicyID);
                entity.Property(p => p.DepositeRate)
                      .IsRequired()
                      .HasColumnType("decimal(3,2)");
                entity.Property(p => p.EffectiveDate)
                      .IsRequired();
                entity.Property(p => p.IsActive)
                      .IsRequired()
                      .HasDefaultValue(false);
            });
        }

    }
}
