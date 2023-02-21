using Carrinho.Models;
using Microsoft.EntityFrameworkCore;

namespace Carrinho.Data
{
    public class CarrinhoContext : DbContext
    {
        public CarrinhoContext (DbContextOptions<CarrinhoContext> options)
            : base(options) {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
    }
}
