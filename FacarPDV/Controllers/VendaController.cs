using Domain.Domain;
using Domain.EF;
using FacarPDV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace FacarPDV.Controllers
{
    public class VendaController : Controller
    {
        private readonly Context _context;
        private const string CART_KEY = "PDV_CART";

        public VendaController(Context context) => _context = context;

        // Modelo simples do carrinho (fica em TempData como JSON)
        public class ItemCarrinho
        {
            public int ProdutoId { get; set; }
            public string Nome { get; set; }
            public decimal Preco { get; set; }
            public int Quantidade { get; set; }
            public decimal Subtotal => Preco * Quantidade;
        }

        // --- utilitários com TempData ---
        private List<ItemCarrinho> GetCart()
        {
            var json = TempData.Peek(CART_KEY) as string; // Peek não consome
            return string.IsNullOrEmpty(json)
                ? new List<ItemCarrinho>()
                : JsonSerializer.Deserialize<List<ItemCarrinho>>(json);
        }
        private void SaveCart(List<ItemCarrinho> cart)
        {
            TempData[CART_KEY] = JsonSerializer.Serialize(cart);
        }

        // GET /Venda/Venda  (carrega selecionados + carrinho)
        [HttpGet]
        public IActionResult Venda(int? codigoCliente, string cliente, int? codigoProduto, string produto)
        {
            Cliente clienteSel = null;
            Produto produtoSel = null;

            if (codigoCliente.HasValue)
                clienteSel = _context.Set<Cliente>().AsNoTracking().FirstOrDefault(c => c.Id == codigoCliente.Value);
            else if (!string.IsNullOrWhiteSpace(cliente))
                clienteSel = _context.Set<Cliente>().AsNoTracking()
                               .FirstOrDefault(c => EF.Functions.Like(c.Nome, $"%{cliente.Trim()}%"));

            if (codigoProduto.HasValue)
                produtoSel = _context.Produto.AsNoTracking().FirstOrDefault(p => p.ProdutoId == codigoProduto.Value);
            else if (!string.IsNullOrWhiteSpace(produto))
                produtoSel = _context.Produto.AsNoTracking()
                               .FirstOrDefault(p => EF.Functions.Like(p.Nome, $"%{produto.Trim()}%"));

            var cart = GetCart();
            ViewBag.Itens = cart;
            ViewBag.Total = cart.Sum(i => i.Subtotal);
            ViewBag.Cliente = clienteSel;
            ViewBag.Produto = produtoSel;

            return View();
        }

        // POST /Venda/RegistrarVenda  (adiciona item ao carrinho)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrarVenda(int? codigoCliente, int? codigoProduto, int quantidade)
        {
            if (codigoCliente == null || codigoProduto == null || quantidade <= 0)
            {
                TempData["Erro"] = "Informe cliente, produto e quantidade.";
                return RedirectToAction(nameof(Venda), new { codigoCliente, codigoProduto });
            }

            var prod = _context.Produto.AsNoTracking().FirstOrDefault(p => p.ProdutoId == codigoProduto.Value);
            if (prod == null)
            {
                TempData["Erro"] = "Produto inválido.";
                return RedirectToAction(nameof(Venda), new { codigoCliente });
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProdutoId == prod.ProdutoId);
            if (item == null)
                cart.Add(new ItemCarrinho { ProdutoId = prod.ProdutoId, Nome = prod.Nome, Preco = prod.Preco, Quantidade = quantidade });
            else
                item.Quantidade += quantidade;

            SaveCart(cart);
            TempData["Ok"] = "Item adicionado.";
            return RedirectToAction(nameof(Venda), new { codigoCliente, codigoProduto });
        }

        // POST /Venda/RemoverItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoverItem(int produtoId, int? codigoCliente)
        {
            var cart = GetCart();
            cart.RemoveAll(i => i.ProdutoId == produtoId);
            SaveCart(cart);
            return RedirectToAction(nameof(Venda), new { codigoCliente });
        }

        // POST /Venda/FinalizarVenda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarVenda(int codigoCliente)
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Erro"] = "Carrinho vazio.";
                return RedirectToAction(nameof(Venda), new { codigoCliente });
            }

            var cliente = _context.Set<Cliente>().FirstOrDefault(c => c.Id == codigoCliente);
            if (cliente == null)
            {
                TempData["Erro"] = "Cliente inválido.";
                return RedirectToAction(nameof(Venda));
            }

            // Cria a venda
            var venda = new Venda
            {
                ClienteId = cliente.Id,
                DataEmissao = DateTime.Now,
                ValorTotal = cart.Sum(i => i.Subtotal)
            };
            _context.Set<Venda>().Add(venda);
            _context.SaveChanges();

            // Salva itens (ajuste o nome da entidade conforme seu projeto)
            var itens = cart.Select(i => new ItemVenda
            {
                VendaId = venda.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.Preco
            }).ToList();

            _context.Set<ItemVenda>().AddRange(itens);
            _context.SaveChanges();

            // --- BAIXA NO ESTOQUE ---
            foreach (var item in cart)
            {
                Estoque.SaidaEstoque(_context, item.ProdutoId, item.Quantidade);
            }

            // Limpa carrinho
            TempData.Remove(CART_KEY);
            TempData["Ok"] = $"Venda #{venda.Id} registrada com sucesso.";

            return RedirectToAction(nameof(Venda), new { codigoCliente = cliente.Id });
        }

        public IActionResult Caixa() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}