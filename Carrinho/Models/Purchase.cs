using System.Collections.Generic;
using System.ComponentModel;

namespace Carrinho.Models
{
    public class Purchase
    {
        [DisplayName("Código Pedido")]
        public int Id { get; set; }

        [DisplayName("Pedido")]
        public string PurchaseName { get; set; }

        public ICollection<PurchaseItem> PurchaseItems { get; set; }

        public Purchase(int id, string purchaseName)
        {
            Id = id;
            PurchaseName = purchaseName;
        }

        public Purchase(string purchaseName)
        {
            PurchaseName = purchaseName;
        }

        public Purchase()
        {

        }
    }
}
