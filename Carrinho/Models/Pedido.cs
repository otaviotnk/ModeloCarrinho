using System.Collections.Generic;
using System.ComponentModel;

namespace Carrinho.Models
{
    public class Pedido
    {
        [DisplayName("Código Pedido")]
        public int Id { get; set; }

        [DisplayName("Pedido")]
        public string NomePedido { get; set; }

        public ICollection<PedidoItem> PedidosItens { get; set; }

        public Pedido(int id, string nomePedido)
        {
            Id = id;
            NomePedido = nomePedido;
        }
        public Pedido()
        {
        }
    }
}
