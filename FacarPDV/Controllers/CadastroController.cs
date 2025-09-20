using Microsoft.AspNetCore.Mvc;

namespace FacarPDV.Controllers
{
    public class CadastroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastroProduto()
        {
            return View();
        }

        public IActionResult CadastroUsuario()
        {
            return View();
        }
    }
}
