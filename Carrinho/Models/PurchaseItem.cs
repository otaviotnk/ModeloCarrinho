using System.ComponentModel;

namespace Carrinho.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [DisplayName("Produto")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [DisplayName("Pedido")]
        public int PurchaseId { get; set; }       
        public Purchase Purchase { get; set; }

        public PurchaseItem(int id, int quantity, int productId, int purchaseId)
        {
            Id = id;
            Quantity = quantity;
            ProductId = productId;
            PurchaseId = purchaseId;
        }
        public PurchaseItem()
        {
        }
    }
}
