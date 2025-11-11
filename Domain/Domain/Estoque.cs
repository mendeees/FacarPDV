using Domain.EF;
using System.Linq;

namespace Domain.Domain
{
    public class Estoque
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public Produto? Produto { get; set; }

        /// <summary>
        /// Dá entrada no estoque de um produto.
        /// Cria o registro se ele ainda não existir.
        /// </summary>
        public static bool EntradaEstoque(Context context, int produtoId, int quantidade)
        {
            if (quantidade <= 0)
                return false;

            var estoque = context.Estoque.FirstOrDefault(e => e.ProdutoId == produtoId);

            if (estoque == null)
            {
                // Produto ainda sem estoque registrado
                estoque = new Estoque
                {
                    ProdutoId = produtoId,
                    Quantidade = quantidade
                };
                context.Estoque.Add(estoque);
            }
            else
            {
                // Atualiza quantidade existente
                estoque.Quantidade += quantidade;
            }

            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Dá saída do estoque do produto, validando saldo antes.
        /// Retorna false se o produto não existir ou se o saldo for insuficiente.
        /// </summary>
        public static bool SaidaEstoque(Context context, int produtoId, int quantidade)
        {
            if (quantidade <= 0)
                return false;

            int saldoAtual = GetSaldo(context, produtoId);

            if (saldoAtual == -1)
                return false; // Produto sem estoque cadastrado

            if (saldoAtual < quantidade)
                return false; // Saldo insuficiente

            var estoque = context.Estoque.FirstOrDefault(e => e.ProdutoId == produtoId);
            estoque.Quantidade -= quantidade;
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Retorna o saldo atual de um produto.
        /// Retorna -1 se o produto não tiver estoque registrado.
        /// </summary>
        public static int GetSaldo(Context context, int produtoId)
        {
            var estoque = context.Estoque.FirstOrDefault(e => e.ProdutoId == produtoId);
            return estoque?.Quantidade ?? -1;
        }
    }
}
