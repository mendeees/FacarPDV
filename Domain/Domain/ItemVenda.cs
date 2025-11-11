using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class ItemVenda
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;

        public Venda? Venda { get; set; }
        public Produto? Produto { get; set; }

        // --- MÉTODOS CRUD ---
        public List<ItemVenda> BuscarTodos(Context context)
        {
            return context.Set<ItemVenda>().ToList();
        }

        public ItemVenda BuscarPorId(Context context, int id)
        {
            return context.Set<ItemVenda>().FirstOrDefault(i => i.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<ItemVenda>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<ItemVenda>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<ItemVenda>().Remove(this);
            context.SaveChanges();
        }
    }
}
