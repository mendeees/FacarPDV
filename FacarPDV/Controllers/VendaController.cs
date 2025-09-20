using FacarPDV.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FacarPDV.Controllers
{
    public class VendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Venda() {
            return View();
        }
        public IActionResult Caixa() {
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
