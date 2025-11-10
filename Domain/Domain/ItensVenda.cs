using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class ItensVenda
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;

        public Vendas? Venda { get; set; }
        public Produtos? Produto { get; set; }

        // --- MÉTODOS CRUD ---
        public List<ItensVenda> BuscarTodos(Context context)
        {
            return context.Set<ItensVenda>().ToList();
        }

        public ItensVenda BuscarPorId(Context context, int id)
        {
            return context.Set<ItensVenda>().FirstOrDefault(i => i.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<ItensVenda>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<ItensVenda>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<ItensVenda>().Remove(this);
            context.SaveChanges();
        }
    }
}
