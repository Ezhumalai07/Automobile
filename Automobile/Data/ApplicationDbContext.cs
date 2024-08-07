using Automobile.Models;
using Microsoft.EntityFrameworkCore;
namespace Automobile.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Brand> Brand { get; set; }
    }
}
