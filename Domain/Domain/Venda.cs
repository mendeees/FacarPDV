using Domain.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Venda
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public int? ClienteId { get; set; }
        public DateTime DataEmissao { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }

        public Usuario? Usuario { get; set; }
        public Cliente? Cliente { get; set; }
        public ICollection<ItemVenda>? Itens { get; set; }

        // --- MÉTODOS CRUD ---
        public List<Venda> BuscarTodos(Context context)
        {
            return context.Set<Venda>().ToList();
        }

        public Venda BuscarPorId(Context context, int id)
        {
            return context.Set<Venda>().FirstOrDefault(v => v.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<Venda>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<Venda>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<Venda>().Remove(this);
            context.SaveChanges();
        }
    }
}
