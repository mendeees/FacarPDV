using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = string.Empty;

        // ---- MÉTODOS CRUD ----

        public List<Produto> BuscarTodos(Context context)
        {
            return context.Produto.ToList();
        }

        public Produto BuscarPorId(Context context, int id)
        {
            return context.Produto.FirstOrDefault(p => p.ProdutoId == id);
        }

        public void Salvar(Context context)
        {
            context.Produto.Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Produto.Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Produto.Remove(this);
            context.SaveChanges();
        }
    }
}
