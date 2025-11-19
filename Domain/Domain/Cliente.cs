using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }

        // --- MÉTODOS CRUD ---
        public List<Cliente> BuscarTodos(Context context)
        {
            return context.Set<Cliente>().ToList();
        }

        public Cliente BuscarPorId(Context context, int id)
        {
            return context.Set<Cliente>().FirstOrDefault(c => c.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<Cliente>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<Cliente>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<Cliente>().Remove(this);
            context.SaveChanges();
        }
    }
}
