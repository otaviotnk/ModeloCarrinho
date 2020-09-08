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
            //######-Aqui tá recebendo uma lista de produtos no pedido, tá certo-######\\
            var pedido  = await _context.Pedido.Include(p => p.PedidosItens)
                                                .Include("PedidosItens.Produto")
                                                .FirstOrDefaultAsync(p => p.Id == id);

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
                
                return RedirectToAction(nameof(CriarCarrinho), pedido);
            }
            return View(pedido);
        }

        public IActionResult CriarCarrinho()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarCarrinho(Pedido pedido, PedidoItem pedidoItem)
        {
            //Estava setando um Id no id do pedidoItem, resolvi assim por enquanto(Autoincrement)
            pedidoItem.Id = 0;
            
            pedidoItem.PedidoId = pedido.Id;

            var pedidoItemNoBanco = _context.PedidoItem.FirstOrDefault(p => p.PedidoId == pedidoItem.PedidoId);

            if (ModelState.IsValid)
            {
                //Verifica se já existe o produto no Carrinho, se já tiver, somente acrescenta a quantidade e não um novo produto.
                if (pedidoItemNoBanco != null)
                {                    
                    if (pedidoItem.ProdutoId == pedidoItemNoBanco.ProdutoId)
                    {                        
                        pedidoItemNoBanco.Quantidade += pedidoItem.Quantidade;

                        _context.PedidoItem.Update(pedidoItemNoBanco);
                        await _context.SaveChangesAsync();
                        
                        return RedirectToAction(nameof(Details), pedido);
                    }
                }
                _context.PedidoItem.Add(pedidoItem);
                await _context.SaveChangesAsync();

                //Aqui, antes de enviar quando recarrega a página, o "Pedido" e "Produtos" recebe os itens para poder exibir
                pedido = await _context.Pedido.Include(p => p.PedidosItens)
                                        .Include("PedidosItens.Produto")
                                        .FirstOrDefaultAsync(m => m.Id == pedido.Id);

                return RedirectToAction(nameof(Details),pedido);
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
            //Incluindo os dados do PedidoItem e objeto Produto
            var pedido = await _context.Pedido.Include(p => p.PedidosItens)
                                                .Include("PedidosItens.Produto")
                                                .FirstOrDefaultAsync(p => p.Id == id);
            
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

        //Aqui ele recebe o Id do PedidoId selecionado(ou seja, o produto que quer deletar)
        public async Task<IActionResult> DeletarItemCarrinho(int id)
        {            
            var pedidoItem = _context.PedidoItem.FirstOrDefault(p => p.Id == id);            

            _context.PedidoItem.Remove(pedidoItem);
            await _context.SaveChangesAsync();            

            return RedirectToAction(nameof(Details), new { id = pedidoItem.PedidoId });
            
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.Id == id);
        }
    }
}
