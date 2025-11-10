using Domain.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class NivelUsuario
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public ICollection<Usuarios>? Usuarios { get; set; }

        // ---- MÉTODOS CRUD ----

        public List<NivelUsuario> BuscarTodos(Context context)
        {
            return context.NivelUsuario.ToList();
        }

        public NivelUsuario BuscarPorId(Context context, int id)
        {
            return context.NivelUsuario.FirstOrDefault(n => n.Id == id);
        }

        public void Salvar(Context context)
        {
            context.NivelUsuario.Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.NivelUsuario.Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.NivelUsuario.Remove(this);
            context.SaveChanges();
        }
    }
}
