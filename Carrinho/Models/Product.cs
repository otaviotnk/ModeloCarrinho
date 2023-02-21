namespace Carrinho.Models
{
    public class Product
    {
        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Product(string name)
        {
            Name = name;
        }

        public Product()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }        
    }

}
