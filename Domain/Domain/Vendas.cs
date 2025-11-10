using Domain.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Domain
{
    public class Vendas
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public int? ClienteId { get; set; }
        public DateTime DataEmissao { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }

        public Usuarios? Usuario { get; set; }
        public Clientes? Cliente { get; set; }
        public ICollection<ItensVenda>? Itens { get; set; }

        // --- MÉTODOS CRUD ---
        public List<Vendas> BuscarTodos(Context context)
        {
            return context.Set<Vendas>().ToList();
        }

        public Vendas BuscarPorId(Context context, int id)
        {
            return context.Set<Vendas>().FirstOrDefault(v => v.Id == id);
        }

        public void Salvar(Context context)
        {
            context.Set<Vendas>().Add(this);
            context.SaveChanges();
        }

        public void Alterar(Context context)
        {
            context.Set<Vendas>().Update(this);
            context.SaveChanges();
        }

        public void Remover(Context context)
        {
            context.Set<Vendas>().Remove(this);
            context.SaveChanges();
        }
    }
}
