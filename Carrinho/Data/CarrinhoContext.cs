using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Carrinho.Models;

namespace Carrinho.Data
{
    public class CarrinhoContext : DbContext
    {
        public CarrinhoContext (DbContextOptions<CarrinhoContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<PedidoItem> PedidoItem { get; set; }
    }
}
