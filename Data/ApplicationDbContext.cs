using Microsoft.EntityFrameworkCore;
using ST10495148_Practicum.Data;

namespace ST10495148_Practicum.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<DataStreamEvent> DataStreamEvents => Set<DataStreamEvent>();
        public DbSet<PerformanceMetric> PerformanceMetrics => Set<PerformanceMetric>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DataStreamEvent>()
                .HasIndex(e => e.Timestamp)
                .HasDatabaseName("IX_DataStreamEvents_Timestamp");

            modelBuilder.Entity<DataStreamEvent>()
                .HasIndex(e => e.IsProcessed)
                .HasDatabaseName("IX_DataStreamEvents_IsProcessed");

            modelBuilder.Entity<PerformanceMetric>()
                .HasIndex(pm => pm.Timestamp)
                .HasDatabaseName("IX_PerformanceMetrics_Timestamp");
        }
    }
}
