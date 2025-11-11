using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FacarPDV.Controllers
{
    public class ClienteController : Controller
    {
        private readonly Context _context;
        public ClienteController(Context context) => _context = context;

        // GET: /Clientes/CadastroCliente
        [HttpGet]
        public IActionResult CadastroCliente(string q = null)
        {
            var query = _context.Set<Cliente>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var termo = q.Trim();
                query = query.Where(c =>
                    EF.Functions.Like(c.Nome, $"%{termo}%") ||
                    EF.Functions.Like(c.CPF ?? "", $"%{termo}%") ||
                    EF.Functions.Like(c.Email ?? "", $"%{termo}%"));
            }

            var clientes = query.OrderBy(c => c.Nome).ToList();
            ViewBag.Q = q;
            return View(clientes); // @model IEnumerable<Clientes>
        }

        // POST: /Clientes/CreateCliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))
                ModelState.AddModelError(nameof(cliente.Nome), "Informe o nome.");

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(CadastroCliente));

            cliente.Salvar(_context);
            TempData["Ok"] = "Cliente cadastrado com sucesso.";
            return RedirectToAction(nameof(CadastroCliente));
        }

        // GET: /Clientes/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var cliente = new Cliente().BuscarPorId(_context, id);
            if (cliente == null) return NotFound();
            return View(cliente); // @model Clientes
        }

        // POST: /Clientes/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))
                ModelState.AddModelError(nameof(cliente.Nome), "Informe o nome.");

            if (!ModelState.IsValid) return View(cliente);

            cliente.Alterar(_context);
            TempData["Ok"] = "Cliente atualizado.";
            return RedirectToAction(nameof(CadastroCliente));
        }

        // POST: /Clientes/Excluir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var cliente = new Cliente().BuscarPorId(_context, id);
            if (cliente == null) return NotFound();

            cliente.Remover(_context);
            TempData["Ok"] = "Cliente removido.";
            return RedirectToAction(nameof(CadastroCliente));
        }
    }
}
