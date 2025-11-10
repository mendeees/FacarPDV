using Domain.Domain;
using Domain.EF;
using FacarPDV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacarPDV.Controllers
{
    public class PesquisaController : Controller
    {
        private readonly Context _context;

        public PesquisaController(Context context)
        {
            _context = context;
        }

        // === Página inicial (não usada diretamente, mas mantida) ===
        public IActionResult Index()
        {
            return View();
        }

        // === PESQUISA DE PRODUTO ===
        public IActionResult PesquisaProduto(string? nomeProduto = null, int? codigoProduto = null)
        {
            var query = _context.Produto.AsQueryable();

            if (codigoProduto.HasValue)
                query = query.Where(p => p.ProdutoId == codigoProduto.Value);

            if (!string.IsNullOrWhiteSpace(nomeProduto))
                query = query.Where(p => p.Nome.Contains(nomeProduto));

            var produtos = query.ToList();

            ViewBag.Codigo = codigoProduto;
            ViewBag.Nome = nomeProduto;

            return View(produtos);
        }

        // === PESQUISA DE VENDAS COM FILTROS E LISTAGEM ===
        [HttpGet]
        public IActionResult PesquisaVendas(
            string? codigo, string? cliente, string? pagamento = "Todos", string? status = "Todos",
            string? de = null, string? ate = null, int page = 1, int pageSize = 20)
        {
            var query = _context.Set<Vendas>()
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Select(v => new VendaListItemVM
                {
                    Id = v.Id,
                    Codigo = "V-" + v.Id.ToString("D4"),
                    Cliente = v.Cliente != null ? v.Cliente.Nome : "",
                    DataHora = v.DataEmissao,
                    Pagamento = "Pix",   // substitua pelo campo real, se existir
                    Status = "Pago",     // idem
                    Valor = v.ValorTotal
                });

            // === FILTROS ===
            if (!string.IsNullOrWhiteSpace(codigo))
                query = query.Where(x => x.Codigo.Contains(codigo.Trim(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(cliente))
                query = query.Where(x => EF.Functions.Like(x.Cliente, $"%{cliente.Trim()}%"));

            if (!string.IsNullOrWhiteSpace(pagamento) && pagamento != "Todos")
                query = query.Where(x => x.Pagamento == pagamento);

            if (!string.IsNullOrWhiteSpace(status) && status != "Todos")
                query = query.Where(x => x.Status == status);

            if (DateTime.TryParse(de, out var dataDe))
                query = query.Where(x => x.DataHora >= dataDe.Date);

            if (DateTime.TryParse(ate, out var dataAte))
                query = query.Where(x => x.DataHora < dataAte.Date.AddDays(1));

            // === PAGINAÇÃO ===
            var totalRegistros = query.Count();
            var vendas = query
                .OrderByDescending(x => x.DataHora)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // === RESUMO ===
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.Page = page;
            ViewBag.Pages = (int)Math.Ceiling(totalRegistros / (double)pageSize);
            ViewBag.HasPaging = totalRegistros > pageSize;
            ViewBag.TotalPago = vendas.Where(x => x.Status == "Pago").Sum(x => x.Valor);
            ViewBag.TotalCancelado = vendas.Where(x => x.Status == "Cancelado").Sum(x => x.Valor);

            // === FILTROS (ViewBag) ===
            ViewBag.FiltroCodigo = codigo;
            ViewBag.FiltroCliente = cliente;
            ViewBag.FiltroPagamento = pagamento;
            ViewBag.FiltroStatus = status;
            ViewBag.FiltroDe = de;
            ViewBag.FiltroAte = ate;

            return View(vendas);
        }

        // === DETALHE DE VENDA (usado no modal) ===
        [HttpGet]
        public IActionResult DetalheVenda(int id)
        {
            var venda = _context.Set<Vendas>()
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Include(v => v.Itens!)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(v => v.Id == id);

            if (venda == null)
                return Content("<div class='text-danger'>Venda não encontrada.</div>", "text/html");

            var itens = venda.Itens ?? new List<ItensVenda>();

            var html = $@"
                <div class='mb-2'>
                    <strong>Cliente:</strong> {venda.Cliente?.Nome ?? "-"}<br/>
                    <strong>Data:</strong> {venda.DataEmissao:dd/MM/yyyy HH:mm}<br/>
                    <strong>Total:</strong> {venda.ValorTotal:C}
                </div>
                <div class='table-responsive'>
                    <table class='table table-sm'>
                        <thead class='table-light'>
                            <tr>
                                <th>Produto</th>
                                <th>Qtd</th>
                                <th class='text-end'>Preço</th>
                                <th class='text-end'>Subtotal</th>
                            </tr>
                        </thead>
                        <tbody>";

            foreach (var i in itens)
            {
                html += $@"
                    <tr>
                        <td>{(i.Produto?.Nome ?? i.ProdutoId.ToString())}</td>
                        <td>{i.Quantidade}</td>
                        <td class='text-end'>{i.PrecoUnitario:C}</td>
                        <td class='text-end'>{(i.PrecoUnitario * i.Quantidade):C}</td>
                    </tr>";
            }

            html += "</tbody></table></div>";

            return Content(html, "text/html");
        }
    }
}
