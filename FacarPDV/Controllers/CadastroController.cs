using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Mvc;

namespace FacarPDV.Controllers
{
    public class CadastroController : Controller
    {
        private readonly Context _context;
        public CadastroController(Context context) => _context = context;

        // GET: /Cadastro/CadastroProduto
        public IActionResult CadastroProduto()
        {
            return View(new Produtos()); // casa com @model Produtos
        }

        // POST: /Cadastro/CreateProduto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduto(Produtos produto)
        {
            if (!ModelState.IsValid) return View("CadastroProduto", produto);

            produto.Salvar(_context);
            return RedirectToAction(nameof(CadastroProduto));
        }
    }
}
