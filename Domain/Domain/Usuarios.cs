using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int NivelId { get; set; }
        public NivelUsuario? Nivel { get; set; }

        // --- MÉTODOS CRUD ---

        public List<Usuarios> BuscarTodos(Context context)
        {
            return context.Usuarios.ToList();
        }

        public Usuarios BuscarPorId(Context context, int id)
        {
            return context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Usuarios.Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Usuarios.Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Usuarios.Remove(this);
            context.SaveChanges();
        }
    }
}
