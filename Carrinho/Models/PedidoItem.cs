using System.ComponentModel;

namespace Carrinho.Models
{
    public class PedidoItem
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }

        //----------------------------------------//
        [DisplayName("Produto")]
        public int ProdutoId { get; set; }

        [DisplayName("Pedido")]
        public int PedidoId { get; set; }
        public Produto Produto { get; set; }
        public Pedido Pedido { get; set; }

        public PedidoItem(int id, int quantidade, int produtoId, int pedidoId)
        {
            Id = id;
            Quantidade = quantidade;
            ProdutoId = produtoId;
            PedidoId = pedidoId;
        }
        public PedidoItem()
        {
        }
    }
}
