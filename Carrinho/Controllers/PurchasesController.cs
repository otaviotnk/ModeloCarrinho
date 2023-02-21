using Carrinho.Data;
using Carrinho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Carrinho.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly CarrinhoContext _context;

        public PurchasesController(CarrinhoContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            return View(await _context.Purchases.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Purchase purchase = await FindPurchaseItem(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Carrinho), purchase);
            }
            return View(purchase);
        }

        public IActionResult Carrinho()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Purchases, "Id", "Nome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Carrinho(Purchase purchase, PurchaseItem purchaseItem)
        {
            purchaseItem.Id = 0;

            purchaseItem.PurchaseId = purchase.Id;

            var oldPurchaseItem = await FindPurchase(purchaseItem);

            if (ModelState.IsValid)
            {
                if (await PurchaseProductExists(purchaseItem) == true)
                {
                    oldPurchaseItem.Quantity += purchaseItem.Quantity;

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), purchase);
                }

                _context.PurchaseItems.Add(purchaseItem);
                await _context.SaveChangesAsync();

                purchase = await FindPurchaseItem(purchase.Id);

                return RedirectToAction(nameof(Details), purchase);
            }

            return View(purchaseItem);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var purchaseExists = await PurchaseExists(purchase.Id);
                    if (!purchaseExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await FindPurchaseItem(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Purchases.FindAsync(id);

            _context.Purchases.Remove(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemovePurchaseItem(int id)
        {
            var purchaseItem = await _context.PurchaseItems.FirstOrDefaultAsync(p => p.Id == id);

            _context.PurchaseItems.Remove(purchaseItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = purchaseItem.PurchaseId });

        }
        private async Task<bool> PurchaseProductExists(PurchaseItem purchaseItem)
        {
            return await _context.PurchaseItems.Where(p => p.PurchaseId == purchaseItem.PurchaseId && p.ProductId == purchaseItem.ProductId).AnyAsync();
        }

        private async Task<PurchaseItem> FindPurchase(PurchaseItem purchaseItem)
        {
            return await _context.PurchaseItems.FirstOrDefaultAsync(p => p.ProductId == purchaseItem.ProductId && p.PurchaseId == purchaseItem.PurchaseId);
        }

        private async Task<Purchase> FindPurchaseItem(int? id)
        {
            return await _context.Purchases.Include(p => p.PurchaseItems)
                                           .ThenInclude(p => p.Product)
                                           .FirstOrDefaultAsync(p => p.Id == id);

        }

        private async Task<bool> PurchaseExists(int id)
        {
            return await _context.Purchases.AnyAsync(e => e.Id == id);
        }
    }
}
