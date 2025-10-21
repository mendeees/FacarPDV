using Microsoft.EntityFrameworkCore;
using dotenv.net;
using FacarPDV.Models;

namespace FacarPDV.Data
{
    public class FacarPdvContext : DbContext
    {
        public DbSet<Models.Usuarios> Usuarios { get; set; } = null!;
        public DbSet<Models.NivelUsuario> NivelUsuario { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
