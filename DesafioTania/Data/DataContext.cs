using DesafioTania.Models;

namespace DesafioTania.Data
{
    public class DataContext : DbContext
    {
        public DbSet<VeryBigSum> VeryBigSum { get; set; }
        public DbSet<DiagonalSum> DiagonalSum { get; set; }
        public DbSet<Ratio> Ratio { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) => Database.EnsureCreated();


    }
}
