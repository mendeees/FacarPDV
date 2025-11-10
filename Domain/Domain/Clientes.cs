using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }

        // --- MÉTODOS CRUD ---
        public List<Clientes> BuscarTodos(Context context)
        {
            return context.Set<Clientes>().ToList();
        }

        public Clientes BuscarPorId(Context context, int id)
        {
            return context.Set<Clientes>().FirstOrDefault(c => c.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<Clientes>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<Clientes>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<Clientes>().Remove(this);
            context.SaveChanges();
        }
    }
}
