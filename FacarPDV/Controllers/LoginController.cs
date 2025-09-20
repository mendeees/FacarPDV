using Microsoft.AspNetCore.Mvc;

namespace FacarPDV.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string login, string senha)
        {
            if (login == "admin" && senha == "123")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View();
        }
    }
}

