using Domain.Domain;
using Domain.EF;
using FacarPDV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacarPDV.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string login, string senha)
        {
            // Busca no banco o usuário com o login e senha informados
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == login && u.Senha == senha);

            if (usuario != null)
            {
                // Login correto → redireciona
                return RedirectToAction("Index", "Home");
            }

            // Login incorreto → mostra mensagem
            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View();
        }
    }
}
