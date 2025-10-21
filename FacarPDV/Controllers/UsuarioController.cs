using FacarPDV.Data;
using FacarPDV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FacarPDV.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly FacarPdvContext _context;

        public UsuarioController(FacarPdvContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastroUsuario()
        {
            return View();
        }
        public async Task<IActionResult> CadastroNivelUsuario()
        {
            var niveis = await _context.NivelUsuario
                .AsNoTracking()
                .OrderBy(n => n.NivelId)
                .ToListAsync();

            return View(niveis);
        }
        public async Task<IActionResult> CreateNivelUsuario(NivelUsuario nivel)
        {
            if (ModelState.IsValid)
            {
                _context.NivelUsuario.Add(nivel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CadastroNivelUsuario));
            }
            var niveis = await _context.NivelUsuario.AsNoTracking().ToListAsync();
            return View("CadastroNivelUsuario", niveis);
        }

        public IActionResult AlterarNivelUsuario()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
