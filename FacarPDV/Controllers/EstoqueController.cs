using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacarPDV.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly Context _context;
        public EstoqueController(Context context) => _context = context;

        // GET: /Estoque/Entrada
        [HttpGet]
        public IActionResult Entrada(int? id)
        {
            var model = new Estoque();
            if (id.HasValue && id.Value > 0)
                model.ProdutoId = id.Value;
            return View(model);
        }

        // POST: /Estoque/Entrada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Entrada(Estoque model)
        {
            if (model.ProdutoId <= 0 || model.Quantidade <= 0)
            {
                TempData["Erro"] = "Informe o produto e a quantidade corretamente.";
                return View(model);
            }

            bool sucesso = Estoque.EntradaEstoque(_context, model.ProdutoId, model.Quantidade);

            if (!sucesso)
            {
                TempData["Erro"] = "Não foi possível registrar a entrada de estoque.";
                return View(model);
            }

            TempData["Ok"] = "Entrada registrada com sucesso.";
            return RedirectToAction(nameof(Entrada));
        }

        // GET: /Estoque/Saida
        [HttpGet]
        public IActionResult Saida(int? id)
        {
            var model = new Estoque();
            if (id.HasValue && id.Value > 0)
                model.ProdutoId = id.Value;
            return View(model); // Nunca retorne View() sem model!
        }

        // POST: /Estoque/Saida
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Saida(Estoque model)
        {
            if (model.ProdutoId <= 0 || model.Quantidade <= 0)
            {
                TempData["Erro"] = "Informe o produto e a quantidade corretamente.";
                return View(model);
            }

            bool sucesso = Estoque.SaidaEstoque(_context, model.ProdutoId, model.Quantidade);

            if (sucesso)
            {
                TempData["Ok"] = "Saída registrada com sucesso.";
                return RedirectToAction(nameof(Saida));
            }

            int saldo = Estoque.GetSaldo(_context, model.ProdutoId);

            if (saldo == -1)
                TempData["Erro"] = "Produto ainda não possui estoque cadastrado.";
            else
                TempData["Erro"] = $"Saldo insuficiente. Saldo atual: {saldo} unidade(s).";

            return View(model);
        }
    }
}
