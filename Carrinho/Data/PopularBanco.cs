using Carrinho.Models;
using System.Linq;

namespace Carrinho.Data
{
    public class PopularBanco
    {
        private CarrinhoContext _context;
        public PopularBanco(CarrinhoContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Produto.Any() || _context.Pedido.Any() || _context.PedidoItem.Any())
            {
                return;
            }
            Produto p1 = new Produto(1, "Celular");
            Produto p2 = new Produto(2, "Televisão");
            Produto p3 = new Produto(3, "Notebook");
            Produto p4 = new Produto(4, "Liquidificador");
            Produto p5 = new Produto(5, "Mesa de Jantar");

            Pedido ped1 = new Pedido(1, "Pedido do João");
            Pedido ped2 = new Pedido(1, "Pedido da Maria");
            Pedido ped3 = new Pedido(1, "Pedido do Carlos");

            PedidoItem pedItem1 = new PedidoItem(1, 6, 1, 1);
            PedidoItem pedItem2 = new PedidoItem(2, 1, 2, 1);
            PedidoItem pedItem3 = new PedidoItem(3, 8, 3, 1);
            PedidoItem pedItem4 = new PedidoItem(4, 4, 5, 1);

            PedidoItem pedItem8 = new PedidoItem(5, 1, 1, 2);
            PedidoItem pedItem5 = new PedidoItem(6, 7, 2, 2);            
            PedidoItem pedItem7 = new PedidoItem(7, 1, 3, 2);
            PedidoItem pedItem6 = new PedidoItem(8, 2, 4, 2);


            PedidoItem pedItem9 = new PedidoItem(9, 3, 5, 3);
            PedidoItem pedItem10 = new PedidoItem(10, 1, 3, 3);

            _context.Produto.AddRange(p1, p2, p3, p4, p5);

            _context.Pedido.AddRange(ped1, ped2, ped3);

            _context.PedidoItem.AddRange(pedItem1, pedItem2, pedItem3, pedItem4, pedItem5, pedItem6, pedItem7, pedItem8, pedItem9, pedItem10);

            _context.SaveChanges();


        }
    }
}
