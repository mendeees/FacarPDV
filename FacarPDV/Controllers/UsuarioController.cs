using Domain.Domain;
using Domain.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FacarPDV.Controllers
{
    [LoginFilter]
    [AdminFilter]
    public class UsuarioController : Controller
    {
        private readonly Context _context;
        public UsuarioController(Context context) => _context = context;

        // ===== Cadastro de usuário =====
        [HttpGet]
        public IActionResult CadastroUsuario()
        {
            return View(new Usuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CadastroUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            // login único
            if (_context.Usuario.Any(u => u.Login == usuario.Login))
            {
                ModelState.AddModelError(nameof(usuario.Login), "Login já está em uso.");
                return View(usuario);
            }

            // define nível padrão se vier 0/sem valor
            if (usuario.NivelId <= 0)
            {
                var nivelPadraoId = _context.NivelUsuario
                    .OrderBy(n => n.Id)
                    .Select(n => n.Id)
                    .FirstOrDefault();

                if (nivelPadraoId == 0)
                {
                    var novo = new NivelUsuario { Descricao = "Padrão" };
                    _context.NivelUsuario.Add(novo);
                    _context.SaveChanges();
                    nivelPadraoId = novo.Id;
                }

                usuario.NivelId = nivelPadraoId;
            }

            usuario.Salvar(_context);
            return RedirectToAction(nameof(AlterarNivelUsuario));
        }

        // ===== CRUD de Níveis =====
        [HttpGet]
        public IActionResult CadastroNivelUsuario()
        {
            var niveis = new NivelUsuario().BuscarTodos(_context);
            return View(niveis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNivelUsuario(NivelUsuario nivel)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(CadastroNivelUsuario));

            if (_context.NivelUsuario.Any(n => n.Descricao == nivel.Descricao))
            {
                TempData["ErroNivel"] = "Já existe um nível com essa descrição.";
                return RedirectToAction(nameof(CadastroNivelUsuario));
            }

            nivel.Salvar(_context);
            TempData["OkNivel"] = "Nível cadastrado com sucesso.";
            return RedirectToAction(nameof(CadastroNivelUsuario));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirNivelUsuario(int id)
        {
            var nivel = new NivelUsuario().BuscarPorId(_context, id);
            if (nivel == null) return NotFound();

            if (_context.Usuario.Any(u => u.NivelId == id))
            {
                TempData["ErroNivel"] = "Não é possível excluir: há usuários nesse nível.";
                return RedirectToAction(nameof(CadastroNivelUsuario));
            }

            nivel.Remover(_context);
            TempData["OkNivel"] = "Nível removido.";
            return RedirectToAction(nameof(CadastroNivelUsuario));
        }

        // ===== Listar/Buscar usuários + carregar combos =====
        [HttpGet]
        public IActionResult AlterarNivelUsuario(string q = null, int? codigo = null)
        {
            var query = _context.Usuario
                .AsNoTracking()
                .Include(u => u.Nivel)
                .AsQueryable();

            if (codigo.HasValue)
                query = query.Where(u => u.Id == codigo.Value);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var termo = q.Trim();
                query = query.Where(u =>
                    EF.Functions.Like(u.Nome, $"%{termo}%") ||
                    EF.Functions.Like(u.Login, $"%{termo}%"));
            }

            var usuario = query
                .OrderBy(u => u.Nome)
                .ToList();

            ViewBag.Niveis = _context.NivelUsuario
                .AsNoTracking()
                .OrderBy(n => n.Descricao)
                .ToList();

            // manter valores da busca na view
            ViewBag.Q = q;
            ViewBag.Codigo = codigo;

            return View(usuario);
        }

        // ===== Salvar alteração de nível (POST separado) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarNivelUsuario(int id, int nivelId)
        {
            var usuario = _context.Usuario.Find(id);
            if (usuario == null) return NotFound();

            var nivelExiste = _context.NivelUsuario.Any(n => n.Id == nivelId);
            if (!nivelExiste)
            {
                TempData["Erro"] = "Nível inválido.";
                return RedirectToAction(nameof(AlterarNivelUsuario));
            }

            usuario.NivelId = nivelId;
            _context.SaveChanges();

            TempData["Ok"] = "Nível atualizado.";
            return RedirectToAction(nameof(AlterarNivelUsuario));
        }
    }
}
