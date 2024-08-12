using Microsoft.EntityFrameworkCore;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Domain.Entities
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext() { }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) :
            base(options)
        {
            
        }
        public virtual DbSet<OrderDao> Orders { get; set; } = null!;

        public virtual DbSet<OrderDetailsDao> OrderDetails { get; set; } = null!;
        // public virtual DbSet<ProductDao> Products { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDao>(etb =>
            {
                etb.Property(e => e.OrderId).ValueGeneratedOnAdd();
            });
            
            modelBuilder.Entity<OrderDetailsDao>(etb =>
            {
                etb.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

    }
}
