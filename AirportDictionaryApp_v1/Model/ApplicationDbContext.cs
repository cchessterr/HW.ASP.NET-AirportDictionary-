using Microsoft.EntityFrameworkCore;

namespace AirportDictionaryApp_v1.Model
{
    public class ApplicationDbContext : DbContext
    {
        // таблицы
        public required DbSet<Airport> Airports { get; set; }
        public required DbSet<Company> Companies { get; set; }
        public required DbSet<Country> Countries { get; set; }

        // конфигурирования
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string useConnection = config.GetSection("UseConnection").Value ?? "DefaultConnection";
            string? connectionString = config.GetConnectionString(useConnection);
            optionsBuilder.UseSqlServer(connectionString);
        }

    }
}
