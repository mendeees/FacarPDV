using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Mvc;

namespace FacarPDV.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly Context _context;
        public ProdutosController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var produto = new Produtos().BuscarPorId(_context, id);
            if (produto == null) return NotFound();
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Produtos produto)
        {
            if (!ModelState.IsValid)
                return View(produto);

            produto.Alterar(_context);
            TempData["Ok"] = "Produto atualizado com sucesso.";
            return RedirectToAction("PesquisaProduto", "Pesquisa");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var produto = new Produtos().BuscarPorId(_context, id);
            if (produto == null) return NotFound();

            produto.Remover(_context);
            TempData["Ok"] = "Produto excluído com sucesso.";
            return RedirectToAction("PesquisaProduto", "Pesquisa");
        }
    }
}
