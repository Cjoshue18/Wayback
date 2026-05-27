using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    public class FashionShopContext : DbContext
    {
        public FashionShopContext(DbContextOptions<FashionShopContext> options) : base(options)
        {
        }
        public DbSet<Models.Clientes> Clientes { get; set; }
    }
}
