using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PapelariaMVC.Data;
using PapelariaMVC.Models;

namespace PapelariaMVC.Controllers
{
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Venda.Include(v => v.Cliente).Include(v => v.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome");
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome");
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,ProdutoId,ValorTotal,DataEmissao")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                venda.Id = Guid.NewGuid();

                // Busca o produto pelo ID
                var produto = await _context.Produto.FindAsync(venda.ProdutoId);
                if (produto == null)
                {
                    ModelState.AddModelError(string.Empty, "Produto não encontrado.");
                    return View(venda);
                }

                // Verifica se há quantidade suficiente
                if (produto.QuantidadeEstoque <= 0)
                {
                    ModelState.AddModelError(string.Empty, "Quantidade insuficiente do produto.");
                    return View(venda);
                }

                // Adiciona a venda ao contexto
                _context.Add(venda);

                // Decrementa a quantidade do produto
                produto.QuantidadeEstoque -= 1;

                // Atualiza o produto no contexto
                _context.Update(produto);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClienteId,ProdutoId,ValorTotal,DataEmissao")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Nome", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var venda = await _context.Venda.FindAsync(id);
            if (venda != null)
            {
                _context.Venda.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(Guid id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }

        // Métodos adicionais para obter totais para o dashboard

        // GET: Vendas/GetTotalVendas
        [HttpGet]
        public async Task<IActionResult> GetTotalVendas()
        {
            var totalVendas = await _context.Venda.CountAsync();
            return Json(totalVendas);
        }

        // GET: Vendas/GetValorTotalVendas
        [HttpGet]
        public async Task<IActionResult> GetValorTotalVendas()
        {
            var valorTotalVendas = await _context.Venda.SumAsync(v => v.ValorTotal);
            return Json(valorTotalVendas);
        }
    }
}
