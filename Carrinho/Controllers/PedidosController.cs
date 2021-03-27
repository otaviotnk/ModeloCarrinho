using Carrinho.Data;
using Carrinho.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Carrinho.Controllers
{
    public class PedidosController : Controller
    {
        private readonly CarrinhoContext _context;

        public PedidosController(CarrinhoContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pedido.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Pedido pedido = await ObterProdutosDoPedido(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }        

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Carrinho), pedido);
            }
            return View(pedido);
        }

        public IActionResult Carrinho()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Carrinho(Pedido pedido, PedidoItem pedidoItem)
        {
            pedidoItem.Id = 0;

            pedidoItem.PedidoId = pedido.Id;

            var pedidoItemNoBanco = BuscaPedido(pedidoItem);

            if (ModelState.IsValid)
            {                
                if (ExisteProdutoPedido(pedidoItem) == true)
                {
                    pedidoItemNoBanco.Quantidade += pedidoItem.Quantidade;

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), pedido);
                }

                _context.PedidoItem.Add(pedidoItem);
                await _context.SaveChangesAsync();                

                pedido = await ObterProdutosDoPedido(pedido.Id);

                return RedirectToAction(nameof(Details), pedido);
            }

            return View(pedidoItem);
        }        

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await ObterProdutosDoPedido(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        
        public async Task<IActionResult> DeletarItemCarrinho(int id)
        {
            var pedidoItem = _context.PedidoItem.FirstOrDefault(p => p.Id == id);

            _context.PedidoItem.Remove(pedidoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = pedidoItem.PedidoId });

        }
        private bool ExisteProdutoPedido(PedidoItem pedidoItem)
        {
            return _context.PedidoItem.Where(p => p.PedidoId == pedidoItem.PedidoId && p.ProdutoId == pedidoItem.ProdutoId).Any();
        }

        private PedidoItem BuscaPedido(PedidoItem pedidoItem)
        {
            return _context.PedidoItem.Where(p => p.ProdutoId == pedidoItem.ProdutoId)
                                      .FirstOrDefault(p => p.PedidoId == pedidoItem.PedidoId);
        }

        private async Task<Pedido> ObterProdutosDoPedido(int? id)
        {
            return await _context.Pedido.Include(p => p.PedidosItens)
                                        .ThenInclude(p => p.Produto)
                                        .FirstOrDefaultAsync(p => p.Id == id);

        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
