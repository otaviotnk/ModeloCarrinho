using Carrinho.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Carrinho.Data
{
    public class DatabaseSeeder
    {
        private readonly CarrinhoContext _context;
        public DatabaseSeeder(CarrinhoContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var purchasesToAdd = new List<Purchase>();

            try
            {
                purchasesToAdd.Add(new Purchase
                {
                    PurchaseName = "Pedido do João",
                    PurchaseItems = new List<PurchaseItem>()
                    {
                        new PurchaseItem
                        {
                            Quantity = new Random().Next(10),
                            Product = new Product
                            {
                                Name = "Celular"
                            }
                        },
                        new PurchaseItem
                        {
                            Quantity = new Random().Next(10),
                            Product = new Product
                            {
                                Name = "Televisão"
                            }
                        },
                        new PurchaseItem
                        {
                            Quantity = new Random().Next(10),
                            Product = new Product
                            {
                                Name = "Mesa de Jantar"
                            }
                        }
                    }
                });

                purchasesToAdd.Add(new Purchase
                {
                    PurchaseName = "Pedido do Carlos",
                    PurchaseItems = new List<PurchaseItem>()
                    {
                        new PurchaseItem
                        {
                            Quantity = new Random().Next(10),
                            Product = new Product
                            {
                                Name = "Liquidificador"
                            }
                        },
                        new PurchaseItem
                        {
                            Quantity = new Random().Next(10),
                            Product = new Product
                            {
                                Name = "Armário quatro gavetas"
                            }
                        }
                    }
                });

                purchasesToAdd.Add(new Purchase
                {
                    PurchaseName = "Pedido da MAria",
                    PurchaseItems = new List<PurchaseItem>()
                    {
                        new PurchaseItem
                        {
                            Quantity = new Random().Next(10),
                            Product = new Product
                            {
                                Name = "Notebook"
                            }
                        }
                    }
                });

                _context.Purchases.AddRange(purchasesToAdd);
                await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);

            }
            catch (Exception)
            {
                _context.Database.RollbackTransaction();
            }
        }
    }
}
