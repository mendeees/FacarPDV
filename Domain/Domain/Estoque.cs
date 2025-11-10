using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Estoques
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public Produtos? Produto { get; set; }

        // --- MÉTODOS CRUD ---
        public List<Estoques> BuscarTodos(Context context)
        {
            return context.Set<Estoques>().ToList();
        }

        public Estoques BuscarPorId(Context context, int id)
        {
            return context.Set<Estoques>().FirstOrDefault(e => e.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<Estoques>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<Estoques>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<Estoques>().Remove(this);
            context.SaveChanges();
        }
    }
}
