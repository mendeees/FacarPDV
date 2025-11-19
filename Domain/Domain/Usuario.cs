using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int NivelId { get; set; }
        public NivelUsuario? Nivel { get; set; }

        // --- MÉTODOS CRUD ---

        public List<Usuario> BuscarTodos(Context context)
        {
            return context.Usuario.ToList();
        }

        public Usuario BuscarPorId(Context context, int id)
        {
            return context.Usuario.FirstOrDefault(u => u.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Usuario.Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Usuario.Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Usuario.Remove(this);
            context.SaveChanges();
        }
    }
}
